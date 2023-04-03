using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnThrownSomethingInHere;
    public override void Interact(PlayerController playerController)
    {
        if(playerController.HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(playerController.GetKitchenObject());

            OnThrownSomethingInHere?.Invoke(this, EventArgs.Empty);
        }
    }
    new public static void ResetStaticData()
    {
        OnThrownSomethingInHere = null;
    }
}
