using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainCounter : BaseCounter
{
    public event Action OnAnimateAction;
    [SerializeField] KitchenObjectSO kitchenObjectSO;
    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, playerController);
            InteractLogicServerRpc();
        }
        /*else
        {
            if(playerController.GetKitchenObject().GetKitchenObjectSo() == kitchenObjectSO)
            {
                Destroy(playerController.GetKitchenObject().gameObject);
                playerController.ClearKitchenObject();
                InteractLogicServerRpc();
            }
        }*/
    }
    [ServerRpc(RequireOwnership =false)]
    void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    void InteractLogicClientRpc()
    {
        OnAnimateAction?.Invoke();
    }
}
