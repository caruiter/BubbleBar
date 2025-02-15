using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    private static AudioManager instance;
    public AudioSource audioSource;

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }
    
    void Start()
    {
        //takes in audiosource from object
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip, float volume = 1f)
    {
        //Plays specified clip without stopping it. PlaySound function will be called by other scripts that sub in the parameters
        instance.audioSource.PlayOneShot(clip, volume);
    }
}
