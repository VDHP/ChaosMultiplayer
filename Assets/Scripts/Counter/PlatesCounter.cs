using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        if (plateSpawnAmount < spawnPlateAmountMax && GameManager.Instance.IsGamePlaying())
        {
            spawnPlateTime += Time.deltaTime;
            if (spawnPlateTime > spawnPlateTimeMax)
            {
                spawnPlateTime = 0f;
                plateSpawnAmount++;
                OnPlateSpawn?.Invoke();
            }
        }
    }
    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasKitchenObject())
        {
            /// player is empty handed
            if(plateSpawnAmount > 0)
            {
                plateSpawnAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, playerController);

                OnPlateRemoved?.Invoke();
            }
        }
    }
}
