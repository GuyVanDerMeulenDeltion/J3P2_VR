using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public List<AudioSource> audioSourcesSpecific = new List<AudioSource>();
    public List<AudioSource> audioSourcesRandom = new List<AudioSource>();

    public void PlaySpecificAudio(int whichOne)
    {
        audioSourcesSpecific[whichOne].Play();
    }

    public void PlayRandomAudio()
    {
        audioSourcesRandom[Random.Range(0, audioSourcesRandom.Count)].Play();
    }
}
