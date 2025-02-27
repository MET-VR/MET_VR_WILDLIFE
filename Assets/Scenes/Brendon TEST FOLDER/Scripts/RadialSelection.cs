using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RadialSelection : MonoBehaviour
{
    public InputActionProperty spawnButton;
    public AudioManager1 AM;

    public List<Vector3> handReplacementRotations;
    public List<Vector3> handReplacementScales;

    [Range(2, 10)]
    public int numberOfRadialParts = 4; // A default value to avoid uninitialized
    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;
    public float angleBetweenParts = 10;
    public Transform leftHandTransform; // Hand for selection
    public Transform rightHandTransform; // Hand to replace
    public GameObject rightHandModel; // Default hand model

    public UnityEvent<int> OnPartSelected;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private int currentHoveredRadialPart = -1;

    private string hoverColorHex = "#da5834";
    private Color hoverColor;

    // List of objects to replace the right hand
    public List<GameObject> handReplacementObjects;
    public List<GameObject> secondaryObjects; // Second-stage replacement objects
    public List<GameObject> resetTriggers; // Objects that reset to the original tweezer

    private GameObject currentReplacementObject;
    private bool isSecondaryObjectActive = false; // Tracks if second model is active

    public int rightHandOptionIndex = -1; // Set this in Inspector

    // Tweezer replacement logic variables
    public GameObject tweezers; // Original tweezer model
    public GameObject tweezersWithFood; // Tweezer model with food

    private void OnEnable() => spawnButton.action.Enable();
    private void OnDisable() => spawnButton.action.Disable();
    private void Start() => hoverColor = HexToColor(hoverColorHex);

    private void Update()
    {
        if (spawnButton.action.WasPressedThisFrame()) ActivateRadialMenu();
        if (spawnButton.action.IsPressed()) GetHoveredRadialPart();
        if (spawnButton.action.WasReleasedThisFrame()) HideAndTriggerSelection();
    }

    public void ActivateRadialMenu()
    {
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        spawnPosition.y -= 0.1f;
        radialPartCanvas.position = spawnPosition;
        radialPartCanvas.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        radialPartCanvas.gameObject.SetActive(true);
        AM.PlaySFX(AM.openMenu);

        ClearSpawnedParts();

        for (int i = 0; i < numberOfRadialParts; i++)
        {
            float angle = -i * 360 / numberOfRadialParts - angleBetweenParts / 2;
            Vector3 radialPartEulerAngle = new Vector3(0, 0, angle);

            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);
            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngle;
            spawnedRadialPart.GetComponent<Image>().fillAmount = 1f / (numberOfRadialParts + angleBetweenParts / 360); // Adjust fill amount properly

            spawnedParts.Add(spawnedRadialPart);
        }
    }

    private void ClearSpawnedParts()
    {
        foreach (var item in spawnedParts) Destroy(item);
        spawnedParts.Clear();
    }

    public void GetHoveredRadialPart()
    {
        Vector3 centerToHand = leftHandTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);
        if (angle < 0) angle += 360;

        currentHoveredRadialPart = (int)(angle * numberOfRadialParts / 360);

        for (int i = 0; i < spawnedParts.Count; i++)
        {
            var image = spawnedParts[i].GetComponent<Image>();
            image.color = (i == currentHoveredRadialPart) ? hoverColor : Color.white;
            spawnedParts[i].transform.localScale = (i == currentHoveredRadialPart) ? 1.1f * Vector3.one : Vector3.one;
            if (i == currentHoveredRadialPart) AM.PlaySFX(AM.menuSelection);
        }
    }

    public void HideAndTriggerSelection()
    {
        radialPartCanvas.gameObject.SetActive(false);
        ReplaceRightHand(currentHoveredRadialPart);
    }

    private void ReplaceRightHand(int partIndex)
    {
        if (partIndex == rightHandOptionIndex)
        {
            rightHandModel.SetActive(true);
            DestroyCurrentReplacementObject();
        }
        else if (partIndex >= 0 && partIndex < handReplacementObjects.Count)
        {
            DestroyCurrentReplacementObject();
            rightHandModel.SetActive(false);

            currentReplacementObject = Instantiate(handReplacementObjects[partIndex], rightHandTransform);
            currentReplacementObject.transform.localPosition = Vector3.zero;

            if (partIndex < handReplacementRotations.Count)
                currentReplacementObject.transform.localRotation = Quaternion.Euler(handReplacementRotations[partIndex]);
            else
                currentReplacementObject.transform.localRotation = Quaternion.identity;

            if (partIndex < handReplacementScales.Count)
                currentReplacementObject.transform.localScale = handReplacementScales[partIndex];
            else
                currentReplacementObject.transform.localScale = Vector3.one;

            if (handReplacementObjects[partIndex] != null)
            {
                var collider = currentReplacementObject.AddComponent<TweezerCollision>();
                collider.radialSelection = this;
            }
        }
    }

    private void DestroyCurrentReplacementObject()
    {
        if (currentReplacementObject != null)
        {
            Destroy(currentReplacementObject);
            currentReplacementObject = null;
        }
    }

    public void ReplaceTweezerWithFood()
    {
        // Disable the original tweezer and enable the tweezers with food
        tweezers.SetActive(false);
        tweezersWithFood.SetActive(true);
    }

    public void ReplaceTweezerWithSecondary(GameObject newModel)
    {
        DestroyCurrentReplacementObject();
        currentReplacementObject = Instantiate(newModel, rightHandTransform);
        currentReplacementObject.transform.localPosition = Vector3.zero;
        isSecondaryObjectActive = true;

        var collider = currentReplacementObject.AddComponent<TweezerCollision>();
        collider.radialSelection = this;
        collider.isResetTrigger = true;
    }

    public void ResetToTweezer()
    {
        ReplaceRightHand(rightHandOptionIndex);
        isSecondaryObjectActive = false;
        tweezers.SetActive(true);
        tweezersWithFood.SetActive(false);
    }

    private Color HexToColor(string hex)
    {
        hex = hex.Replace("#", "");
        if (hex.Length == 6)
        {
            float r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            float g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            float b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            return new Color(r, g, b);
        }
        return Color.white;
    }
}