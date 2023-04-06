using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button optionButton;

    public static GamePauseUI Instance { get; private set; }
    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionButton.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show();
            Hide();
        });
        
        Instance= this;
    }

    private void Start()
    {
        GameManager.Instance.OnLocalPausedGame += Instance_OnLocalPausedGame;
        GameManager.Instance.OnLocalUnPausedGame += Instance_OnLocalUnPausedGame;

        Hide();
    }

    private void Instance_OnLocalUnPausedGame()
    {
        Hide();
    }

    private void Instance_OnLocalPausedGame()
    {
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
