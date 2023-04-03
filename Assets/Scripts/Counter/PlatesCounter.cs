using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class PlatesCounter : BaseCounter
{
    float spawnPlateTime;
    float spawnPlateTimeMax = 4f;
    int plateSpawnAmount;
    int spawnPlateAmountMax = 4;

    [SerializeField] KitchenObjectSO plateKitchenObjectSO;  

    public event Action OnPlateSpawn;
    public event Action OnPlateRemoved;
    // Update is called once per frame
    void Update()
    {
        if(!IsServer) return;
        if (plateSpawnAmount < spawnPlateAmountMax && GameManager.Instance.IsGamePlaying())
        {
            spawnPlateTime += Time.deltaTime;
            if (spawnPlateTime > spawnPlateTimeMax)
            {
                SpawnPlateSObjectClientRpc();
            }
        }
    }
    [ClientRpc]
    void SpawnPlateSObjectClientRpc()
    {
        spawnPlateTime = 0f;
        plateSpawnAmount++;
        OnPlateSpawn?.Invoke();
    }
    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasKitchenObject())
        {
            /// player is empty handed
            if(plateSpawnAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, playerController);
                InteractLogicServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    void InteractLogicClientRpc()
    {
        plateSpawnAmount--;

        OnPlateRemoved?.Invoke();
    }
}
