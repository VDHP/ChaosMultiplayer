using System;
using System.Collections;
using System.Collections.Generic;
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
            if (playerController.HasKitchenObject() && HasRecipeWithInput(playerController.GetKitchenObject().GetKitchenObjectSo()))
            {
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgressTime = 0;
            }
        }
        // There is a Kitchen Object here
        else
        {
            // player has empty handy
            if (!playerController.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                OnHasProgressTimeChanged?.Invoke(0);
            }
            else
            {
                // player is holding something
                if (playerController.GetKitchenObject().TryGetPlates(out PlatesKitchenObject platesKitchenObject))
                {
                    // player is holding a plate
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        // ingredient is valid 
                        GetKitchenObject().DestroySelf();

                    }
                }
            }
        }
    }
    public override void InteractAlternate(PlayerController playerController)
    {
        if (HasKitchenObject())
        {
            if (HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
            {
                cuttingProgressTime++;
                OnAnyCut?.Invoke(this,EventArgs.Empty);

                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());

                float progressTimeNormalize = cuttingProgressTime / cuttingRecipeSO.cuttingProgressMaxTime;
                OnHasProgressTimeChanged?.Invoke(progressTimeNormalize);
                if (cuttingProgressTime >= cuttingRecipeSO.cuttingProgressMaxTime)
                {
                    /// There has a Kitchen Object and can be cut and Cutting Progress is done
                    KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                }
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

