using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    float volume;

    const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

        volume =  PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        musicSource.volume = volume;
    }   
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME,volume);
        musicSource.volume = volume;
    }
    public float GetVolume()
    {
        return volume;
    }
}
