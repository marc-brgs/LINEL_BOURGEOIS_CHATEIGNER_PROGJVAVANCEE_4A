using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Vector3 playerPosition;
    public Vector3 ennemyPosition;
    public Vector3 frisbeePosition;
    public Vector2 frisbeeDirection;
    public int playerScore;
    public int ennemyScore;
    public bool isHeld;
    public string lastHolder;
    public bool isScored;
    public bool isFinished;


    public string[] getPossibleAction(string entity)
    {
        string[] gStatePosAct = new string[] { };
        
        if (isHeld)
        {
            if (entity == "ENNEMY" && lastHolder == "Ennemy")
                gStatePosAct = new string[] { "UP", "DOWN", "LEFT", "RIGHT", "SHOOT_TOP", "SHOOT_BOTTOM" };
            else if (entity == "PLAYER" && lastHolder == "Player")
                gStatePosAct = new string[] { "UP", "DOWN", "LEFT", "RIGHT", "SHOOT_TOP", "SHOOT_BOTTOM" };
        }
        else
        {
            gStatePosAct = new string[] { "UP", "DOWN", "LEFT", "RIGHT" };
        }
        
        return gStatePosAct;
    }

    public string getRandomAction(string[] array)
    {
        int rdmNum = Random.Range(0, array.Length - 1);
        return array[rdmNum];
    }
    
    public void copyGameState(GameState toCopy)
    {
        this.playerPosition = toCopy.playerPosition;
        this.ennemyPosition = toCopy.ennemyPosition;
        this.frisbeePosition = toCopy.frisbeePosition;
        this.frisbeeDirection = toCopy.frisbeeDirection;
        this.playerScore = toCopy.playerScore;
        this.ennemyScore = toCopy.ennemyScore;
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
        this.isHeld = toCopy.isHeld;
        this.lastHolder = toCopy.lastHolder;
        this.isScored = toCopy.isScored;
        this.isFinished = toCopy.isFinished;
    }
    
    public GameState(Vector3 playerPosition, Vector3 ennemyPosition, Vector3 frisbeePosition, Vector2 frisbeeDirection,
        int playerScore, int ennemyScore, bool isHeld, string lastHolder, bool isScored, bool isFinished)
    {
        this.playerPosition = playerPosition;
        this.ennemyPosition = ennemyPosition;
        this.frisbeePosition = frisbeePosition;
        this.frisbeeDirection = frisbeeDirection;
        this.playerScore = playerScore;
        this.ennemyScore = ennemyScore;
        this.isHeld = isHeld;
        this.lastHolder = lastHolder;
        this.isScored = isScored;
        this.isFinished = isFinished;
    }
}
