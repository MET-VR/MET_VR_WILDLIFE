using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameMangaer : MonoBehaviour
{
    [Header("References")]
    public GameObject[] GameObjects;
    public FadeScreen FadeScreen;
    public XRBodyTransformer move;
    public Button Step1;
    public Button Step2;
    public Button Step3;

    [Header("Variables")]
    public bool Hasbeengrabbed = false;
    public bool Hasmoved = false;
    public int Tutorialstep = 0;

    private Color disabledColor = Color.gray;
    private Color normalColor = Color.white;

    /* GameObjects List
    0. Start Game Prompt
    1. Step 1 prompt
    2. Chair 1
    3. Player (XR Origin)
    4. Bird
    5. Step 2
    6. Step 3
    */


    private Vector3 targetPosition = new Vector3(-3.25f, 0.225030482f, -0.439999998f); // Position to check for movement
    private Quaternion initialRotation; // To store the initial rotation

    void Start()
    {
        move.enabled = false;
        GameObjects[0].SetActive(true);
        FadeScreen.FadeIn();
        GameObjects[4].SetActive(false);
        GameObjects[5].SetActive(false);
        GameObjects[6].SetActive(false);
        GameObjects[1].SetActive(false);

        // Disable button and set its color to gray
        if (Step1 != null)
        {
            Step1.interactable = false;
            Step2.interactable = false;
            Step3.interactable = false;
            Step1.GetComponent<Image>().color = disabledColor;
            Step2.GetComponent<Image>().color = disabledColor;
            Step3.GetComponent<Image>().color = disabledColor;
        }

        // Store the initial rotation of the player
        initialRotation = GameObjects[3].transform.rotation;
    }

    void Update()
    {
        if (Tutorialstep == 2)
        {
            // Check if the player has moved from the initial position
            if (Vector3.Distance(GameObjects[3].transform.position, targetPosition) > 1f) // 0.5f is the threshold for movement
            {
                Hasmoved = true;
                if (Step2 != null)
                {
                    Step2.interactable = true; // Enable Step 2 button
                    Step2.GetComponent<Image>().color = normalColor; // Change color to normal
                }
            }
        }
        else if (Tutorialstep == 3)
        {
            // Check for the player's rotation change
            float rotationDifference = Quaternion.Angle(initialRotation, GameObjects[3].transform.rotation);

            // If rotation difference exceeds a threshold, enable the third button
            if (rotationDifference > 45f) // Adjust the threshold as needed
            {
                if (Step3 != null)
                {
                    Step3.interactable = true;
                    Step3.GetComponent<Image>().color = normalColor; // Change color to normal
                }
            }
        }
    }

    public void OnGrab()
    {
        Hasbeengrabbed = true;

        // Enable Step 1 button and change its color
        if (Step1 != null)
        {
            Step1.interactable = true;
            Step1.GetComponent<Image>().color = normalColor;
        }
    }

    public void NextStep()
    {
        StartGame();
    }

    public void StartGame()
    {
        FadeScreen.FadeOut();
        StartCoroutine(BetweenFade());
    }

    public IEnumerator BetweenFade()
    {
        yield return new WaitForSeconds(FadeScreen.fadeDuration * 2);
        if (Tutorialstep == 0)
        {
            GameObjects[0].SetActive(false);
            GameObjects[2].SetActive(false);
            GameObjects[1].SetActive(true);
            GameObjects[3].transform.position = new Vector3(-1.84000003f, 0.225030482f, 1.47000003f);
            GameObjects[4].SetActive(true);

        }
        else if (Tutorialstep == 1)
        {
            GameObjects[1].SetActive(false);
            GameObjects[3].transform.position = new Vector3(-3.25f, 0.225030482f, -0.439999998f);
            GameObjects[4].SetActive(false);
            GameObjects[5].SetActive(true);
            move.enabled = true;
        }
        else if (Tutorialstep == 2)
        {
            GameObjects[5].SetActive(false);
            GameObjects[6].SetActive(true);
        }

        Tutorialstep += 1;
        FadeScreen.FadeIn();
    }
}
