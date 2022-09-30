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
    public GameObject filet;
    public float filetRadius;
    
    [SerializeField] private  GameObject goalP;
    [SerializeField] private GameObject goalE;
    private float goalRadius;

    public GameState State;
    public bool isScored = false;
    public bool isFinished = false;

    private string gameMode = "MCTS"; // Default gameMode

    // string[] possibleActions = { "UP", "DOWN", "LEFT", "RIGHT", "SHOOT" };

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
        filetRadius = filet.transform.localScale.x / 2;

        if(GameConfig.instance != null) // Recover gameMode selected from menu
            gameMode = GameConfig.instance.gameMode;
        
        if (gameMode == "Solo")
        {
            ennemy.GetComponent<RandomAgent>().enabled = false; // Remove random agent
            ennemy.GetComponent<MCTSAgent>().enabled = false; // Remove MCTS agent
        }
        if (gameMode == "Duo")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // Remove controls
            ennemy.GetComponent<RandomAgent>().enabled = false; // Remove random agent
            ennemy.GetComponent<MCTSAgent>().enabled = false; // Remove MCTS agent
        }

        if (gameMode == "Random")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // Remove controls
            ennemy.GetComponent<MCTSAgent>().enabled = false; // Remove MCTS agent
        }
        if (gameMode == "MCTS")
        {
            ennemy.GetComponent<PlayerController>().enabled = false; // Remove controls
            ennemy.GetComponent<RandomAgent>().enabled = false; // Remove random agent
        }

        // Instantiate GameState
        State = new GameState(player.transform.position, new Vector2(0f, 0f), ennemy.transform.position,
            new Vector2(0f, 0f), frisbee.transform.position,
            FrisbeeController.instance.frisbeeDirection, 0, 0, false, "Ennemy", false, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMode == "MCTS")
        {
            State = ExecuteActionForEnnemy(this.State, "UP");
            State = ExecuteActionForEnnemy(this.State, "LEFT");
        }

        checkGoals(State);
        RenderGameState(State);
        
        if(!isFinished)
            CheckForPause();
    }

    public void CheckForPause()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
       
        if (GameIsPaused) ResumeGame();
        else PauseGame();
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
        isFinished = false;
    }
    
    public void EndGame()
    {
        menuFin.SetActive(true);
        Time.timeScale = 0f;
        GameObject.Find("UI/Menu Fin/Texte").GetComponent<TMPro.TextMeshProUGUI>().text = Scores.instance.EnnemyScore.ToString() + '-' + Scores.instance.PlayerScore.ToString();
    }

    /*
     *  Render game with given GameState
     */
    public void RenderGameState(GameState State)
    {
        player.transform.position = State.playerPosition;
        ennemy.transform.position = State.ennemyPosition;
        frisbee.transform.position = State.frisbeePosition;
        Scores.instance.PlayerScore = State.playerScore;
        Scores.instance.EnnemyScore = State.ennemyScore;
        FrisbeeController.instance.isHeld = State.isHeld;
        FrisbeeController.instance.lastHolder = State.lastHolder;
        isScored = State.isScored;
        isFinished = State.isFinished;
    }
    
    public GameState GetCurrentGameState()
    {
        return State;
    }

    
    /*
     *  Check frisbee position 
     */
    public void checkGoals(GameState state)
    {
        if (state.frisbeePosition.x < goalE.transform.position.x + goalRadius) // Frisbee half enter ennemy goal - Player scored
        {
            state.isScored = true;
            state.playerScore += 1;
            nextRound(state,"ENNEMY");

            if (state.playerScore == 10)
            {
                state.isFinished = true;
                EndGame();
            }
        }

        if (state.frisbeePosition.x > goalP.transform.position.x - goalRadius) // Frisbee half enter player goal - Ennemy scored
        {
            isScored = true;
            state.ennemyScore += 1;
            nextRound(state, "PLAYER");

            if (Scores.instance.EnnemyScore == 10)
            {
                state.isFinished = true;
                EndGame();
            }
        }
    }
    
    public void nextRound(GameState state, string entity)
    {
        if (entity == "ENNEMY") // Set frisbee in ennemy zone
        {
            state.lastHolder = "Player";
            state.frisbeePosition = new Vector3(-6f, 2.25f, 0f);
        }
        else // Set frisbee in player zone
        {
            state.lastHolder = "Ennemy";
            state.frisbeePosition = new Vector3(6f, 2.25f, 0f);
        }
        FrisbeeController.instance.isMoving = false;
        state.isHeld = false;
        state.isScored = false;
    }
    
    public GameState ExecuteActionForPlayer(GameState state, string action)
    {
        Vector2 simulatedInput = new Vector2(0f, 0f);
        
        switch (action)
        {
            case "UP":
                simulatedInput = new Vector2(0f, 1f);
                break;
            case "DOWN":
                simulatedInput = new Vector2(0f, -1f);
                break;
            case "LEFT":
                simulatedInput = new Vector2(-1f, 0f);
                break;
            case "RIGHT":
                simulatedInput = new Vector2(1f, 0f);
                break;
        }

        if (action == "UP" || action == "DOWN" || action == "LEFT" || action == "RIGHT")
        {
            Vector2 playerPosition2D = new Vector2(state.playerPosition.x, state.playerPosition.z);
            simulatedInput = EntityCollisionHandler(playerPosition2D, simulatedInput, "RIGHT");
            state.playerPosition = new Vector3(state.playerPosition.x + simulatedInput.x/1.5f, state.playerPosition.y, state.playerPosition.z + simulatedInput.y/1.5f);
        }

        return state;
    }
    
    public GameState ExecuteActionForEnnemy(GameState state, string action)
    {
        Vector2 simulatedInput = new Vector2(0f, 0f);
        
        switch (action)
        {
            case "UP":
                simulatedInput = new Vector2(0f, 1f);
                break;
            case "DOWN":
                simulatedInput = new Vector2(0f, -1f);
                break;
            case "LEFT":
                simulatedInput = new Vector2(-1f, 0f);
                break;
            case "RIGHT":
                simulatedInput = new Vector2(1f, 0f);
                break;
        }

        if (action == "UP" || action == "DOWN" || action == "LEFT" || action == "RIGHT")
        {
            Vector2 ennemyPosition2D = new Vector2(state.ennemyPosition.x, state.ennemyPosition.z);
            simulatedInput = EntityCollisionHandler(ennemyPosition2D, simulatedInput, "LEFT");
            state.ennemyPosition = new Vector3(state.ennemyPosition.x + simulatedInput.x/1.5f, state.ennemyPosition.y, state.ennemyPosition.z + simulatedInput.y/1.5f);
        }

        return state;
    }

    /*
     *  Return inputX and inputY handling border collision (set 0 on axis if border)
     */
    public Vector2 EntityCollisionHandler(Vector2 entityPosition, Vector2 input, string side)
    {
        float entityRadius = player.transform.localScale.x / 1.2f; // approx
        
        if (side == "LEFT" && entityPosition.x + entityRadius > 0 + filetRadius) // Left side middle border
        {
            if (input.x > 0)
                input = new Vector2(0f, input.y);
        }
        if (side == "RIGHT" && entityPosition.x - entityRadius < 0 - filetRadius) // Right side middle border
        {
            if (input.x < 0)
                input = new Vector2(0f, input.y);
        }
        if (entityPosition.y + entityRadius > borderTop.transform.position.z - borderRadius) // Border top
        {
            if (input.y > 0)
                input = new Vector2(input.x, 0f);
        }
        if (entityPosition.y - entityRadius < borderBottom.transform.position.z + borderRadius) // Border bottom
        {
            if (input.y < 0)
                input = new Vector2(input.x, 0f);
        }
        if (entityPosition.x - entityRadius < borderLeft.transform.position.x + borderRadius) // Border left
        {
            if (input.x < 0)
                input = new Vector2(0f, input.y);
        }
        if (entityPosition.x + entityRadius > borderRight.transform.position.x - borderRadius) // Border right
        {
            if (input.x > 0)
                input = new Vector2(0f, input.y);
        }

        return input;
    }
}
