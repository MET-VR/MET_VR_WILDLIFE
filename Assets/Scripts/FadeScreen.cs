using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor;
    public AnimationCurve fadeCurve;
    public string colorPropertyName = "_Color";

    private Renderer rend;
    private static FadeScreen instance; // Singleton to prevent multiple fade screens

    void Awake()
    {
        // Singleton pattern to prevent duplicates when loading scenes
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object when loading new scenes
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;

        if (fadeOnStart)
            FadeIn();

        // Listen for when a new scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeIn(); // Fade in when a new scene is loaded
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }
    
    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        rend.enabled = true;

        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, fadeCurve.Evaluate(timer / fadeDuration));

            rend.material.SetColor(colorPropertyName, newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        rend.material.SetColor(colorPropertyName, newColor2);

        if (alphaOut == 0)
            rend.enabled = false;
    }
}
