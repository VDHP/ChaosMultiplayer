using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] float footTimerMax;
    float footTimer;

    private void Update()
    {
        if (playerController.IsWalking())
        {
            footTimer -= Time.deltaTime;
            if(footTimer < 0 ) 
            { 
                footTimer= footTimerMax;    
                SoundManager.Instance.PlayeFootStepsSound(playerController.transform.position, 1f);
            }
        }
    }
}
