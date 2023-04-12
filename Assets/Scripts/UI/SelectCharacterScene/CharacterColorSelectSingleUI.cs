using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] GameObject setlectedObject;
    [SerializeField] int  colorID;

    private void Awake()
    {

        GetComponent<Button>().onClick.AddListener(() =>
        {
            KitchenObjectMultiplayer.Instance.ChangePlayerColor(colorID);
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        KitchenObjectMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged;
        image.color = KitchenObjectMultiplayer.Instance.GetPlayerColor(colorID);
        UpdateIsSelected();
    }

    private void KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged()
    {
        UpdateIsSelected();
    }

    void UpdateIsSelected()
    {
        if(KitchenObjectMultiplayer.Instance.GetPlayerData().colorID == colorID)
        {
            setlectedObject.SetActive(true);
        }
        else
        {
            setlectedObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        KitchenObjectMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenObjectMultiplayer_OnPlayerDataNetworkListChanged;
    }
}
