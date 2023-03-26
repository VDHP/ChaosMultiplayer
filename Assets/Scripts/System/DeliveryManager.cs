using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] float timeToSpawnOrder;
    [SerializeField] float amountOrderMax;
    [SerializeField] RecipeListSO recipeListSO;

    List<RecipeSO> waitingRecipeSOList;

    public static DeliveryManager Instance { get; private set; }

    public event Action OnRecipeSpawn;
    public event Action OnRecipeComplete;
    public event Action OnDeliverySucced;
    public event Action OnDeliveryFailded;

    float amountOrder;
    int successRecipeAmount;

    private void Awake()
    {
        Instance= this;
        waitingRecipeSOList = new List<RecipeSO>();
        
    }
    private void Start()
    {
        StartCoroutine(CountTimeSpawnOrder());
    }
    IEnumerator CountTimeSpawnOrder()
    {
        // amount of Recipe Waiting current 
        amountOrder = 0;
        while (true)
        {
            if (amountOrder < amountOrderMax && GameManager.Instance.IsGamePlaying())
            {
                yield return new WaitForSeconds(timeToSpawnOrder);

                amountOrder++;
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0,recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawn?.Invoke();
            }
            else
            {
                yield return null;
            }
        }
    }
    public void DeliverRecipe(PlatesKitchenObject platesKitchenObject)
    {
        for(int i = 0; i < waitingRecipeSOList?.Count; i++)
        {
            RecipeSO recipeSO = waitingRecipeSOList[i];
            if(recipeSO.kitchenObjectSOList.Count == platesKitchenObject.GetKitchenObjectSOList().Count)
            {
                // has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in recipeSO.kitchenObjectSOList)
                {
                    // Cycling through all ingredients in the Recipe
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in platesKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredients in the plate
                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
                        {   
                            // Ingredient matches!
                            ingredientFound=true;
                            break;
                        }
                    }
                    // this recipe ingredient was not found on the plate
                    if(!ingredientFound)
                    {
                        plateContentsMatchesRecipe =false;
                        break;
                    }
                }
                if(plateContentsMatchesRecipe)
                {
                    //player delivered the correct recipe   
                    successRecipeAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    amountOrder--;
                    OnRecipeComplete?.Invoke();
                    OnDeliverySucced?.Invoke();
                    return;
                }
            }
        }
        // player deliverd the wrong recipe
        OnDeliveryFailded?.Invoke();
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetSuccessRecipeDelivery()
    {
        return successRecipeAmount;
    }
}
