using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipeDeliveryText;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
        Hide();
    }

    private void Instance_OnStateChanged()
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        recipeDeliveryText.text = DeliveryManager.Instance.GetSuccessRecipeDelivery().ToString();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
