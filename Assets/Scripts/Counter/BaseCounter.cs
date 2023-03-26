using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] Transform topPointCounter;

    KitchenObject kitchenObject;

    public static event EventHandler OnDropSomethingHere;

    public static void ResetStaticData()
    {
        OnDropSomethingHere = null;
    }
    public virtual void Interact(PlayerController playerController)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate(PlayerController playerController)
    {
        Debug.LogError("BaseCounter.InteractAlternate();");
    }
    public Transform GetKitchenFollowTransform()
    {
        return topPointCounter;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    public void SetKitchenObject(KitchenObject kitchenObjectPr)
    {
        kitchenObject = kitchenObjectPr;

        if(kitchenObject != null)
        {
            OnDropSomethingHere?.Invoke(this,EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
}
