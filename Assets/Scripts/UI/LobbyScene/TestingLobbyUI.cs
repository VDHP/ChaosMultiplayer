using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] Button createGameButton;    
    [SerializeField] Button joinGameButton;

    private void Awake()
    {
        createGameButton.onClick.AddListener(()=>{
            KitchenObjectMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.SelectCharacterScene);
        });
        joinGameButton.onClick.AddListener(()=>{
            KitchenObjectMultiplayer.Instance.StartClient();
        });
    }
}
