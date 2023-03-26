using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;

    AudioSource audioSource;
    bool playWarningBurnSound;
    bool isFired;
    bool isFirstCalled;

    [SerializeField] float warningPlaySoundInterval;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnHasProgressTimeChanged += StoveCounter_OnHasProgressTimeChanged;

        isFirstCalled = true;
        audioSource.volume = .5f;
    }

    private void StoveCounter_OnHasProgressTimeChanged(float obj)
    {
        float timeProgressNormalized = obj;
        float burnShowProgressAmount = .5f;
        isFired = stoveCounter.IsFired();
        playWarningBurnSound = isFired && burnShowProgressAmount <= timeProgressNormalized;
        if (playWarningBurnSound && isFirstCalled)
        {
            StartCoroutine(IntervalWarningSound());
            isFirstCalled = false;
        }
    }

    private void StoveCounter_OnStateChanged(StoveCounter.State obj)
    {
        bool playSound = obj == StoveCounter.State.Fried || obj == StoveCounter.State.Frying;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
    IEnumerator IntervalWarningSound()
    {
        while (isFired)
        {
            SoundManager.Instance.PlayWarningBurnSound(stoveCounter.transform.position);
            Debug.Log(isFired);
            yield return new WaitForSeconds(warningPlaySoundInterval);
        }
    }
}
