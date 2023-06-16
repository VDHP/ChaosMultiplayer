using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button createLobbyButton;
    [SerializeField] Button quickJoinButton;
    [SerializeField] Button joinWithCodeButton;
    [SerializeField] CreateLobbyUI createLobbyUI;
    [SerializeField] TMP_InputField joinCodeInputField;
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] Transform lobbyContainer;
    [SerializeField] Transform lobbyTemplate;
    [SerializeField] Transform InteractUI;

    public static LobbyUI Instance { get; private set; }
    private void Awake()
    {
        
        mainMenuButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            createLobbyUI.Show();
            HideInteractUI();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.QuickJoin();
        });
        joinWithCodeButton.onClick.AddListener(() =>
        {
             KitchenGameLobby.Instance.JoinWithCode(joinCodeInputField.text);
        });

        lobbyTemplate.gameObject.SetActive(false);

        Instance = this;
    }
    private void Start()
    {
        playerNameInputField.text = KitchenObjectMultiplayer.Instance.GetPlayerName();

        playerNameInputField.onValueChanged.AddListener((string newPlayerName) =>
        {
            KitchenObjectMultiplayer.Instance.SetPlayerName(newPlayerName);
        });

        KitchenGameLobby.Instance.OnListLobbyChanged += KitchenGameLobby_OnListLobbyChanged;

        UpdateLobbyList(new List<Lobby>());
    }

    private void KitchenGameLobby_OnListLobbyChanged(object sender, KitchenGameLobby.OnListLobbyChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }
    void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyTemplateSingleUI>().SetLobby(lobby);
        }
    }
    private void OnDestroy()
    {
        KitchenGameLobby.Instance.OnListLobbyChanged -= KitchenGameLobby_OnListLobbyChanged;
    }
    public void HideInteractUI()
    {
        InteractUI.gameObject.SetActive (false);
    }
    public void ShowInteractUI()
    {
        InteractUI.gameObject.SetActive (true);

        createLobbyButton.Select();
    }
}
