using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] GameObject readyObject;

    private void Start()
    {
        KitchenObjectMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectedReady.Instance.OnReadyChanged += CharacterSelectedReady_OnReadyChanged;

        UpdatePlayer();
    }

    private void CharacterSelectedReady_OnReadyChanged()
    {
        UpdatePlayer();
    }

    private void KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged()
    {
        UpdatePlayer();
    }
    void UpdatePlayer()
    {
        if(KitchenObjectMultiplayer.Instance.IsPlayerConnectedWithIndex(playerIndex))
        {
            Show();

            PlayerData playerData = KitchenObjectMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);
            readyObject.SetActive(CharacterSelectedReady.Instance.IsPlayerReady(playerData.clientID));
        }
        else
        {
            Hide();
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
}
