using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public GameObject gameObject;
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectsList;
    [SerializeField] PlatesKitchenObject platesKitchenObject;
    // Start is called before the first frame update
    void Start()
    {
        platesKitchenObject.OnIngredientAdded += PlatesKitchenObject_OnIngredientAdded1;
    }   
    private void PlatesKitchenObject_OnIngredientAdded1(KitchenObjectSO kitchenObjectSO)
    {
        foreach(KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectsList)
        {
            if(kitchenObjectSO_GameObject.kitchenObjectSO == kitchenObjectSO)
            {
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
    }
}
