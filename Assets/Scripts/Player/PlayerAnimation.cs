    using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController controller;

    string isWalkingParameter;
    private void Update()
    {
        if (IsOwner) return;
        animator.SetBool(isWalkingParameter, controller.IsWalking());
    }
    private void Awake()
    {
        isWalkingParameter = animator.GetParameter(0).name;
    }
}
