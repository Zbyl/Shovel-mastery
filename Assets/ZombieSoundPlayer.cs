using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundPlayer : MonoBehaviour
{
    private AudioSource[] audioSources;

    // Start is called before the first frame update
    void Awake()
    {
        audioSources = gameObject.GetComponentsInChildren<AudioSource>();
        selectedSound = Random.Range(0, audioSources.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    int selectedSound;

    public void Play()
    {
        AudioSource audioSource = audioSources[selectedSound];
        audioSource.Play();
    }

    public bool isPlaying()
    {
        AudioSource audioSource = audioSources[selectedSound];
        return audioSource.isPlaying;
    }
}
