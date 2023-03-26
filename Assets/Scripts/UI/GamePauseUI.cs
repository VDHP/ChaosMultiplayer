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
        GameManager.Instance.OnPausedGame += Instance_OnPausedGame;
        GameManager.Instance.OnUnPausedGame += Instance_OnUnPausedGame;

        Hide();
    }

    private void Instance_OnUnPausedGame()
    {
        Hide();
    }

    private void Instance_OnPausedGame()
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
