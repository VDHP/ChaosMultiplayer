using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMeassageUI : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] TextMeshProUGUI messageText;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    // Start is called before the first frame update
    void Start()
    {
        KitchenObjectMultiplayer.Instance.OnFailedToJoinGame += KitchenObjectMultiplayer_OnFailedToJoinGame;

        Hide();
    }

    private void KitchenObjectMultiplayer_OnFailedToJoinGame()
    {
        Show();

        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if(messageText.text == "")
        {
            messageText.text = "Failed to connect";
        }
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        KitchenObjectMultiplayer.Instance.OnFailedToJoinGame -= KitchenObjectMultiplayer_OnFailedToJoinGame;

    }
}
