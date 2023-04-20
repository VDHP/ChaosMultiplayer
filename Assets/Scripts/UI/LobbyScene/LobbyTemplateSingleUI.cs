using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTemplateSingleUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lobbyNameText;

    Lobby lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.JoinWithID(lobby.Id);
        });
    }
    public void SetLobby(Lobby newLobby)
    {
        lobby = newLobby;

        lobbyNameText.text = newLobby.Name;
    }
}
