using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    [SerializeField] CuttingRecipeSO[] cuttingKitchenObjectSOArray;

    public event Action<float> OnHasProgressTimeChanged;
    public static event EventHandler OnAnyCut;

    float cuttingProgressTime;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    public override void Interact(PlayerController playerController)
    {
        // There is no Kitchen Object here
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject() && HasRecipeWithInput(playerController.GetKitchenObject().GetKitchenObjectSO()))
            {
                // player carrying something and it can be cut
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        // There is a Kitchen Object here
        else
        {
            // player has empty handy
            if (!playerController.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                InteractLogicPlaceObjectOnCounterServerRpc();
            }
            else
            {
                // player is holding something
                if (playerController.GetKitchenObject().TryGetPlates(out PlatesKitchenObject platesKitchenObject))
                {
                    // player is holding a plate
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // ingredient is valid 
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                    }
                }
            }
        }
    }
    [ServerRpc(RequireOwnership =false)]
    void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }
    [ClientRpc]
    void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        cuttingProgressTime = 0;
        OnHasProgressTimeChanged?.Invoke(0);
    }
    public override void InteractAlternate(PlayerController playerController)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
                CutObjectServerRpc();
                TestingCuttingProgressDoneServerRpc();
        } 
    }
    [ServerRpc(RequireOwnership =false)]
    void CutObjectServerRpc()
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectClientRpc();
        }
    }
    [ClientRpc]
    void CutObjectClientRpc() {
        cuttingProgressTime++;
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

        float progressTimeNormalize = cuttingProgressTime / cuttingRecipeSO.cuttingProgressMaxTime;
        OnHasProgressTimeChanged?.Invoke(progressTimeNormalize);
    }
    [ServerRpc(RequireOwnership =false)]
    void TestingCuttingProgressDoneServerRpc()
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            if (cuttingProgressTime >= cuttingRecipeSO.cuttingProgressMaxTime)
            {
                /// There has a Kitchen Object and can be cut and Cutting Progress is done
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }
    public KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSOInput)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSOInput);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output; 
        }
        else { return null; }
    }
    public bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSOInput)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSOInput);
        return cuttingRecipeSO!= null;       
    }
    public CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingKitchenObjectSOArray)
        {
            if (cuttingRecipeSO.input == kitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    } 
}

