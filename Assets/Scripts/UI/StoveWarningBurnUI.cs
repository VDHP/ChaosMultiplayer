using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveWarningBurnUI : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnHasProgressTimeChanged += StoveCounter_OnHasProgressTimeChanged;
        Hide();
    }

    private void StoveCounter_OnHasProgressTimeChanged(float obj)
    {
        float timeProgressNormalized = obj;
        float burnShowProgressAmount = .5f;
        bool isShow = stoveCounter.IsFired() && burnShowProgressAmount <= timeProgressNormalized;

        if (isShow)
        {
            Show();
        }
        else
        {
            Hide();
        }
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
