using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] PlatesKitchenObject platesKitchenObject;
    [SerializeField] Transform iconTemplate;
    private void Start()
    {
        platesKitchenObject.OnIngredientAdded += PlatesKitchenObject_OnIngredientAdded;
    }
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void PlatesKitchenObject_OnIngredientAdded(KitchenObjectSO obj)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // destroy all icon before
        foreach(Transform child in transform)
        {
            if (child == iconTemplate) 
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        // show all icon of kichenObjectSO is on plate
        foreach (KitchenObjectSO kitchenObjectSO in platesKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateSingleIconUI>().SetKithenObjectSO(kitchenObjectSO);
        }
    }
}
