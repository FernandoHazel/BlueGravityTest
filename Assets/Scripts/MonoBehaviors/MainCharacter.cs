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
    private Target target;
    private SpriteRenderer ren;
    [SerializeField] GameObject cameraTracker;
    [SerializeField] MainCharacter_SO mainCharacter_SO;
    private HealthBar healthBar;
    public static bool isExhausted;
    private bool dead;
    public bool isDead
    {
        get
       {
           return dead;
       }
    }
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
        target = GetComponent<Target>();
        ren = GetComponent<SpriteRenderer>();
        healthBar = GameObject.FindObjectOfType<HealthBar>();

    }
    private void Start() 
    {
        //Make sure the game object is not going to
        // fall when the game starts
        rb.gravityScale = 0;
        dead = false;
        healt = mainCharacter_SO.maxHealt;

        //this is just for test
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
        //Debug.Log("Exhausted "+ isExhausted);

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

        //Attack close enemies
        foreach (IDamagable bacteria in bacterias)
        {
            if(!bacteria.isDead)
            {
                //If there is an enemy chose and we are not exhausted
                if(Vector2.Distance(transform.position, bacteria.Position) <= mainCharacter_SO.attackRange && isExhausted == false)
                {
                    //Generate damage on the bacteria
                    bacteria.Damage(mainCharacter_SO.damage * Time.deltaTime);
                }
            }
        }
    }

    public void Damage(float damage)
    {
        //Only get damage if I'm exhausted
        if(isExhausted == true)
        {
            //Debug.Log("Taking damage " + damage);
            healt -= damage;
            
            //Invoke event on target to update the healtbar
            //target.TargetDamaged(mainCharacter_SO.maxHealt, healt);

            //obtain the amount of healt left
            float percent = healt/mainCharacter_SO.maxHealt;
            healthBar.On_damaged(percent);

            //Adjust the sprite color
            Color newColor = new Color(255, 255 * percent, 255 * percent, 255);
            ren.color = newColor;

            if(healt <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        dead = true;
        StartCoroutine(DestroyObject());
    }
    
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(.1f);
        ren.enabled = false;
    }

    //This is just for test
    IEnumerator Exhauste()
    {
        //wait top destroy to avoid missing reference exceptions
        //In the damage process
        yield return new WaitForSeconds(5);
        isExhausted = true;
        
    }
}
