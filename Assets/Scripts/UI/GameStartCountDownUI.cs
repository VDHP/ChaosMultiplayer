using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gamePlayingCountDownText;
    [SerializeField] Animator animator;

    int previousTimer;
    string numberPopUp;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
        numberPopUp = animator.GetParameter(0).name;
        Hide();
    }
    private void Instance_OnStateChanged()
    {
        if (GameManager.Instance.IsStartCountDown())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        int countDownTimer = GameManager.Instance.GetStartCountDownTimer();
        gamePlayingCountDownText.text = countDownTimer.ToString();
        if(countDownTimer != previousTimer)
        {
            previousTimer = countDownTimer;
            animator.SetBool(numberPopUp,true);
            SoundManager.Instance.PlayCountDownSound();
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
