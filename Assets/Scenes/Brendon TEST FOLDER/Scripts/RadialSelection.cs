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
    public Transform handTransform;

    public UnityEvent<int> OnPartSelected;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private int currentHoveredRadialPart = -1; // Change to track hovered part

    // Hex color code for the hover color
    private string hoverColorHex = "#da5834"; // Your desired hover color
    private Color hoverColor; // To store the converted color

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
        // Convert the hex color code to a Color object
        hoverColor = HexToColor(hoverColorHex);
    }

    void Update()
    {
        if (spawnButton.action.WasPressedThisFrame())
        {
            SpawnRadialPart();
        }
        if (spawnButton.action.IsPressed())
        {
            GetHoveredRadialPart(); // Check for hovering instead of selection
        }
        if (spawnButton.action.WasReleasedThisFrame())
        {
            HideAndTriggerSelected();
        }
    }

    public void HideAndTriggerSelected()
    {
        OnPartSelected.Invoke(currentHoveredRadialPart);
        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetHoveredRadialPart()
    {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);

        if (angle < 0)
            angle += 360;

        //Debug.Log($"Angle: {angle}, Hovered Part: {currentHoveredRadialPart}");

        
        // Calculate the hovered part index
        currentHoveredRadialPart = (int)(angle * numberOfRadialParts / 360);

        for (int i = 0; i < spawnedParts.Count; i++)
        {
            if (i == currentHoveredRadialPart)
            {
                spawnedParts[i].GetComponent<Image>().color = hoverColor; // Use the hover color
                spawnedParts[i].transform.localScale = 1.1f * Vector3.one; // Scale up the hovered part
            }
            else
            {
                spawnedParts[i].GetComponent<Image>().color = Color.white; // Default color for unhovered parts
                spawnedParts[i].transform.localScale = Vector3.one; // Reset scale for unhovered parts
            }
        }
    }

    public void SpawnRadialPart()
{
    // Calculate the position directly in front of the camera
    Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;

    // Lowers the spawn position of the radial menu
    spawnPosition.y -= 0.1f;

    // Set the radial part canvas to the calculated position
    radialPartCanvas.position = spawnPosition;

    // Set the rotation to align the canvas to the camera's forward direction
    radialPartCanvas.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

    // Activate the radial menu canvas
    radialPartCanvas.gameObject.SetActive(true);

    // Clear previous radial parts
    foreach (var item in spawnedParts)
    {
        Destroy(item);
    }

    spawnedParts.Clear();

    for (int i = 0; i < numberOfRadialParts; i++)
    {
        float angle = -i * 360 / numberOfRadialParts - angleBetweenParts / 2;
        Vector3 radialPartEulerAngle = new Vector3(0, 0, angle);

        GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);
        spawnedRadialPart.transform.position = radialPartCanvas.position; // Use the canvas position
        spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngle;

        spawnedRadialPart.GetComponent<Image>().fillAmount = 1 / (float)numberOfRadialParts - (angleBetweenParts / 360);

        spawnedParts.Add(spawnedRadialPart);
    }
}


    // Helper method to convert hex to Color
    private Color HexToColor(string hex)
    {
        hex = hex.Replace("#", ""); // Remove the hash at the start if it's there

        // Parse the hex code
        if (hex.Length == 6)
        {
            float r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            float g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            float b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            return new Color(r, g, b);
        }

        // Return white if the hex code is invalid
        return Color.white;
    }
}
