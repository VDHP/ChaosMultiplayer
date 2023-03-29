using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClipRefSO audioClipRefSO;

    const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectVolume";
    public static SoundManager Instance { get; private set; }

    float volume;
    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, .5f);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnDeliverySucced += DeliveryManager_OnDeliverySucced;
        DeliveryManager.Instance.OnDeliveryFailded += DeliveryManager_OnDeliveryFailded;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        PlayerController.OnAnyPlayerPickedUpSomeThing += PlayerController_OnAnyPlayerPickedUpSomeThing;
        BaseCounter.OnDropSomethingHere += BaseCounter_OnDropSomethingHere;
        TrashCounter.OnThrownSomethingInHere += TrashCounter_OnThrownSomethingInHere;
        PlatesKitchenObject.OnIngredientAddedSound += PlatesKitchenObject_OnIngredientAddedSound;
    }

    private void PlayerController_OnAnyPlayerPickedUpSomeThing(PlayerController obj)
    {
        PlayerController player = obj as PlayerController;
        PlaySound(audioClipRefSO.objectPickUp, player.transform.position);
    }

    private void PlatesKitchenObject_OnIngredientAddedSound(PlatesKitchenObject obj)
    {
        PlatesKitchenObject platesKitchenObject = (PlatesKitchenObject)obj;
        PlaySound(audioClipRefSO.objectPickUp, platesKitchenObject.transform.position);
    }

    private void TrashCounter_OnThrownSomethingInHere(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipRefSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnDropSomethingHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipRefSO.objectDrop, baseCounter.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(audioClipRefSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnDeliveryFailded()
    {
        PlaySound(audioClipRefSO.deliveryFail, Camera.main.transform.position);
    }

    private void DeliveryManager_OnDeliverySucced()
    {
        PlaySound(audioClipRefSO.deliverySucced, Camera.main.transform.position);
    }
    void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiple = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiple * volume);
    }
    public void PlayCountDownSound()
    {
        PlaySound(audioClipRefSO.warning, Camera.main.transform.position );
    }
    public void PlayWarningBurnSound(Vector3 position )
    {
        PlaySound(audioClipRefSO.warning, position);
    }

    void PlaySound(AudioClip[] audioClipsArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsArray[Random.Range(0, audioClipsArray.Length)], position, volume);
    }
    public void PlayeFootStepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefSO.footStep, position, volume);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
    }
    public float GetVolume()
    {
        return volume;
    }
}
