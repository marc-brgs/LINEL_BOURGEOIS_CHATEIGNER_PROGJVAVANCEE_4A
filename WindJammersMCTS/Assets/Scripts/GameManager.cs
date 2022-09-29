using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool GameIsPaused = false;
    public bool isFinished = false;
    public bool endPoint = false;
    
    public GameObject pauseMenu;
    public GameObject menuFin;
    public GameObject frisbee;
    public GameObject player;
    public GameObject ennemy;

    public GameObject borderTop;
    public GameObject borderBottom;
    public GameObject borderLeft;
    public GameObject borderRight;
    public float borderRadius;
    
    public GameObject goalP;
    public GameObject goalE;
    private float goalRadius;

    public GameState State;
    public bool isScored = true;

    // GameState
    public Vector3 playerPosition;
    public Vector3 ennemyPosition;
    
    private string gameMode = "MCTS"; // Default gameMode
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
        borderRadius = borderTop.transform.localScale.z / 2;
        goalRadius = goalP.transform.localScale.x / 2;

        if(GameConfig.instance != null) // Recover gameMode selected from menu
            gameMode = GameConfig.instance.gameMode;
        
        if (gameMode == "Solo")
        {
            ennemy.GetComponent<RandomAgent>().enabled = false; // remove random agent
            ennemy.GetComponent<MCTSAgent>().enabled = false; // remove MCTS agent
        }
        if (gameMode == "Duo")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // remove controls
            ennemy.GetComponent<RandomAgent>().enabled = false; // remove random agent
            ennemy.GetComponent<MCTSAgent>().enabled = false; // remove MCTS agent
        }

        if (gameMode == "Random")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // remove controls
            ennemy.GetComponent<MCTSAgent>().enabled = false; // remove MCTS agent
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
        checkGoals();
        
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

    public void UpdateGameState()
    {
        State.playerPosition = player.transform.position;
        State.ennemyPosition = ennemy.transform.position;
        State.frisbeePosition = frisbee.transform.position;
        State.playerScore = Scores.instance.PlayerScore;
        State.ennemyScore = Scores.instance.EnnemyScore;
        State.isHeld = FrisbeeController.instance.isHeld;
        State.lastHolder = FrisbeeController.instance.lastHolder;
        State.isScored = isScored;
        State.isFinished = gameEnded;
        
    }

    public GameState GetCurrentGameState()
    {
        return State;
    }
    
    public void ApplyGameState(GameState State)
    {
        player.transform.position = State.ennemyPosition;
        ennemy.transform.position = State.ennemyPosition;
        frisbee.transform.position = State.frisbeePosition;
    }

    public void checkGoals()
    {
        if (frisbee.transform.position.x < goalE.transform.position.x + goalRadius) // frisbee half enter ennemy goal - Player scored
        {
            isScored = true;
            Scores.instance.PlayerScore += 1;
            GameManager.instance.nextMatchPoint("Ennemy");
            
            if (Scores.instance.PlayerScore == 10)
                GameManager.instance.EndGame();
        }

        if (frisbee.transform.position.x > goalP.transform.position.x - goalRadius) // frisbee half enter player goal - Ennemy scored
        {
            isScored = true;
            Scores.instance.EnnemyScore += 1;
            GameManager.instance.nextMatchPoint("Player");

            if (Scores.instance.EnnemyScore == 10)
                GameManager.instance.EndGame();
        }
    }
    
    public void nextMatchPoint(string entity)
    {
        if (entity == "Ennemy")
        {
            FrisbeeController.instance.lastHolder = "Player";
            frisbee.transform.position = new Vector3(-6f, 2.25f, 0f); // Set frisbee in ennemy zone
        }
        else
        {
            FrisbeeController.instance.lastHolder = "Ennemy";
            frisbee.transform.position = new Vector3(6f, 2.25f, 0f); // Set frisbee in player zone
        }
        FrisbeeController.instance.isMoving = false;
        FrisbeeController.instance.isHeld = false;
        isScored = false;
    }
    
    public void executeAction(string action)
    {
        switch (action)
        {
            case "UP":
                break;
        }
    }
}
