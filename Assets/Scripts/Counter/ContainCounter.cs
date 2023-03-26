using System;
using System.Collections;
using System.Collections.Generic;
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
            OnAnimateAction?.Invoke();
        }
        else
        {
            if(playerController.GetKitchenObject().GetKitchenObjectSo() == kitchenObjectSO)
            {
                Destroy(playerController.GetKitchenObject().gameObject);
                playerController.ClearKitchenObject();
                OnAnimateAction?.Invoke();
            }
        }
    }
}
