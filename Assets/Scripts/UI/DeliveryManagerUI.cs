using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Transform recipeTemplates;
    private void Awake()
    {
        recipeTemplates.gameObject.SetActive(false);
    }
    void Start()
    {
        DeliveryManager.Instance.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
        DeliveryManager.Instance.OnRecipeSpawn += DeliveryManger_OnRecipeSpawn;
        UpdateVisual();
    }

    private void DeliveryManger_OnRecipeSpawn()
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeComplete()
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        foreach(Transform child in container)
        {
            if (child == recipeTemplates) continue;
            Destroy(child.gameObject);
        }
        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplates, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeNameText(recipeSO);
        }
    }
}
