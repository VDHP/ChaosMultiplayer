using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateSingleIconUI : MonoBehaviour
{
    [SerializeField] Image image;
    public void SetKithenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
