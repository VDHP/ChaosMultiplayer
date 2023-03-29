using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenFollowTransform();

    public bool HasKitchenObject();

    public void SetKitchenObject(KitchenObject kitchenObjectPr);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public NetworkObject GetNetworkObject();
}
