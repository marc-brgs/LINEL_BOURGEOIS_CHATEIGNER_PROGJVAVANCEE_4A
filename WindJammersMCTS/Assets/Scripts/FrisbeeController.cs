using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeController : MonoBehaviour
{
	
    public static FrisbeeController instance;

    public bool isHeld = false;
    public bool isMoving = false;
    public string lastHolder = "Ennemy";
    public Vector2 frisbeeDirection = new Vector2(-1, 1);

    public GameObject player;
    public GameObject ennemy;
    public GameObject borderTop;
    public GameObject borderBottom;

    public AudioClip CollisionWall;
    public AudioSource audioSource;

    private float frisbeeRadius;
    private float entityRadius;
    private float borderRadius;
    
    private GameState state;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Frisbee dans la scene");
            return;
        }
        instance = this;
    }
    

    void Start()
    {
        frisbeeRadius = this.transform.localScale.x / 2;
        entityRadius = player.transform.localScale.x / 2;
        borderRadius = borderTop.transform.localScale.z / 2;
    }

    public void moveOrStick(GameState state)
    {
        if (state.isMoving && !state.isHeld) // Move frisbee
        {
            state.frisbeePosition = new Vector3(state.frisbeePosition.x + state.frisbeeDirection.x / 2, 2.25f, state.frisbeePosition.z + state.frisbeeDirection.y / 2);
        }
        else if(state.isHeld) // Stick
        {
            if(state.lastHolder == "Player")
                state.frisbeePosition = state.playerPosition + new Vector3(-3.0f, 0f, 0f);
            else if(state.lastHolder == "Ennemy")
                state.frisbeePosition = state.ennemyPosition + new Vector3(3.0f, 0f, 0f);
        }
    }

    public void checkCatch(GameState state)
    {
        float distPlayer = Vector3.Distance(state.playerPosition, state.frisbeePosition);
        if (!state.isHeld && distPlayer < 2.5f && state.lastHolder != "Player") // Player frisbee catch
        {
            state.isHeld = true;
            state.isMoving = true; // first catch
            state.lastHolder = "Player";
        }
        
        float distEnnemy = Vector3.Distance(state.ennemyPosition, state.frisbeePosition);
        if (!state.isHeld && distEnnemy < 2.5f && state.lastHolder != "Ennemy") // Ennemy frisbee catch
        {
            state.isHeld = true;
            state.isMoving = true; // first catch
            state.lastHolder = "Ennemy";
        }
    }
    
    public void checkBorderCollisions(GameState state)
    {
        if (state.frisbeePosition.z + frisbeeRadius > borderTop.transform.position.z - borderRadius) // Frisbee collide border top
        {
            audioSource.clip = CollisionWall;
            audioSource.Play();
            state.frisbeeDirection = new Vector2(state.frisbeeDirection.x, -state.frisbeeDirection.y);
        }

        if (state.frisbeePosition.z - frisbeeRadius < borderBottom.transform.position.z + borderRadius) // Frisbee collide border bottom
        {
            audioSource.clip = CollisionWall;
            audioSource.Play();
            state.frisbeeDirection = new Vector2(state.frisbeeDirection.x, -state.frisbeeDirection.y);   
        }
    }
    
    /*
     *  Thow frisbee in given GameState depending of the thrower and aim
     */
    public void Shoot(GameState state, string aim="BOTTOM")
    {
       if(state.isHeld == true)
       {
           state.isHeld = false;
           if (state.lastHolder == "Player")
           { 
               state.frisbeePosition = new Vector3(state.frisbeePosition.x, 2.25f, state.frisbeePosition.z);
               if(aim == "TOP") 
                   state.frisbeeDirection = new Vector2(-1, 1);
               else if(aim == "BOTTOM")
                   state.frisbeeDirection = new Vector2(-1, -1);
           }
           else if (state.lastHolder == "Ennemy")
           { 
               state.frisbeePosition = new Vector3(state.frisbeePosition.x, 2.25f, state.frisbeePosition.z);
               if(aim == "TOP") 
                   state.frisbeeDirection = new Vector2(1, 1);
               else if(aim == "BOTTOM")
                   state.frisbeeDirection = new Vector2(1, -1);
           }
       }
    }

}
