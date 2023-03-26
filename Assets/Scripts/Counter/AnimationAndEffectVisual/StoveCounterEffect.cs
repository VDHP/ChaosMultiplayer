using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounterEffect : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    [SerializeField] GameObject sizzlingParticale;
    [SerializeField] GameObject stoveVisualEffect;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }
    void OnVisual()
    {
        sizzlingParticale.SetActive(true); 
        stoveVisualEffect.SetActive(true);
    }
    void OffVisual()
    {
        sizzlingParticale.SetActive(false);
        stoveVisualEffect.SetActive(false);
    }

    private void StoveCounter_OnStateChanged(StoveCounter.State state)
    {
        bool onStove = state == StoveCounter.State.Frying || state == StoveCounter.State.Fried;
        if (onStove)
        {
            OnVisual();            
        }
        else
        {
            OffVisual();
        }
    }
}
