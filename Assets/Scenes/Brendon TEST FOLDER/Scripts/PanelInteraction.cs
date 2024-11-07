using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInteraction : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false); // Start with the panel hidden
    }

    // Trigger event for VR interaction (adjust as per interaction style)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Or use specific VR input
        {
            panel.SetActive(true); // Show the panel on interaction
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panel.SetActive(false); // Hide the panel when no longer interacting
        }
    }
}