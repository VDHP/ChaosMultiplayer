using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] GameObject readyObject;
    [SerializeField] PlayerVisual playerVisual;
    [SerializeField] Button kickButton;
    [SerializeField] GameObject canvas;
    [SerializeField] TextMeshPro playerNameText;

    private void Awake()
    {
        kickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = KitchenObjectMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);
            KitchenGameLobby.Instance.KickPlayer(playerData.playerID.ToString());
            KitchenObjectMultiplayer.Instance.KickPlayer(playerData.clientID);
        });
    }
    private void Start()
    {
        KitchenObjectMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectedReady.Instance.OnReadyChanged += CharacterSelectedReady_OnReadyChanged;


        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

        if(playerIndex == 0)
        {
            canvas.SetActive(false);
        }
        UpdatePlayer();
    }

    private void CharacterSelectedReady_OnReadyChanged()
    {
        UpdatePlayer();
    }

    private void KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged()
    {
        UpdatePlayer();
    }
    void UpdatePlayer()
    {
        if(KitchenObjectMultiplayer.Instance.IsPlayerConnectedWithIndex(playerIndex))
        {
            Show();

            PlayerData playerData = KitchenObjectMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);

            readyObject.SetActive(CharacterSelectedReady.Instance.IsPlayerReady(playerData.clientID));

            playerNameText.text = playerData.playerName.ToString();
            
            playerVisual.SetPlayerColor(KitchenObjectMultiplayer.Instance.GetPlayerColor(playerData.colorID)); 
        }
        else
        {
            Hide();
        }
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        KitchenObjectMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged;
    }
}
