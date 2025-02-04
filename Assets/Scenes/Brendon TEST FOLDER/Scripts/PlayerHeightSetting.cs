using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine.UI;

public class PlayerHeightSetting : MonoBehaviour
{
    public XROrigin xrOrigin; // Assign your XR Origin in Inspector
    public TMP_Text heightText; // Assign UI Text (only shows number)
    public Button increaseButton; // Assign "+"
    public Button decreaseButton; // Assign "-"

    private float playerHeight = 1.0f; // Default height
    private const float heightStep = 0.5f; // Change amount per button press
    private const float minHeight = 0.5f; // Minimum height
    private const float maxHeight = 2.5f; // Maximum height

    void Start()
    {
        // Load stored height from PlayerPrefs (or use default)
        if (PlayerPrefs.HasKey("playerHeight"))
            playerHeight = PlayerPrefs.GetFloat("playerHeight");

        ApplyHeight();

        // Add button click listeners
        increaseButton.onClick.AddListener(IncreaseHeight);
        decreaseButton.onClick.AddListener(DecreaseHeight);
    }

    public void IncreaseHeight()
    {
        if (playerHeight + heightStep <= maxHeight)
        {
            playerHeight += heightStep;
            ApplyHeight();
        }
    }

    public void DecreaseHeight()
    {
        if (playerHeight - heightStep >= minHeight)
        {
            playerHeight -= heightStep;
            ApplyHeight();
        }
    }

    private void ApplyHeight()
    {
        if (xrOrigin != null)
        {
            // Manually adjust camera position instead of CameraYOffset
            Vector3 cameraPosition = xrOrigin.Camera.transform.localPosition;
            cameraPosition.y = playerHeight; // Set new height
            xrOrigin.Camera.transform.localPosition = cameraPosition;

            // Save the new height in PlayerPrefs
            PlayerPrefs.SetFloat("playerHeight", playerHeight);
            PlayerPrefs.Save();
        }

        // Update UI text (only the number)
        if (heightText != null)
        {
            heightText.text = playerHeight.ToString("F2"); // Only show number (e.g., "1.20")
        }
    }
}
