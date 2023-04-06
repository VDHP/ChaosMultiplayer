using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingPlayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += Instance_OnLocalPlayerReadyChanged;
        GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
        Hide();
    }

    private void Instance_OnStateChanged()
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Hide();
        }
    }

    private void Instance_OnLocalPlayerReadyChanged()
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Show();
        }
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
