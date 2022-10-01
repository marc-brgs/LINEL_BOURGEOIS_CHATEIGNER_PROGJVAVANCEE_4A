using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip Music;
    public AudioSource audioSource;

    void Start()
    {
        audioSource.clip = Music;
        audioSource.Play();
    }

    
    void Update()
    {
        
    }
}
