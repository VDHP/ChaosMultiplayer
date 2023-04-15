using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class KitchenGameLobby : MonoBehaviour
{
    public static KitchenGameLobby Instance { get; private set; }

    Lobby joinedLobby;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(Random.Range(0, 1000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        int playerAmount = KitchenObjectMultiplayer.MAX_PLAYER_AMOUNT;
        try
        {
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
        }
    }
    public async void QuickJoin()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            KitchenObjectMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
