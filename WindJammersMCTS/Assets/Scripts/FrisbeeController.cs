using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeController : MonoBehaviour
{
	
    public static FrisbeeController instance;

    private bool isHeld = false;
    private bool isMoving = false;
    private string lastHolder = "Ennemy";

    private float frisbeeSpeed = 20f;

    private float directionX = -1;
    private float directionY = 1;

    private Rigidbody rb;

    public GameObject player;
    public GameObject ennemy;

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
    }

    void Update()
    {
        float distPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distPlayer < 2.5f && lastHolder != "Player") // Catch
        {
            isHeld = true;
            isMoving = true; // first catch
            lastHolder = "Player";
            Debug.Log("Player catch");
        }

        float distEnnemy = Vector3.Distance(ennemy.transform.position, transform.position);
        if (distEnnemy < 2.5f && lastHolder != "Ennemy") // Catch
        {
            isHeld = true;
            isMoving = true; // first catch
            lastHolder = "Ennemy";
            Debug.Log("Ennemy catch");
        }

        if(isHeld && Input.GetMouseButtonDown(0)) // Release frisbee
        {
            isHeld = false;
            if (lastHolder == "Player")
            {
                transform.position = new Vector3(transform.position.x, 2.25f, transform.position.z);
                directionX = -1;
                directionY = 1;
            }
            else if (lastHolder == "Ennemy")
            {
                transform.position = new Vector3(transform.position.x, 2.25f, transform.position.z);
                directionX = 1;
                directionY = -1;
            }
        }
    }
    void FixedUpdate()
    {
        if (isMoving && !isHeld) // Move frisbee
        {
            rb.velocity = new Vector3(directionX, 0, directionY) * frisbeeSpeed;
        }
        else if(isHeld)// Stick
        {
            if(lastHolder == "Player")
                transform.position = player.transform.position + new Vector3(-3.0f, 0f, 0f);
            else if(lastHolder == "Ennemy")
                transform.position = ennemy.transform.position + new Vector3(3.0f, 0f, 0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border")) // Bounce
        {
            if (collision.transform.localScale.x > collision.transform.localScale.z) // Horizontal border
            {
                directionY = -directionY;
            }
            else // Vertical border
            {
                directionX = -directionX;
            }
        }

    }
    
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("EGoal")) // Player scored
        {
            Scores.instance.PlayerScore += 1;
            lastHolder = "Player";
            this.transform.position = new Vector3(-6f, 2.25f, 0f); // Set frisbee in ennemy zone
            isMoving = false;
            isHeld = false;
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (collision.CompareTag("PGoal")) // Ennemy scored
        {
            Scores.instance.EnnemyScore += 1;
            lastHolder = "Ennemy";
            this.transform.position = new Vector3(6f, 2.25f, 0f); // Set frisbee in player zone
            isMoving = false;
            isHeld = false;
            rb.velocity = new Vector3(0, 0, 0);
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
                directionX = -1;
                directionY = 1;
            }
            else if (lastHolder == "Ennemy")
            {
                transform.position = new Vector3(transform.position.x, 2.25f, transform.position.z);
                directionX = 1;
                directionY = -1;
            }
        }
    }

}
