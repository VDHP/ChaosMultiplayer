using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnMultiplayerGamePaused += Instance_OnMultiplayerGamePaused;
        GameManager.Instance.OnMultiplayerGameUnPaused += Instance_OnMultiplayerGameUnPaused;
        Hide();
    }

    private void Instance_OnMultiplayerGameUnPaused()
    {
        Hide();
    }

    private void Instance_OnMultiplayerGamePaused()
    {
        Show();
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
