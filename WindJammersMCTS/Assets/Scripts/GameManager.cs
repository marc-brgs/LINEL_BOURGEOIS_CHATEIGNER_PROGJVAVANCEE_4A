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
    public float entityRadius;
    
    [SerializeField] private  GameObject goalP;
    [SerializeField] private GameObject goalE;
    private float goalRadius;

    public GameState State;

    private string gameMode = "Random"; // Default game mode
    private int maxScore = 7;
    
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la scene");
            return;
        }
        instance = this;
        
        // Scales
        borderRadius = borderTop.transform.localScale.z / 2;
        goalRadius = goalP.transform.localScale.x / 2;
        filetRadius = filet.transform.localScale.x / 2;
        entityRadius = 2f;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Instantiate GameState
        State = new GameState(player.transform.position, ennemy.transform.position, frisbee.transform.position,
            FrisbeeController.instance.frisbeeDirection, 0, 0, false, false, "Ennemy", false, false);

        if (GameConfig.instance != null) // Recover game mode from menu
            gameMode = GameConfig.instance.gameMode;

            if (gameMode == "Random")
        {
            ennemy.GetComponent<MCTSAgent>().enabled = false; // Remove MCTS agent
        }
        if (gameMode == "MCTS")
        {
            ennemy.GetComponent<RandomAgent>().enabled = false; // Remove random agent

            ennemy.GetComponent<MCTSAgent>().GMInstance = instance;
            // ennemy.GetComponent<MCTSAgent>().ComputeMCTS(); // Debug MCTS (big freeze)
        }
    }

    // Physics (collisions, movements)
    void FixedUpdate()
    {
        if (gameMode == "MCTS")
        {
            // ennemy.GetComponent<MCTSAgent>().ComputeMCTS(); // MCTS Big freeze
        }

        FrisbeeController.instance.checkCatch(this.State);
        FrisbeeController.instance.checkBorderCollisions(this.State);
        FrisbeeController.instance.moveOrStick(this.State);
        
        bool endRound = checkGoals(this.State);

        if (endRound)
            setupNextRound(this.State); // Unnecessary to simulate (simulation end when scoring a point)
    }
    
    // Update is called once per frame (inputs)
    void Update()
    {
        RenderGameState(this.State);
        
        if(!State.isFinished)
            CheckForPause();
    }

    private void CheckForPause()
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
        State.playerScore = 0;
        State.ennemyScore = 0;
        Time.timeScale = 1f;
        State.isFinished = false;
    }
    
    public void EndGame()
    {
        RenderGameState(State);
        menuFin.SetActive(true);
        Time.timeScale = 0f;
        GameObject.Find("UI/Menu Fin/Score").GetComponent<TMPro.TextMeshProUGUI>().text = Scores.instance.EnnemyScore.ToString() + '-' + Scores.instance.PlayerScore.ToString();
    }

    /*
     *  Render game with given GameState
     */
    public void RenderGameState(GameState state)
    {
        player.transform.position = state.playerPosition;
        ennemy.transform.position = state.ennemyPosition;
        frisbee.transform.position = state.frisbeePosition;
        Scores.instance.PlayerScore = state.playerScore;
        Scores.instance.EnnemyScore = state.ennemyScore;
        FrisbeeController.instance.isHeld = state.isHeld;
        FrisbeeController.instance.lastHolder = state.lastHolder;
    }
    
    public GameState GetCurrentGameState()
    {
        return State;
    }

    /*
     *  Check if scored with frisbee position 
     */
    public bool checkGoals(GameState state)
    {
        bool scored = false;
        
        if (state.frisbeePosition.x < goalE.transform.position.x + goalRadius) // Frisbee half enter ennemy goal - Player scored
        {
            state.isScored = true;
            state.playerScore += 1;

            if (state.playerScore == maxScore)
            {
                state.isFinished = true;
                if(state == State) // Real game end
                    EndGame();
            }

            scored = true;
        }
        else if (state.frisbeePosition.x > goalP.transform.position.x - goalRadius) // Frisbee half enter player goal - Ennemy scored
        {
            state.isScored = true;
            state.ennemyScore += 1;

            if (state.ennemyScore == maxScore)
            {
                state.isFinished = true;
                if(state == State) // Real game end
                    EndGame();
            }
            scored = true;
        }

        return scored;
    }
    
    public void setupNextRound(GameState state)
    {
        if (state.lastHolder == "Ennemy") // Set frisbee in player zone
        {
            state.frisbeePosition = new Vector3(6f, 2.25f, 0f);
        }
        else if(state.lastHolder == "Player") // Set frisbee in ennemy zone
        {
            state.frisbeePosition = new Vector3(-6f, 2.25f, 0f);
        }
        state.isMoving = false;
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
            case "SHOOT_TOP":
                FrisbeeController.instance.Shoot(state, "TOP");
                break;
            case "SHOOT_BOTTOM":
                FrisbeeController.instance.Shoot(state, "BOTTOM");
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
            case "SHOOT_TOP":
                FrisbeeController.instance.Shoot(state, "TOP");
                break;
            case "SHOOT_BOTTOM":
                FrisbeeController.instance.Shoot(state, "BOTTOM");
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
