using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class requires a rigidbody2D component in the game object
[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{
    //Setting up the player control
    private PlayerControl playerControl;
    private Rigidbody2D rb;

    private void Awake() 
    {
        playerControl = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start() 
    {
        //Make sure the game object is not going to
        // fall when the game starts
        rb.gravityScale = 0;
    }
    private void OnEnable() 
    {
        playerControl.Enable();
    }
    private void OnDisable() 
    {
        playerControl.Disable();
    }

    //Because we are using physics I use Fixed Update to manage 
    //The character movement

    private void FixedUpdate() 
    {
        //Get the input value
        Vector2 movementValue = playerControl.Player.Move.ReadValue<Vector2>();

        //Move the player
        rb.AddForce(movementValue, ForceMode2D.Force);
    }
}
