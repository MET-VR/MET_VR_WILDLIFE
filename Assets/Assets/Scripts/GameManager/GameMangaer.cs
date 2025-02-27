using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using static OVRPlugin;

public class GameMangaer : MonoBehaviour
{
    [Header("References")]
    public GameObject[] GameObjects;
    public Material TextrueBlender; // Blending material
    public FadeScreen FadeScreen;
    public XRBodyTransformer move;
    public Button[] Steps;
    public WaterTrigger watatirgger;
    public TextMeshProUGUI Debuger;

    [Header("Variables")]
    public int GameStage;
    public float lerpValue = 0; // Track blending value
    public int currentStep = 0; // Track the current cleaning step
    public int triggers;

    private Color disabledColor = Color.gray;
    private Color normalColor = Color.white;
    private List<string> correctOrder = new List<string> { "Chlorhexidine", "10% bleach", "water1", "water2" };

    /* GameObjects List
    0. Start Menu
    1. Bird 1
    2. Bird 2
    3. Bird 3
    4. Syringe
    5. Glass 1
    6. Glass 2
    7. Glass 3
    8. Glass 4
    9. Tweezers
    10. Disinfect Step
    11. Player
    */

    void Start()
    {
        for (int i = 0; i <= 10; i++)
        {
            GameObjects[i].SetActive(false);
        }
        GameObjects[0].SetActive(true);
        //GameObjects[10].SetActive(true);
        GameStage = 0;
        GameObjects[9].GetComponent<Renderer>().material = TextrueBlender;
        Steps[0].interactable = false;
        Steps[0].GetComponent<Image>().color = disabledColor;
        TextrueBlender.SetFloat("_LerpValue", 0); // Reset blend to full rust
        triggers = 0;
        //StartGame();
    }

    void Update()
    {
        TextrueBlender.SetFloat("_LerpValue", lerpValue); // Update material in real time
        //ResetTweezers();
        if (triggers == 1)
        {
            IncreaseLerpValue(0.2f,0.5f);
        }
        if (triggers == 3)
        {
            IncreaseLerpValue(0.2f, 1);
        }
    }

    public void IncreaseLerpValue(float amount, float maxValue)
    {
        lerpValue = Mathf.Clamp(lerpValue + amount, 0, maxValue); // Blend up (max 0.5)
        UpdateDebugText();

    }

    public void DecreaseLerpValue(float amount, float minValue)
    {
        lerpValue = Mathf.Clamp(lerpValue - amount, minValue, 1); // Blend down to 0
    }
    public void CheckStep(string stepName)
    {
        
    }

    public void ResetTweezers()
    {
        //currentStep = 0;
        triggers = 0;
        lerpValue = 0; // Reset material
        TextrueBlender.SetFloat("_LerpValue", lerpValue);
        GameObjects[9].transform.localPosition = new Vector3(-3.373f, 15.5578f, -57.18f); // Adjust using localPosition
        UpdateDebugText("Reset");
    }

    public void UpdateDebugText(string lastTrigger = "None")
    {
        if (Debuger != null)
        {
            Debuger.text = $"Tigger Value: {triggers}\n" +
                           $"Last Trigger: {lastTrigger}\n" +
                           $"Lerp Value: {lerpValue:F2}";
        }
    }

    public void NextStep()
    {
       
    }

    public void StartGame()
    {
        FadeScreen.FadeOut();
        StartCoroutine(BetweenFade());
    }

    public IEnumerator BetweenFade()
    {
        yield return new WaitForSeconds(FadeScreen.fadeDuration * 2);
        if (GameStage == 0)
        {
            GameObjects[0].SetActive(false);
            for (int i = 5; i <= 10; i++)
            {
                GameObjects[i].SetActive(true);
            }
            GameObjects[11].transform.position = new Vector3(-3.37264681f, GameObjects[11].transform.position.y, -2.5655787f);
        }
        GameStage += 1;
        FadeScreen.FadeIn();
    }
}
