using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetworkUI : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    // Start is called before the first frame update
    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            KitchenObjectMultiplayer.Instance.StartHost();
            Hide();
        });
        clientButton.onClick.AddListener(() =>
        {
            KitchenObjectMultiplayer.Instance.StartClient();
            Hide();
        });
    }

    void Hide()
    {
        gameObject.SetActive(false);    
    }
}
