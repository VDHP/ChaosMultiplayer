using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingSelectCharacterUI : MonoBehaviour
{
    [SerializeField] Button readyButton;

    private void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            CharacterSelectedReady.Instance.SetPlayerReady();
        });
    }
}
