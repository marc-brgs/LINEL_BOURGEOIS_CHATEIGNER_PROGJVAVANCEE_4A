using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 20f;

    private Rigidbody rb;

    private float horizontalMovement;
    private float verticalMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalMovement, 0, verticalMovement) * moveSpeed;
    }
}