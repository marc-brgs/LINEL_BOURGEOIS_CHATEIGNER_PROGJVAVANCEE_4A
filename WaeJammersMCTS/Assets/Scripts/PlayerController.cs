using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public string side;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private Vector2 playerSpeed = new Vector2(0f, 0f);

    private PlayerInput playerInput;
    private InputAction ShootTop;
    private InputAction ShootBottom;


    private float entityRadius;
    private GameObject borderTop;
    private GameObject borderBottom;
    private GameObject borderLeft;
    private GameObject borderRight;
    private float borderRadius;
    private float filetRadius;

    private GameState state;
    
    private void Awake()
    { 
        playerInput = new PlayerInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        entityRadius = GameManager.instance.entityRadius;
        borderTop = GameManager.instance.borderTop;
        borderBottom = GameManager.instance.borderBottom;
        borderLeft = GameManager.instance.borderLeft;
        borderRight = GameManager.instance.borderRight;
        borderRadius = GameManager.instance.borderRadius;
        filetRadius = GameManager.instance.filetRadius;
        
        state = GameManager.instance.State;
    }

    private void OnEnable()
    {
        playerInput.Player.ShootTop.performed += DoShootTop;
        playerInput.Player.ShootTop.Enable();

        playerInput.Player.ShootBottom.performed += DoShootBottom;
        playerInput.Player.ShootBottom.Enable();
    }
    
    private void DoShootTop(InputAction.CallbackContext obj)
    {
        FrisbeeController.instance.Shoot(GameManager.instance.State, "TOP");
    }

    private void DoShootBottom(InputAction.CallbackContext obj)
    {
        FrisbeeController.instance.Shoot(GameManager.instance.State, "BOTTOM");
    }

    
    void Update()
    {
        GetInputs();
        
        if(Input.GetMouseButtonDown(0) && GameManager.instance.State.lastHolder == "Player") // Release frisbee BOTTOM
            FrisbeeController.instance.Shoot(GameManager.instance.State, "BOTTOM");
        else if(Input.GetMouseButtonDown(1) && GameManager.instance.State.lastHolder == "Player") // Release TOP
            FrisbeeController.instance.Shoot(GameManager.instance.State, "TOP");
    }
    
    void FixedUpdate()
    {
        checkCollisions(GameManager.instance.State);

        if (horizontalInput == 0f && verticalInput == 0f) return; // Don't modify GameState
        Move(GameManager.instance.State);
    }
    
    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    
    private void Move(GameState state)
    {
        state.playerPosition = new Vector3(state.playerPosition.x + horizontalInput/1.5f, state.playerPosition.y, state.playerPosition.z + verticalInput/1.5f);
    }
    
    private void checkCollisions(GameState state)
    {
        if (side == "LEFT" && state.playerPosition.x + entityRadius > 0 + filetRadius) // Left side middle border
        {
            if (horizontalInput > 0)
                horizontalInput = 0;
        }
        if (side == "RIGHT" && state.playerPosition.x - entityRadius < 0 - filetRadius) // Right side middle border
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
