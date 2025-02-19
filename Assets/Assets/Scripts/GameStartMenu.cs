using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject start;
    public GameObject mainMenu;
    public GameObject options;
    public GameObject about;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button tutorialButton;
    public Button skipTutorialButton;
    public Button optionButton;
    public Button aboutButton;
    public Button quitButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(EnableStart);
        skipTutorialButton.onClick.AddListener(StartGame);
        tutorialButton.onClick.AddListener(StartTutorial);
        optionButton.onClick.AddListener(EnableOption);
        aboutButton.onClick.AddListener(EnableAbout);
        quitButton.onClick.AddListener(QuitGame);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(2);
    }

    public void StartTutorial()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }
    
    public void HideAll()
    {
        start.SetActive(false);
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(false);
    }

    public void EnableMainMenu()
    {
        start.SetActive(false);
        mainMenu.SetActive(true);
        options.SetActive(false);
        about.SetActive(false);
    }
    public void EnableOption()
    {
        start.SetActive(false);
        mainMenu.SetActive(false);
        options.SetActive(true);
        about.SetActive(false);
    }
    public void EnableAbout()
    {
        start.SetActive(false);
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(true);
    }
    public void EnableStart()
    {
        start.SetActive(true);
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(false);
    }

}
