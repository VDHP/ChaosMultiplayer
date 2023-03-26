using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter,IHasProgress
{
    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOArray;
    private FryingRecipeSO fryingRecipeSO;
    BurningRecipeSO burningrecipeSO;

    private float fryingTimer;
    private float burningTimer;
    float timeNormalized;

    public event Action<float> OnHasProgressTimeChanged;
    public event Action<State> OnStateChanged;
    public enum State
    {
        Raw,
        Frying,
        Fried,
        Burned
    }
    State state;
    private void Start()
    {
        state = State.Raw;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Raw:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    timeNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax;
                    OnHasProgressTimeChanged(timeNormalized);
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningrecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                        burningTimer = 0f;
                        OnStateChanged?.Invoke(state);
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    timeNormalized = burningTimer / burningrecipeSO.burningTimerMax;
                    OnHasProgressTimeChanged(timeNormalized);
                    if (burningTimer > burningrecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningrecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(state);
                        OnHasProgressTimeChanged(burningTimer);
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }
    public override void Interact(PlayerController playerController)
    {
        // There is no Kitchen Object here
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject() && HasFryingnRecipeWithInput(playerController.GetKitchenObject().GetKitchenObjectSo()))
            {
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                burningrecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());

                fryingTimer = 0;
                state = State.Frying;
                OnStateChanged?.Invoke(state);
            }
        }
        // There is a Kitchen Object here
        else
        {
            if (!playerController.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                state = State.Raw;
                OnStateChanged?.Invoke(state);
                OnHasProgressTimeChanged?.Invoke(0);
            }
            else
            {
                if (playerController.GetKitchenObject().TryGetPlates(out PlatesKitchenObject platesKitchenObject))
                {
                    // player is holding a plate
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        // ingredient is valid 
                        GetKitchenObject().DestroySelf();
                        state = State.Raw;
                        OnStateChanged?.Invoke(state);
                        OnHasProgressTimeChanged?.Invoke(0);
                    }
                }
            }
        }
    }
    public KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSOInput)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSOInput);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else { return null; }
    }
    public bool HasFryingnRecipeWithInput(KitchenObjectSO kitchenObjectSOInput)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSOInput);
        return fryingRecipeSO != null;
    }
    public FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == kitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    public BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == kitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
    public bool IsFired()
    {
        return state == State.Fried;
    }
}
