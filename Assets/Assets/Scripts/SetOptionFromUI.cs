using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class SetOptionFromUI : MonoBehaviour
{
    public Scrollbar volumeSlider;
    public XROrigin xrOrigin;
    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetGlobalVolume);
        
        if (PlayerPrefs.HasKey("playerHeight"))
        SetPlayerHeight(PlayerPrefs.GetFloat("playerHeight"));
    }

    public void SetGlobalVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetPlayerHeight(float value)
    {
        if(xrOrigin != null)
        {
            xrOrigin.CameraYOffset = value;
            PlayerPrefs.SetFloat("playerHeight", value);
        }
    }
}
