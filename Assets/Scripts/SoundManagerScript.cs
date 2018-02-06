using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    private static AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/wood_hit");
    }

    public static void PlayAudio()
    {
        print("Playing hit!");
        audio.Play();
    }

    public static void StopAudio()
    {
        audio.Stop();
    }

}
