using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySucced;
    public AudioClip[] footStep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickUp;
    public AudioClip[] trash;
    public AudioClip[] warning;
    public AudioClip stoveSizzle;
}
