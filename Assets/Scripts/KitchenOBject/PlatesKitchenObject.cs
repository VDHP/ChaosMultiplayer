using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesKitchenObject : KitchenObject
{
    [SerializeField] List<KitchenObjectSO> validKitchenObjectSOList;
    List<KitchenObjectSO> kitchenObjectSOList;

    public static event Action<PlatesKitchenObject> OnIngredientAddedSound;
    public event Action<KitchenObjectSO> OnIngredientAdded;
    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList= new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // not valid ingredient
            return false;
        }
        // already has this type
        if(kitchenObjectSOList.Contains(kitchenObjectSO)) return false;
        else
        {
            AddIngredientServerRpc(KitchenObjectMultiplayer.Instance.GetKitchenObjectSOIndexOf(kitchenObjectSO));
            return true;
        }
    }
    [ServerRpc(RequireOwnership =false)]
    void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientCLientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    void AddIngredientCLientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenObjectMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(kitchenObjectSO);
        OnIngredientAddedSound?.Invoke(this);
    }
    public static void ResetStaticData()
    {
        OnIngredientAddedSound = null;
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
