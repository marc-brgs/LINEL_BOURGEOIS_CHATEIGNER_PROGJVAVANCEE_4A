using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public Vector3 playerPosition;
    public Vector3 ennemyPosition;
    public Vector3 frisbeePosition;
    public Vector2 frisbeeDirection;
    public int playerScore;
    public int ennemyScore;
    public bool isMoving; // Frisbee state
    public bool isHeld;
    public string lastHolder;
    public bool isScored;
    public bool isFinished;


    public string[] getPossibleAction(string entity)
    {
        Debug.Log("MCTS : getPossibleAction");
        
        if (isHeld)
        {
            if (entity == "ENNEMY" && lastHolder == "Ennemy")
                return new string[] { "UP", "DOWN", "LEFT", "RIGHT", "SHOOT_TOP", "SHOOT_BOTTOM" };
            else if (entity == "PLAYER" && lastHolder == "Player")
                return new string[] { "UP", "DOWN", "LEFT", "RIGHT", "SHOOT_TOP", "SHOOT_BOTTOM" };
        }
        
        return new string[] { "UP", "DOWN", "LEFT", "RIGHT" };
    }

    public string getRandomAction(string[] actions)
    {
        Debug.Log("MCTS : getRandomAction");
        
        int i = Random.Range(0, actions.Length - 1);
        return actions[i];
    }
    
    public void copyGameState(GameState toCopy)
    {
        this.playerPosition = toCopy.playerPosition;
        this.ennemyPosition = toCopy.ennemyPosition;
        this.frisbeePosition = toCopy.frisbeePosition;
        this.frisbeeDirection = toCopy.frisbeeDirection;
        this.playerScore = toCopy.playerScore;
        this.ennemyScore = toCopy.ennemyScore;
        this.isMoving = toCopy.isMoving;
        this.isHeld = toCopy.isHeld;
        this.lastHolder = toCopy.lastHolder;
        this.isScored = toCopy.isScored;
        this.isFinished = toCopy.isFinished;
    }
    
    // Constructors
    
    public GameState()
    {
        
    }
    
    public GameState(GameState toCopy)
    {
        this.playerPosition = toCopy.playerPosition;
        this.ennemyPosition = toCopy.ennemyPosition;
        this.frisbeePosition = toCopy.frisbeePosition;
        this.frisbeeDirection = toCopy.frisbeeDirection;
        this.playerScore = toCopy.playerScore;
        this.ennemyScore = toCopy.ennemyScore;
        this.isMoving = toCopy.isMoving;
        this.isHeld = toCopy.isHeld;
        this.lastHolder = toCopy.lastHolder;
        this.isScored = toCopy.isScored;
        this.isFinished = toCopy.isFinished;
    }
    
    public GameState(Vector3 playerPosition, Vector3 ennemyPosition, Vector3 frisbeePosition, Vector2 frisbeeDirection,
        int playerScore, int ennemyScore, bool isMoving, bool isHeld, string lastHolder, bool isScored, bool isFinished)
    {
        this.playerPosition = playerPosition;
        this.ennemyPosition = ennemyPosition;
        this.frisbeePosition = frisbeePosition;
        this.frisbeeDirection = frisbeeDirection;
        this.playerScore = playerScore;
        this.ennemyScore = ennemyScore;
        this.isMoving = isMoving;
        this.isHeld = isHeld;
        this.lastHolder = lastHolder;
        this.isScored = isScored;
        this.isFinished = isFinished;
    }
}
