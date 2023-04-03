using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    public override void Interact(PlayerController playerController)
    {
        // There is no Kitchen Object here
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        // There is a Kitchen Object here
        else
        {
            if(!playerController.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
            }
            else
            {
                // player is carrying something
                if(playerController.GetKitchenObject().TryGetPlates(out PlatesKitchenObject platesKitchenObject))
                {
                    // player is holding a plate
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // ingredient is valid 
                        
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
                // player isn't carrying plates but something else
                else if(GetKitchenObject().TryGetPlates(out platesKitchenObject))
                {
                    // counter is holding a plate
                    if (platesKitchenObject.TryAddIngredient(playerController.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(playerController.GetKitchenObject());
                    }
                }
            }
        }
    } 
}
