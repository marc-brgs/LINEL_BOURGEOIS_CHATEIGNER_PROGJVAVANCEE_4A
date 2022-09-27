using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAction : MonoBehaviour
{
    public void Jouer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quitter()
    {
        Application.Quit();
    }

    public void setGameMode(string gameMode)
    {
        GameConfig.instance.gameMode = gameMode;
    }
}
