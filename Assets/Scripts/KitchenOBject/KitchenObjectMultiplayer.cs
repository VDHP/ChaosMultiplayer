using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenObjectMultiplayer : NetworkBehaviour
{
    public static KitchenObjectMultiplayer Instance { get; private set; }

    public event Action OnTryingToJoinGame;
    public event Action OnFailedToJoinGame;
    public event Action OnPlayerDataNetworkListChanged;
    
    [SerializeField] KitchenObjectSOList kitchenObjectSOsList;
    [SerializeField] List<Color> playerColorList;
    

     public const int MAX_PLAYER_AMOUNT = 4;

    NetworkList<PlayerData> playerDataNetworkList;
    
    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;

        NetworkManager.Singleton.StartHost();

    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientID)
    {
        for(int i =0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientID == clientID)
            {
                // this player disconnected 
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientID)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientID = clientID,
            colorID = GetFirstUnUsuedColorID()
        });
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if(SceneManager.GetActiveScene().name != Loader.Scene.SelectCharacterScene.ToString() && connectionApprovalRequest.ClientNetworkId != 0)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }
        if(NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }
        connectionApprovalResponse.Approved = true;
    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke();

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();

    }

    private void NetworkManager_Client_OnClientDisconnectCallback(ulong obj)
    {
        OnFailedToJoinGame?.Invoke();
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndexOf(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab); 
        NetworkObject kitchenNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        // take network object that was originally sent 
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }
    public int GetKitchenObjectSOIndexOf(KitchenObjectSO kitchenObjectSO)
    {
        return kitchenObjectSOsList.kitchenObjectSOsList.IndexOf(kitchenObjectSO);
    }
    public KitchenObjectSO GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex)
    {
        return kitchenObjectSOsList.kitchenObjectSOsList[kitchenObjectSOIndex];
    }
    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }
    [ServerRpc(RequireOwnership =false)]
     void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenNetworkObjectRefernce)
    {
        kitchenNetworkObjectRefernce.TryGet(out NetworkObject kitchenNetworkObject);
        KitchenObject kitchenObject =  kitchenNetworkObject.GetComponent<KitchenObject>();

        ParentClearKitchenObjectClientRpc(kitchenNetworkObjectRefernce);
        kitchenObject.DestroySelf();
    }
    [ClientRpc]
    void ParentClearKitchenObjectClientRpc(NetworkObjectReference kitchenNetworkObjectRefernce)
    {
        kitchenNetworkObjectRefernce.TryGet(out NetworkObject kitchenNetworkObject);
        KitchenObject kitchenObject = kitchenNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ParentClearKitchenObject();
    }

    public bool IsPlayerConnectedWithIndex(int index) { 
        return index < playerDataNetworkList.Count;
    }

    public PlayerData GetPlayerDataFromIndex(int playerIndex) {
        return playerDataNetworkList[playerIndex];
    }
    public Color GetPlayerColor(int colorID)
    {
        return playerColorList[colorID];
    }
    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromFromClientID(NetworkManager.Singleton.LocalClientId);
    }
    public PlayerData GetPlayerDataFromFromClientID(ulong clientID)
    {
        foreach(PlayerData playerData in playerDataNetworkList)
        {
            if(playerData.clientID == clientID)
            {
                return playerData;
            }
        }
        return default;
    }
    public int GetPlayerDataIndexFromFromClientID(ulong clientID)
    {
        for(int i =0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientID == clientID)
            {
                return i;
            }
        }
        return -1; 
    }
    public void ChangePlayerColor(int colorID)
    {
        ChangePlayerColorServerRpc(colorID);
    }
    [ServerRpc(RequireOwnership =false)]
    void ChangePlayerColorServerRpc(int colorID, ServerRpcParams serverRpcParams = default)
    {
        if(!IsColorAvailable(colorID))
        {
            // Color is not available
            return;
        }
        // Color is available
        int playerDataIndex = GetPlayerDataIndexFromFromClientID(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.colorID = colorID;
        playerDataNetworkList[playerDataIndex] = playerData;

    }
    bool IsColorAvailable(int colorID)
    {
        foreach(PlayerData playerData in playerDataNetworkList)
        {
            if (playerData.colorID == colorID)
            {
                return false;
            } 
        }
        return true; 
    }
    int GetFirstUnUsuedColorID()
    {
        for(int i = 0;i < playerColorList.Count;i++)
        {
            if (IsColorAvailable(i)) return i;
        }
        return -1;
    }
    public void KickPlayer(ulong clientID)
    {
        NetworkManager.Singleton.DisconnectClient(clientID);
        NetworkManager_Server_OnClientDisconnectCallback(clientID);
    }
}
