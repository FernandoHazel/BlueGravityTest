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
    [SerializeField] float force;
    [SerializeField] GameObject cameraTracker;

    public static bool isExhausted = false;

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
        StartCoroutine(Exhauste());
    }
    private void OnEnable() 
    {
        playerControl.Enable();
    }
    private void OnDisable() 
    {
        playerControl.Disable();
    }

    //Because I am using physics I use FixedUpdate to manage 
    //The character movement

    private void FixedUpdate() 
    {
        //Get the input value
        Vector2 movementValue = playerControl.Player.Move.ReadValue<Vector2>();

        //Move the player
        rb.AddForce(movementValue * force, ForceMode2D.Force);

        //move the tracker for the camera
        Vector2 trackerPosition = new Vector2
        (
            transform.position.x + movementValue.x,
            transform.position.y + movementValue.y
        );

        cameraTracker.transform.position = trackerPosition;
    }

    IEnumerator Exhauste()
    {
        yield return new WaitForSeconds(4);
        isExhausted = true;
        Debug.Log("Exhausted");
    }
}
