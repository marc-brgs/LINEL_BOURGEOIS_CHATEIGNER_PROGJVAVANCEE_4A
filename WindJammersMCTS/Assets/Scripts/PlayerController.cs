using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 26f;
    public string operatingSide;
    
    private Rigidbody rb;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private float dashingTime = 0.2f;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashingPower = 30f;
    private float dashX = 0f;
    private float dashY = 0f;

    private PlayerInput playerInput;
    private InputAction Shoot;

    private float entityRadius;
    private GameObject borderTop;
    private GameObject borderBottom;
    private GameObject borderLeft;
    private GameObject borderRight;
    private float borderRadius;
    
    private void Awake()
    { 
        playerInput = new PlayerInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        entityRadius = this.transform.localScale.x / 1.2f;
        borderTop = GameManager.instance.borderTop;
        borderBottom = GameManager.instance.borderBottom;
        borderLeft = GameManager.instance.borderLeft;
        borderRight = GameManager.instance.borderRight;
        borderRadius = GameManager.instance.borderRadius;
    }

    private void OnEnable()
    {
        playerInput.Player.Shoot.performed += DoShoot;
        playerInput.Player.Shoot.Enable();
    }


    private void DoShoot(InputAction.CallbackContext obj)
    {
            FrisbeeController.instance.Shoot();
    }

    void Update()
    {
        GetInputs();
    }
    
    void FixedUpdate()
    {
        checkCollisions();
        
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
        if (horizontalInput == 0f && verticalInput == 0)
            yield return new WaitForSeconds(0f);
        
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

    private void checkCollisions()
    {
        if (operatingSide == "Left" && this.transform.position.x + entityRadius > 0) // Left side middle border
        {
            if (horizontalInput > 0)
                horizontalInput = 0;
        }
        if (operatingSide == "Right" && this.transform.position.x - entityRadius < 0) // Right side middle border
        {
            if (horizontalInput < 0)
                horizontalInput = 0;
        }
        if (this.transform.position.z + entityRadius > borderTop.transform.position.z - borderRadius) // Border top
        {
            if (verticalInput > 0)
                verticalInput = 0;
        }
        if (this.transform.position.z - entityRadius < borderBottom.transform.position.z + borderRadius) // Border bottom
        {
            if (verticalInput < 0)
                verticalInput = 0;
        }
        if (this.transform.position.x - entityRadius < borderLeft.transform.position.x + borderRadius) // Border left
        {
            if (horizontalInput < 0)
                horizontalInput = 0;
        }
        if (this.transform.position.x + entityRadius > borderRight.transform.position.x - borderRadius) // Border right
        {
            if (horizontalInput > 0)
                horizontalInput = 0;
        }
    }
}
