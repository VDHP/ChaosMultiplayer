using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KitchenObjectMultiplayer.Instance.OnFailedToJoinGame += KitchenObjectMultiplayer_OnFailedToJoinGame;
        KitchenObjectMultiplayer.Instance.OnTryingToJoinGame += KitchenObjectMultiplayer_OnTryingToJoinGame;
        Hide();
    }

    private void KitchenObjectMultiplayer_OnTryingToJoinGame()
    {
        Show();
    }

    private void KitchenObjectMultiplayer_OnFailedToJoinGame()
    {
        Hide();
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
    void Show()
    {
        gameObject.SetActive (true);
    }
    private void OnDestroy()
    {
        KitchenObjectMultiplayer.Instance.OnFailedToJoinGame -= KitchenObjectMultiplayer_OnFailedToJoinGame;
        KitchenObjectMultiplayer.Instance.OnTryingToJoinGame -= KitchenObjectMultiplayer_OnTryingToJoinGame;
    }
}
