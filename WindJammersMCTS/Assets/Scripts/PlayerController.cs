using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 26f;

    private Rigidbody rb;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private float dashingTime = 0.2f;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashingPower = 30f;
    private float dashX = 0f;
    private float dashY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        GetInputs();
    }
    void FixedUpdate()
    {
        if (isDashing) return;
        Move();
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(1))
            StartCoroutine(Dash());
    }
    private void Move()
    {
        rb.velocity = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        dashX = 0f;
        dashY = 0f;
        if (horizontalInput != 0f)
            dashX = horizontalInput / Mathf.Abs(horizontalInput);
        if(verticalInput != 0f)
            dashY = verticalInput / Mathf.Abs(verticalInput);

        rb.velocity = new Vector3(dashX, 0f, dashY) * dashingPower;


        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        canDash = true;
    }
}
