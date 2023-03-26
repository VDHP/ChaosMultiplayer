using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainCounterAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ContainCounter containCounter;
    private void Start()
    {
        containCounter.OnAnimateAction += ContainCounter_OnAnimateAction;
    }
    private void ContainCounter_OnAnimateAction()
    {
        animator.SetTrigger(animator.GetParameter(0).name);
    }
}
