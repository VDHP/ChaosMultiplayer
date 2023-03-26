using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] Button soundEffectButton;
    [SerializeField] Button musicButton;
    [SerializeField] Button closeButton;
    [SerializeField] Button moveUpButton;
    [SerializeField] Button moveDownButton;
    [SerializeField] Button moveLeftButton;
    [SerializeField] Button moveRigheButton;
    [SerializeField] Button interactButton;
    [SerializeField] Button interactAltButton;
    [SerializeField] Button pauseButton;
    [SerializeField] TextMeshProUGUI soundEffectText;
    [SerializeField] TextMeshProUGUI musicText;
    [SerializeField] TextMeshProUGUI moveUpText;
    [SerializeField] TextMeshProUGUI moveDownText;
    [SerializeField] TextMeshProUGUI moveLeftText;
    [SerializeField] TextMeshProUGUI moveRightText;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI interactAltText;
    [SerializeField] TextMeshProUGUI pauseText;
    [SerializeField] Transform pressToRebindKeyTransform;

    public static OptionUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        soundEffectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => { 
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            GamePauseUI.Instance.Show();    
        });
        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(PlayerInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(PlayerInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(PlayerInput.Binding.Move_Left);
        });
        moveRigheButton.onClick.AddListener(() =>
        {
            RebindBinding(PlayerInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(PlayerInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() =>
        {
            RebindBinding(PlayerInput.Binding.InteractAlt);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(PlayerInput.Binding.Pause);
        });
    }
    void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
    private void Instance_OnUnPausedGame()
    {
        Hide();
    }

    private void Start()
    {
        Hide();
        UpdateVisual();
        GameManager.Instance.OnUnPausedGame += Instance_OnUnPausedGame;
        HidePressToRebindKey();
    }
    void UpdateVisual()
    {
        soundEffectText.text = "Sound Effect: " + Mathf.Round( SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Up);
        moveDownText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Down);
        moveLeftText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Left);
        moveRightText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Right);
        interactText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Interact);
        interactAltText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.InteractAlt);
        pauseText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Pause);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        soundEffectButton.Select();
    }
    void RebindBinding (PlayerInput.Binding binding)
    {
        ShowPressToRebindKey();
        PlayerInput.Instance.Rebinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
