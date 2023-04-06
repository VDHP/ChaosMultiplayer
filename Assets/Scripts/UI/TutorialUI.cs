using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moveUpText;
    [SerializeField] TextMeshProUGUI moveDownText;
    [SerializeField] TextMeshProUGUI moveLeftText;
    [SerializeField] TextMeshProUGUI moveRightText;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI interactAltText;
    [SerializeField] TextMeshProUGUI pauseText;

    private void Start()
    {
        UpdateVisual();
        PlayerInput.Instance.OnRebinding += Instance_OnRebinding;
        GameManager.Instance.OnLocalPlayerReadyChanged += Instance_OnLocalPlayerReadyChanged;
    }

    private void Instance_OnLocalPlayerReadyChanged()
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Hide();
        }
    }

    private void Instance_OnRebinding()
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        moveUpText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Up);
        moveDownText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Down);
        moveLeftText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Left);
        moveRightText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Right);
        interactText.text = PlayerInput .Instance.GetBindingText(PlayerInput.Binding.Interact);
        interactAltText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.InteractAlt);
        pauseText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Pause);
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
