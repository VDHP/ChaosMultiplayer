using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] TextMeshProUGUI messageText;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    // Start is called before the first frame update
    void Start()
    {
        KitchenObjectMultiplayer.Instance.OnFailedToJoinGame += KitchenObjectMultiplayer_OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void KitchenGameLobby_OnQuickJoinFailed()
    {
        ShowMessage("Cound not find a Lobby to Quick Join!");
    }

    private void KitchenGameLobby_OnJoinStarted()
    {
        ShowMessage("Joining Lobby ...");
    }

    private void KitchenGameLobby_OnJoinFailed()
    {
        ShowMessage("Failed to Join Lobby! \n Make sure your code is correct");
    }

    private void KitchenGameLobby_OnCreateLobbyStarted()
    {
        ShowMessage("Creating Lobby ...");
    }

    private void KitchenGameLobby_OnCreateLobbyFailed()
    {
        ShowMessage("Failed to create Lobby!");
    }
    void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }
    private void KitchenObjectMultiplayer_OnFailedToJoinGame()
    {
        if(NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
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
        KitchenObjectMultiplayer.Instance.OnFailedToJoinGame -= KitchenObjectMultiplayer_OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyFailed -= KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnCreateLobbyStarted -= KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnJoinFailed -= KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnJoinStarted -= KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnQuickJoinFailed -= KitchenGameLobby_OnQuickJoinFailed;
    }
}
