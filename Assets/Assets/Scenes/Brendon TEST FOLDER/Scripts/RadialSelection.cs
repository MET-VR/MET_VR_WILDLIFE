using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RadialSelection : MonoBehaviour
{
    public InputActionProperty spawnButton;

    [Range(2, 10)]
    public int numberOfRadialParts;
    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;
    public float angleBetweenParts = 10;
    public Transform leftHandTransform; // The left hand (for activating and selecting)
    public Transform rightHandTransform; // The right hand (to replace with the selected object)
    public GameObject rightHandModel; // The default right hand model

    public UnityEvent<int> OnPartSelected;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private int currentHoveredRadialPart = -1;

    private string hoverColorHex = "#da5834";
    private Color hoverColor;

    // List of objects to replace the right hand
    public List<GameObject> handReplacementObjects;
    private GameObject currentReplacementObject;

    // Index for the "right hand" option in the radial menu
    public int rightHandOptionIndex = -1; // Set this in the Inspector or code to the correct index

    void OnEnable()
    {
        spawnButton.action.Enable();
    }

    void OnDisable()
    {
        spawnButton.action.Disable();
    }

    void Start()
    {
        hoverColor = HexToColor(hoverColorHex);
    }

    void Update()
    {
        if (spawnButton.action.WasPressedThisFrame())
        {
            ActivateRadialMenu();
        }
        if (spawnButton.action.IsPressed())
        {
            GetHoveredRadialPart();
        }
        if (spawnButton.action.WasReleasedThisFrame())
        {
            HideAndTriggerSelection();
        }
    }

    public void ActivateRadialMenu()
    {
        // Position the radial menu
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        spawnPosition.y -= 0.1f;
        radialPartCanvas.position = spawnPosition;
        radialPartCanvas.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        radialPartCanvas.gameObject.SetActive(true);

        // Clear previous radial parts
        foreach (var item in spawnedParts)
        {
            Destroy(item);
        }

        spawnedParts.Clear();

        // Create new radial parts
        for (int i = 0; i < numberOfRadialParts; i++)
        {
            float angle = -i * 360 / numberOfRadialParts - angleBetweenParts / 2;
            Vector3 radialPartEulerAngle = new Vector3(0, 0, angle);

            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);
            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngle;

            spawnedRadialPart.GetComponent<Image>().fillAmount = 1 / (float)numberOfRadialParts - (angleBetweenParts / 360);

            spawnedParts.Add(spawnedRadialPart);
        }
    }

    public void GetHoveredRadialPart()
    {
        Vector3 centerToHand = leftHandTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);

        if (angle < 0)
            angle += 360;

        currentHoveredRadialPart = (int)(angle * numberOfRadialParts / 360);

        for (int i = 0; i < spawnedParts.Count; i++)
        {
            if (i == currentHoveredRadialPart)
            {
                spawnedParts[i].GetComponent<Image>().color = hoverColor;
                spawnedParts[i].transform.localScale = 1.1f * Vector3.one;
            }
            else
            {
                spawnedParts[i].GetComponent<Image>().color = Color.white;
                spawnedParts[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void HideAndTriggerSelection()
    {
        radialPartCanvas.gameObject.SetActive(false);

        // Replace the right hand with the selected object
        ReplaceRightHand(currentHoveredRadialPart);
    }

    private void ReplaceRightHand(int partIndex)
    {
        if (partIndex == rightHandOptionIndex)
        {
            // If the selected part is the "right hand" option, show the default hand model
            rightHandModel.SetActive(true);

            // Destroy any replacement object if it exists
            if (currentReplacementObject != null)
            {
                Destroy(currentReplacementObject);
                currentReplacementObject = null;
            }
        }
        else if (partIndex >= 0 && partIndex < handReplacementObjects.Count)
        {
            // If the selected part is a replacement object
            if (currentReplacementObject != null)
            {
                Destroy(currentReplacementObject);
            }

            // Hide the default hand model
            rightHandModel.SetActive(false);

            // Instantiate the replacement object
            currentReplacementObject = Instantiate(handReplacementObjects[partIndex], rightHandTransform);
            currentReplacementObject.transform.localPosition = Vector3.zero;
            currentReplacementObject.transform.localRotation = Quaternion.identity;
        }
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
