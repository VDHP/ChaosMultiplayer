using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] Button playeSinglePlayerButton;
    [SerializeField] Button playeMultiPlayerButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        playeMultiPlayerButton.onClick.AddListener(() =>
        {
            KitchenObjectMultiplayer.isMultiplayerGame = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        playeSinglePlayerButton.onClick.AddListener(() =>
        {
            KitchenObjectMultiplayer.isMultiplayerGame = false;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale= 1.0f;
    }
     
}
