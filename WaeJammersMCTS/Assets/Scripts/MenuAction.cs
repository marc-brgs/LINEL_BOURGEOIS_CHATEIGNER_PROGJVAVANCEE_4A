using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAction : MonoBehaviour
{
    public AudioClip Menuclick;
    public AudioSource audioSource;

    void Start()
    {
        audioSource.clip = Menuclick ;
    }

    public void Jouer()
    {
        playSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quitter()
    {
        playSound();
        Application.Quit();
    }

    public void setGameMode(string gameMode)
    {
        playSound();
        GameConfig.instance.gameMode = gameMode;
    }

    public void playSound()
    {
        audioSource.Play();
    }
}
