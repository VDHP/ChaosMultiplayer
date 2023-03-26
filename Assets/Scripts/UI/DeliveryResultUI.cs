using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Image iconResult;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Color successColor;
    [SerializeField] Color failureColor;
    [SerializeField] Sprite successSprite;
    [SerializeField] Sprite failureSprite;
    [SerializeField] Animator animator;

    string popUpParamaterName;

    private void Start()
    {
        DeliveryManager.Instance.OnDeliveryFailded += Instance_OnDeliveryFailded;
        DeliveryManager.Instance.OnDeliverySucced += Instance_OnDeliverySucced;

        popUpParamaterName = animator.GetParameter(0).name;

        gameObject.SetActive(false);
    }

    private void Instance_OnDeliverySucced()
    {
        gameObject.SetActive(true);
        animator.SetTrigger(popUpParamaterName);
        background.color = successColor;
        iconResult.sprite = successSprite;
        messageText.text = "Delivery\nSuccess";
    }

    private void Instance_OnDeliveryFailded()
    {
        gameObject.SetActive(true);
        animator.SetTrigger(popUpParamaterName);
        background.color = failureColor;
        iconResult.sprite = failureSprite;
        messageText.text = "Delivery\nFailed";
    }
}
