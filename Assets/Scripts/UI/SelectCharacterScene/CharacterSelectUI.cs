using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] Button menuButton;
    [SerializeField] Button readyButton;

    private void Awake()
    {
        menuButton.onClick.AddListener(()=>{
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);    
        });
        readyButton.onClick.AddListener(() =>
        {
            CharacterSelectedReady.Instance.SetPlayerReady();
        });
    }
}
