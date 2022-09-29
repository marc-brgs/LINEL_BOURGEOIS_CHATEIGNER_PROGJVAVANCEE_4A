using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 26f;
    public string operatingSide;

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
    
    GameState state;
    
    private void Awake()
    { 
        playerInput = new PlayerInput();
    }

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

    private void OnEnable()
    {
        playerInput.Player.Shoot.performed += DoShoot;
        playerInput.Player.Shoot.Enable();
    }
    
    private void DoShoot(InputAction.CallbackContext obj)
    {
        FrisbeeController.instance.Shoot(state);
    }
    
    void Update()
    {
        GetInputs();
    }
    
    void FixedUpdate()
    {
        checkCollisions(GameManager.instance.State);

        Move();
    }
    
    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    
    private void Move()
    {
        GameManager.instance.State.playerPosition = new Vector3(GameManager.instance.State.playerPosition.x + horizontalInput/1.5f, 2.5f, GameManager.instance.State.playerPosition.z + verticalInput/1.5f);
        //this.transform.position = new Vector3(this.transform.position.x + horizontalInput/1.5f, 2.5f, this.transform.position.z + verticalInput/1.5f);
    }

    private void checkCollisions(GameState state)
    {
        if (operatingSide == "Left" && state.playerPosition.x + entityRadius > 0) // Left side middle border
        {
            if (horizontalInput > 0)
                horizontalInput = 0;
        }
        if (operatingSide == "Right" && state.playerPosition.x - entityRadius < 0) // Right side middle border
        {
            if (horizontalInput < 0)
                horizontalInput = 0;
        }
        if (state.playerPosition.z + entityRadius > borderTop.transform.position.z - borderRadius) // Border top
        {
            if (verticalInput > 0)
                verticalInput = 0;
        }
        if (state.playerPosition.z - entityRadius < borderBottom.transform.position.z + borderRadius) // Border bottom
        {
            if (verticalInput < 0)
                verticalInput = 0;
        }
        if (state.playerPosition.x - entityRadius < borderLeft.transform.position.x + borderRadius) // Border left
        {
            if (horizontalInput < 0)
                horizontalInput = 0;
        }
        if (state.playerPosition.x + entityRadius > borderRight.transform.position.x - borderRadius) // Border right
        {
            if (horizontalInput > 0)
                horizontalInput = 0;
        }
    }
}
