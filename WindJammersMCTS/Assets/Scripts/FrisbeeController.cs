using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeController : MonoBehaviour
{
	
    public static FrisbeeController instance;

    public bool isHeld = false;
    public bool isMoving = false;
    public string lastHolder = "Ennemy";

    private float frisbeeSpeed = 20f;
    
    private Vector2 frisbeeDirection = new Vector2(-1, 1);

    private Rigidbody rb;

    public GameObject player;
    public GameObject ennemy;
    public GameObject borderTop;
    public GameObject borderBottom;

    private float frisbeeRadius;
    private float entityRadius;
    private float borderRadius;

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
        rb = GetComponent<Rigidbody>();
        
        frisbeeRadius = this.transform.localScale.x / 2;
        entityRadius = player.transform.localScale.x / 2;
        borderRadius = borderTop.transform.localScale.z / 2;
    }

    void Update()
    {
        float distPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (!isHeld && distPlayer < 2.5f && lastHolder != "Player") // Catch
        {
            isHeld = true;
            isMoving = true; // first catch
            lastHolder = "Player";
        }

        float distEnnemy = Vector3.Distance(ennemy.transform.position, transform.position);
        if (!isHeld && distEnnemy < 2.5f && lastHolder != "Ennemy") // Catch
        {
            isHeld = true;
            isMoving = true; // first catch
            lastHolder = "Ennemy";
        }

        if(isHeld && Input.GetMouseButtonDown(0)) // Release frisbee
        {
            isHeld = false;
            if (lastHolder == "Player")
            {
                transform.position = new Vector3(transform.position.x, 2.25f, transform.position.z);
                frisbeeDirection = new Vector2(-1, 1);
            }
            else if (lastHolder == "Ennemy")
            {
                transform.position = new Vector3(transform.position.x, 2.25f, transform.position.z);
                frisbeeDirection = new Vector2(1, -1);
            }
        }
    }
    void FixedUpdate()
    {
        checkCollisions();
        
        if (isMoving && !isHeld) // Move frisbee
        {
            this.transform.position = new Vector3(this.transform.position.x + frisbeeDirection.x/2, 2.25f, this.transform.position.z + frisbeeDirection.y/2);
        }
        else if(isHeld)// Stick
        {
            if(lastHolder == "Player")
                transform.position = player.transform.position + new Vector3(-3.0f, 0f, 0f);
            else if(lastHolder == "Ennemy")
                transform.position = ennemy.transform.position + new Vector3(3.0f, 0f, 0f);
        }
    }

    public void checkCollisions()
    {
        if (this.transform.position.z + frisbeeRadius > borderTop.transform.position.z - borderRadius) // frisbee collide border top
        {
            frisbeeDirection = new Vector2(frisbeeDirection.x, -frisbeeDirection.y);
        }

        if (this.transform.position.z - frisbeeRadius < borderBottom.transform.position.z + borderRadius) // frisbee collide border bottom
        {
            frisbeeDirection = new Vector2(frisbeeDirection.x, -frisbeeDirection.y);
        }
    }
    
    public void Shoot()
    {
       if(isHeld == true)
        {
            isHeld = false;
            if (lastHolder == "Player")
            {
                transform.position = new Vector3(transform.position.x, 2.25f, transform.position.z);
                frisbeeDirection = new Vector2(-1, 1);
            }
            else if (lastHolder == "Ennemy")
            {
                transform.position = new Vector3(transform.position.x, 2.25f, transform.position.z);
                frisbeeDirection = new Vector2(1, -1);
            }
        }
    }

}
