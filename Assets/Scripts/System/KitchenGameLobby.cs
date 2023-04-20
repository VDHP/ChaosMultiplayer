using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenGameLobby : MonoBehaviour
{
    public static KitchenGameLobby Instance { get; private set; }

    public event Action OnCreateLobbyStarted;
    public event Action OnCreateLobbyFailed;
    public event Action OnJoinStarted;
    public event Action OnQuickJoinFailed;
    public event Action OnJoinFailed;
    public event EventHandler<OnListLobbyChangedEventArgs> OnListLobbyChanged;

    public class OnListLobbyChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    Lobby joinedLobby;
    float heartBeatTimer;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }
    private void Update()
    {
        HandleHeartBeat();
    }
    private void Start()
    {
        StartCoroutine(RefreshLobbyList());
    }
    IEnumerator RefreshLobbyList()
    {
        while(true)
        {
            if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn)
            {
                ListLobbies();
            }
            Debug.Log(Time.realtimeSinceStartup);
            yield return new WaitForSeconds(3);
        }
    }
    private void HandleHeartBeat()
    {
        if (IsLobbyHost())
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer <= 0)
            {
                float heartBeatTimerMax = 15f;
                heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }
    bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0, 1000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        int playerAmount = KitchenObjectMultiplayer.MAX_PLAYER_AMOUNT;
        try
        {
            OnCreateLobbyStarted?.Invoke();
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, playerAmount, new CreateLobbyOptions
            {
                IsPrivate = isPrivate
            });

            KitchenObjectMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.SelectCharacterScene);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnCreateLobbyFailed?.Invoke();
        }
    }
    public async void QuickJoin()
    {
        OnJoinStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            KitchenObjectMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnQuickJoinFailed?.Invoke();
        }
    }
    public Lobby GetLobby()
    {
        return joinedLobby;
    }
    public async void JoinWithCode(string lobbyCode)
    {
        try
        {
            OnJoinStarted?.Invoke();
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            KitchenObjectMultiplayer.Instance.StartClient();

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke();
        }
    }
    public async void JoinWithID(string lobbyID)
    {
        try
        {
            OnJoinStarted?.Invoke();
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID);
            KitchenObjectMultiplayer.Instance.StartClient();

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke();
        }
    }
    async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            OnListLobbyChanged?.Invoke(this, new OnListLobbyChangedEventArgs
            {
                lobbyList = queryResponse.Results
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
    public async void KickPlayer(string playerID)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerID);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}
