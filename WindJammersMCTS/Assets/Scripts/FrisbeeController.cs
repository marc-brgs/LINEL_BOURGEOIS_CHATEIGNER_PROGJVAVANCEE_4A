using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeController : MonoBehaviour
{
    private bool isFixed = false;
    private string lastHolder = "Player";

    private float frisbeeSpeed = 20f;

    private float directionX = -1;
    private float directionY = 1;

    private Rigidbody rb;

    public GameObject player;
    public GameObject ennemy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float distPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distPlayer < 2.5f && lastHolder != "Player") // Catch
        {
            isFixed = true;
            lastHolder = "Player";
            Debug.Log("Player catch");
        }

        float distEnnemy = Vector3.Distance(ennemy.transform.position, transform.position);
        if (distEnnemy < 2.5f && lastHolder != "Ennemy") // Catch
        {
            isFixed = true;
            lastHolder = "Ennemy";
            Debug.Log("Ennemy catch");
        }

        if(isFixed && Input.GetMouseButtonDown(0)) // Release frisbee
        {
            isFixed = false;
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
        if (!isFixed) // Move frisbee
        {
            rb.velocity = new Vector3(directionX, 0, directionY) * frisbeeSpeed;
        }
        else // Stick
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
        if (collision.CompareTag("EGoal"))
        {
            Scores.instance.PlayerScore += 1;
        }

        if (collision.CompareTag("PGoal"))
        {
            Scores.instance.EnnemyScore += 1;

        }
    }




}
