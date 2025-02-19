using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjustmentGPT : MonoBehaviour
{
    public Transform Cameraoffset;
    public float heightAmount;

    void Start()
    {
        // Set camera position to the saved height from HeightManager
        Cameraoffset.position = new Vector3(Cameraoffset.position.x, HeightManager.Instance.playerHeight, Cameraoffset.position.z);
    }

    void Update()
        {
            //Debug.Log("Current Player Height: " + HeightManager.Instance.playerHeight);
        }

    // Adds height
    public void AddHeight()
    {
        HeightManager.Instance.playerHeight += heightAmount; // Increase height
        UpdateCameraHeight(); // Update the camera position
    }

    // Subtracts height
    public void MinusHeight()
    {
        HeightManager.Instance.playerHeight -= heightAmount; // Decrease height
        UpdateCameraHeight(); // Update the camera position
    }

    // Update the camera height based on the current player height
    private void UpdateCameraHeight()
    {
        Cameraoffset.position = new Vector3(Cameraoffset.position.x, HeightManager.Instance.playerHeight, Cameraoffset.position.z);
    }
}
