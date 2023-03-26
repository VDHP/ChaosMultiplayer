using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Transform iconContainer;
    [SerializeField] Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeNameText(RecipeSO recipeSO)
    {
        textMeshProUGUI.text = recipeSO.nameRecipe;

        foreach(Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer); 
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
