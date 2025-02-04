using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;

public class PlayerHeightDisplay : MonoBehaviour
{
    public XROrigin xrOrigin; // Assign your XROrigin in the Inspector
    public TMP_Text heightText; // Assign your TextMeshPro UI Text

    void Update()
    {
        if (xrOrigin != null && heightText != null)
        {
            float playerHeight = xrOrigin.CameraInOriginSpaceHeight;
            heightText.text = playerHeight.ToString("F2");
        }
    }
}
