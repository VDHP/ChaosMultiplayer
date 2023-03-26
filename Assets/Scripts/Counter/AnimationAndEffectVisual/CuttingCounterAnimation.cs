using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CuttingCounter cuttingCounter;
    private void Start()
    {
        cuttingCounter.OnHasProgressTimeChanged += CuttingCounter_OnCuttingProgressTimeChanged;
    }

    private void CuttingCounter_OnCuttingProgressTimeChanged(float cuttingProgressTimeNormalized)
    {
        if(cuttingProgressTimeNormalized > 0)
        {
            CuttingCounter_OnAnimateAction();
        }
    }

    private void CuttingCounter_OnAnimateAction()
    {
        animator.SetTrigger(animator.GetParameter(0).name);
    }
}
