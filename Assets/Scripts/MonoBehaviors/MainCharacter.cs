using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class requires a rigidbody2D component in the game object
[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour, IDamagable
{
    //Setting up the player control
    private PlayerControl playerControl;
    public static List<IDamagable> bacterias = new List<IDamagable>();
    private Rigidbody2D rb;
    private float healt;
    [SerializeField] GameObject cameraTracker;
    [SerializeField] MainCharacter_SO mainCharacter_SO;
    [SerializeField] HealtBar healtBar;
    public static bool isExhausted;
    public Vector3 Position
   {
       get
       {
           return transform.position;
       }
   }

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

    //Because I am using physics I use FixedUpdate to manage 
    //The character movement

    private void FixedUpdate() 
    {

        //Get the input value
        Vector2 movementValue = playerControl.Player.Move.ReadValue<Vector2>();

        //Move the player
        rb.AddForce(movementValue * mainCharacter_SO.force, ForceMode2D.Force);

        //move the tracker for the camera
        Vector2 trackerPosition = new Vector2
        (
            transform.position.x + movementValue.x,
            transform.position.y + movementValue.y
        );
        cameraTracker.transform.position = trackerPosition;

        //Update the exhausted state for the bacteria class
        isExhausted = mainCharacter_SO.Exhausted;

        //Attack close enemies
        foreach (IDamagable bacteria in bacterias)
        {
            //If there is an enemy chose and we are not exhausted
            if(Vector2.Distance(transform.position, bacteria.Position) <= mainCharacter_SO.attackRange && !isExhausted)
            {
                //Generate damage on the bacteria
                bacteria.Damage(mainCharacter_SO.damage);
            }
        }
    }

    public void Damage(float damage)
    {
        //Only get damage if I'm exhausted
        if(isExhausted)
        {
            healt -= damage;
            healtBar.On_damaged(mainCharacter_SO.maxHealt, healt);
        }
    }
}
