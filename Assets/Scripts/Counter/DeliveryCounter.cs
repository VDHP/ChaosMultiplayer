using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set;}
    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(PlayerController playerController)
    {
        if (playerController.HasKitchenObject())
        {
            if(playerController.GetKitchenObject().TryGetComponent(out PlatesKitchenObject platesKitchenObject))
            {
                // Only accepts plates
                DeliveryManager.Instance.DeliverRecipe(platesKitchenObject);

                playerController.GetKitchenObject().DestroySelf();
            }
        }         
    }
}
