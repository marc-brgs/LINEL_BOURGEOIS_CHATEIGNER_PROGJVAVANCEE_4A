using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Vector3 playerPosition;
    public Vector2 playerSpeed;
    public Vector3 ennemyPosition;
    public Vector2 ennemySpeed;
    public Vector3 frisbeePosition;
    public Vector2 frisbeeDirection;
    public int playerScore;
    public int ennemyScore;
    public bool isHeld;
    public string lastHolder;
    public bool isScored;
    
    // input bool

    public bool isFinished;
    // public int result;


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
        int RdmNum = Random.Range(0, array.Length - 1);
        return array[RdmNum];
    }
    
    public GameState(Vector3 playerPosition, Vector2 playerSpeed, Vector3 ennemyPosition, Vector2 ennemySpeed, Vector3 frisbeePosition, Vector2 frisbeeDirection,
        int playerScore, int ennemyScore, bool isHeld, string lastHolder, bool isScored, bool isFinished)
    {
        this.playerPosition = playerPosition;
        this.playerSpeed = playerSpeed;
        this.ennemyPosition = ennemyPosition;
        this.ennemySpeed = ennemySpeed;
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
