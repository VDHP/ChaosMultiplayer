using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarAnimationUI : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;

    [SerializeField] Animator animator;

    string isFlashingParamaterName;

    private void Start()
    {
        stoveCounter.OnHasProgressTimeChanged += StoveCounter_OnHasProgressTimeChanged;

        isFlashingParamaterName = animator.GetParameter(0).name;
    }

    private void StoveCounter_OnHasProgressTimeChanged(float obj)
    {
        float timeProgressNormalized = obj;
        float burnShowProgressAmount = .5f;
        bool isShow = stoveCounter.IsFired() && burnShowProgressAmount <= timeProgressNormalized;

        if (isShow)
        {
            animator.SetBool(isFlashingParamaterName, true);
        }
        else
        {
            animator.SetBool(isFlashingParamaterName, false);
        }
    }
}
