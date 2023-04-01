using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;
    FollowTransform followTransform;

    IKitchenObjectParent kitchenObjectParent;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }
    public KitchenObjectSO GetKitchenObjectSo()
    {
        return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent newkitchenObjectParrent )
    {
        SetKitchenObjectParentServerRpc(newkitchenObjectParrent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership =false)]
    void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }
    [ClientRpc]
    void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent newkitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject();
        }
        kitchenObjectParent = newkitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("Counter already have KitchenObject!");
        }
        kitchenObjectParent.SetKitchenObject(this);
        followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenFollowTransform());
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void ParentClearKitchenObject()
    {
        kitchenObjectParent.ClearKitchenObject();
    }
    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenObjectMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
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
