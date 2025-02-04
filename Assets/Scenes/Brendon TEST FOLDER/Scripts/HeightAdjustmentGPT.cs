using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeightAdjustmentGPT : MonoBehaviour
{
    public Transform cameraOffset; // Assign the XR Origin's Camera Offset Transform
    public TMP_Text heightText; // Assign UI Text to display the height
    public Button increaseButton; // Assign the "+" button
    public Button decreaseButton; // Assign the "-" button

    public float heightAmount = 0.1f; // Adjust height by 0.1m per button press

    void Start()
    {
        // Load stored height from HeightManager
        cameraOffset.position = new Vector3(cameraOffset.position.x, HeightManager.Instance.playerHeight, cameraOffset.position.z);
        UpdateUI();

        // Add button click listeners
        increaseButton.onClick.AddListener(AddHeight);
        decreaseButton.onClick.AddListener(MinusHeight);
    }

    // Adds height
    public void AddHeight()
    {
        HeightManager.Instance.playerHeight += heightAmount;
        UpdateCameraHeight();
    }

    // Subtracts height
    public void MinusHeight()
    {
        HeightManager.Instance.playerHeight -= heightAmount;
        UpdateCameraHeight();
    }

    // Update the camera height based on the current player height
    private void UpdateCameraHeight()
    {
        cameraOffset.position = new Vector3(cameraOffset.position.x, HeightManager.Instance.playerHeight, cameraOffset.position.z);
        UpdateUI();
    }

    // Updates the UI to show the height as a number
    private void UpdateUI()
    {
        if (heightText != null)
        {
            heightText.text = HeightManager.Instance.playerHeight.ToString("F2"); // Just the number
        }
    }
}
