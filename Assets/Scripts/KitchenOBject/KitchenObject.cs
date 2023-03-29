using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;
    IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSo()
    {
        return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent newkitchenObjectParrent )
    {
        if(kitchenObjectParent != null)
        {   
            kitchenObjectParent.ClearKitchenObject();
        }
        kitchenObjectParent = newkitchenObjectParrent;  
        if(kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("Counter already have KitchenObject!");
        }
        kitchenObjectParent.SetKitchenObject(this); 

        /*transform.parent = kitchenObjectParent.GetKitchenFollowTransform();
        transform.localPosition = Vector3.zero;*/
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenObjectMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }
    public bool TryGetPlates(out PlatesKitchenObject platesKitchenObject)
    {
        if(this is PlatesKitchenObject)
        {
            platesKitchenObject = this as PlatesKitchenObject;
            return true;
        }
        else
        {
            platesKitchenObject=null;
            return false;
        }
    }
}
