using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] GameObject[] visualGameObjectArray;
    [SerializeField] BaseCounter baseCounter;


    private void Start()
    {
        if(PlayerController.LocalInstance != null)
        {
            PlayerController.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        }
        else
        {
            PlayerController.OnAnyPlayerSpawned += PlayerController_OnAnyPlayerSpawned;
        }
    }

    private void PlayerController_OnAnyPlayerSpawned()
    {
        if (PlayerController.LocalInstance != null)
        {
            PlayerController.LocalInstance.OnSelectedCounterChanged -= Instance_OnSelectedCounterChanged;
            PlayerController.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        }
    }

    private void Instance_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    void Show()
    {
        foreach(GameObject selectedPart in visualGameObjectArray)
        {
            selectedPart.SetActive(true);
        }
    }
    void Hide()
    {
        foreach (GameObject selectedPart in visualGameObjectArray)
        {
            selectedPart.SetActive(false);
        }
    }
}
