using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction Shoot;



    private void Awake()
    {
        playerInput = new PlayerInput();
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
}
