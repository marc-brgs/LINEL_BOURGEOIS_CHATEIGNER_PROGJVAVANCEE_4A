using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEditor.Search;
using UnityEngine;

public class RandomAgent : MonoBehaviour
{
    private bool actionExecute = true;
    
    private float horizontalInput;
    private float verticalInput;
    
    private float entityRadius;
    private GameObject borderTop;
    private GameObject borderBottom;
    private GameObject borderLeft;
    private GameObject borderRight;
    private float borderRadius;
    
    public string operatingSide;

    private GameState state;

    // Start is called before the first frame update
    void Start()
    {
        entityRadius = this.transform.localScale.x / 1.2f;
        borderTop = GameManager.instance.borderTop;
        borderBottom = GameManager.instance.borderBottom;
        borderLeft = GameManager.instance.borderLeft;
        borderRight = GameManager.instance.borderRight;
        borderRadius = GameManager.instance.borderRadius;

        state = GameManager.instance.State;
    }

    void Update()
    {
        if (actionExecute)
        {
            StartCoroutine(randomizeInput());
            tryToShoot(state);
        }
    }
    
    void FixedUpdate()
    {
        checkCollisions(GameManager.instance.State);
        Move(GameManager.instance.State);
    }

    private IEnumerator randomizeInput()
    {
        actionExecute = false;
        horizontalInput = Random.Range(-1f, 1f);
        verticalInput = Random.Range(-1f, 1f);
        yield return new WaitForSeconds(1f);
        actionExecute = true;
    }

    private void Move(GameState state)
    {
        state.ennemyPosition = new Vector3(state.ennemyPosition.x + horizontalInput/1.5f, 2.5f, state.ennemyPosition.z + verticalInput/1.5f);
    }

    private void tryToShoot(GameState state)
    {
        if (FrisbeeController.instance.isHeld && FrisbeeController.instance.lastHolder == "Ennemy")
        {
            if (Random.Range(1, 10) > 3) // 70% chance to shoot
            {
                FrisbeeController.instance.Shoot(state, Random.Range(1, 10) > 5 ? "TOP" : "BOTTOM"); // 50 % chance to aim TOP or BOTTOM
            }
        }
    }
    
    private void checkCollisions(GameState state)
    {
        if (operatingSide == "Left" && state.ennemyPosition.x + entityRadius > 0) // Left side middle border
        {
            if (horizontalInput > 0)
                horizontalInput = 0;
        }
        if (operatingSide == "Right" && state.ennemyPosition.x - entityRadius < 0) // Right side middle border
        {
            if (horizontalInput < 0)
                horizontalInput = 0;
        }
        if (state.ennemyPosition.z + entityRadius > borderTop.transform.position.z - borderRadius) // Border top
        {
            if (verticalInput > 0)
                verticalInput = 0;
        }
        if (state.ennemyPosition.z - entityRadius < borderBottom.transform.position.z + borderRadius) // Border bottom
        {
            if (verticalInput < 0)
                verticalInput = 0;
        }
        if (state.ennemyPosition.x - entityRadius < borderLeft.transform.position.x + borderRadius) // Border left
        {
            if (horizontalInput < 0)
                horizontalInput = 0;
        }
        if (state.ennemyPosition.x + entityRadius > borderRight.transform.position.x - borderRadius) // Border right
        {
            if (horizontalInput > 0)
                horizontalInput = 0;
        }
    }
}
