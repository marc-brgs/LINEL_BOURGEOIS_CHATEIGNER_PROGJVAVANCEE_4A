using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;


    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void setEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", volume);
    }
}
