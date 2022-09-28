using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool GameIsPaused = false;
    
    public GameObject pauseMenu;
    public GameObject menuFin;
    public GameObject frisbee;
    public GameObject ennemy;
    public bool isFinished = false;
    
    public GameObject borderTop;
    public GameObject borderBottom;
    public GameObject borderLeft;
    public GameObject borderRight;
    public float borderRadius;

    public Vector3 playerPosition;
    public Vector3 ennemyPosition;
    
    private string gameMode = "Random"; // Default gameMode
    private bool gameEnded = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la scene");
            return;
        }
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        borderRadius = borderTop.transform.localScale.z;
        
        if(GameConfig.instance != null) // Recover gameMode selected from menu
            gameMode = GameConfig.instance.gameMode;
        
        if (gameMode == "Solo")
        {
            ennemy.GetComponent<RandomAgent>().enabled = false; // remove random agent
        }
        if (gameMode == "Duo")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // remove controls
            ennemy.GetComponent<RandomAgent>().enabled = false; // remove random agent
        }

        if (gameMode == "Random")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // remove controls
            
        }
        if (gameMode == "MCTS")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // remove controls
            ennemy.GetComponent<RandomAgent>().enabled = false; // remove random agent
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameEnded)
            CheckForPause();
    }

    public void CheckForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ReplayGame()
    {
        menuFin.SetActive(false);
        Scores.instance.PlayerScore = 0;
        Scores.instance.EnnemyScore = 0;
        Time.timeScale = 1f;
        gameEnded = false;
    }
    
    public void EndGame()
    {
        gameEnded = true;
        menuFin.SetActive(true);
        Time.timeScale = 0f;
        GameObject.Find("UI/Menu Fin/Texte").GetComponent<TMPro.TextMeshProUGUI>().text = Scores.instance.EnnemyScore.ToString() + '-' + Scores.instance.PlayerScore.ToString();
    }
}
