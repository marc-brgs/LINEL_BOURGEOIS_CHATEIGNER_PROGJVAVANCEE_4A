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
    
    // input bool

    public bool isFinished;
    // public int result;

    public bool getScored()
    {
        return isScored;
    }

    public GameState(Vector3 playerPosition, Vector3 ennemyPosition, Vector3 frisbeePosition, Vector2 frisbeeDirection, int playerScore, int ennemyScore, bool isHeld, string lastHolder, bool isScored, bool isFinished)
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
