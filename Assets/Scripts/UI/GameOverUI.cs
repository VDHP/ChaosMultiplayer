using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipeDeliveryText;
    [SerializeField] Button PlayAgainButton;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
        Hide();
    }
    private void Awake()
    {
        PlayAgainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
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
