using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Unity.Netcode;

public class StoveCounter : BaseCounter,IHasProgress
{
    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOArray;
    private FryingRecipeSO fryingRecipeSO;
    BurningRecipeSO burningRecipeSO;

    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>();
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>();
    NetworkVariable<State> state = new NetworkVariable<State>(State.Raw);

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
    public override void OnNetworkSpawn()
    {   
        fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }
    void State_OnValueChanged(State previousState,State newState)
    {
        OnStateChanged?.Invoke(state.Value); 
        if(state.Value == State.Burned || state.Value == State.Raw)
        {
            OnHasProgressTimeChanged?.Invoke(0);
        }
    }
    void FryingTimer_OnValueChanged(float  previousValue, float newValue)
    {
        float fryingTimeMax = fryingRecipeSO != null ?  fryingRecipeSO.fryingTimerMax : 1f;
        timeNormalized = fryingTimer.Value / fryingTimeMax;
        OnHasProgressTimeChanged?.Invoke(timeNormalized);
    }
    void BurningTimer_OnValueChanged(float  previousValue, float newValue)
    {
        float burningTimerMax = burningRecipeSO != null ? burningRecipeSO.burningTimerMax : 1f;
        timeNormalized = burningTimer.Value / burningTimerMax;
        OnHasProgressTimeChanged?.Invoke(timeNormalized);
    }

    private void Update()
    {
        if(!IsServer) { return; }
        if (HasKitchenObject())
        {
            switch (state.Value)
            {
                case State.Raw:
                    break;
                case State.Frying:
                    fryingTimer.Value += Time.deltaTime;
                    
                    if (fryingTimer.Value > fryingRecipeSO.fryingTimerMax)
                    {
                        KitchenObject kitchenObject = GetKitchenObject();
                        KitchenObject.DestroyKitchenObject(kitchenObject);
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state.Value = State.Fried;
                        SetBurningRecipeSOClientRpc(KitchenObjectMultiplayer.Instance.GetKitchenObjectSOIndexOf(GetKitchenObject().GetKitchenObjectSO()));
                        burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.output);
                        burningTimer.Value = 0f;
                    }
                    break;  
                case State.Fried:
                    burningTimer.Value += Time.deltaTime;
                    if (burningTimer.Value > burningRecipeSO.burningTimerMax)
                    {
                        KitchenObject kitchenObject = GetKitchenObject();
                        KitchenObject.DestroyKitchenObject(kitchenObject);
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state.Value = State.Burned;
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
            if (playerController.HasKitchenObject() && HasFryingnRecipeWithInput(playerController.GetKitchenObject().GetKitchenObjectSO()))
            {
                // player carrying something can be fired
                KitchenObject kitchenObject = playerController.GetKitchenObject();
                kitchenObject.SetKitchenObjectParent(this);

                int kitchentObjectSOIndex = KitchenObjectMultiplayer.Instance.GetKitchenObjectSOIndexOf(kitchenObject.GetKitchenObjectSO());
                InteractLogicPlaceObjectOnStoveCounterServerRpc( kitchentObjectSOIndex);
                
            }
        }
        // There is a Kitchen Object here
        else
        {
            if (!playerController.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                SetStateIdleServerRpc();
            }
            else
            {
                if (playerController.GetKitchenObject().TryGetPlates(out PlatesKitchenObject platesKitchenObject))
                {
                    // player is holding a plate
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // ingredient is valid 
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        SetStateIdleServerRpc();
                    }
                }
            }
        }
    }
    [ServerRpc(RequireOwnership =false)]
    void SetStateIdleServerRpc()
    {
        state.Value = State.Raw;
    }
    [ServerRpc(RequireOwnership =false)]
    void InteractLogicPlaceObjectOnStoveCounterServerRpc(int kitchentObjectSOIndex)
    {
        fryingTimer.Value = 0;
        SetFryingRecipeSOClientRpc(kitchentObjectSOIndex);

        state.Value = State.Frying;
    }
    [ClientRpc]
    void SetFryingRecipeSOClientRpc(int kitchentObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenObjectMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchentObjectSOIndex);
        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);

    }
    [ClientRpc]
    void SetBurningRecipeSOClientRpc(int kitchentObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenObjectMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchentObjectSOIndex);
        burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSO);

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
        return state.Value == State.Fried;
    }
}
