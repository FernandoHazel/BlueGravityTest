using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class requires a rigidbody2D component in the game object
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Target))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class MainCharacter : MonoBehaviour, IDamagable
{
    //Setting up the player control
    private PlayerControl playerControl;
    public static List<IDamagable> bacterias = new List<IDamagable>();
    private Rigidbody2D rb;
    public static float healt;
    private Target target;
    private SpriteRenderer ren;
    [SerializeField] GameObject cameraTracker;
    [SerializeField] GameObject onKilled_PS;
    [SerializeField] MainCharacter_SO mainCharacter_SO;
    private HealthBar healthBar;
    private Collider2D col;
    private Animator anim;
    [SerializeField] AudioClip killed;
    private AudioSource AS;
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
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();
        healthBar = GameObject.FindObjectOfType<HealthBar>();

        //Suppose the player has no upgrades at the begging of the level (just for test)
        //This can differ from the store
        //I will change this if I add a save system
        mainCharacter_SO.modifiedMaxHealth = mainCharacter_SO.maxHealth;
        mainCharacter_SO.modifiedForce = mainCharacter_SO.force;
        mainCharacter_SO.modifiedAttackRange = mainCharacter_SO.attackRange;
        mainCharacter_SO.modifiedDamage = mainCharacter_SO.damage;
        mainCharacter_SO.inGameProteins = mainCharacter_SO.defaultProteins;

    }
    private void Start() 
    {
        //Make sure the game object is not going to
        // fall when the game starts
        rb.gravityScale = 0;
        healt = mainCharacter_SO.modifiedMaxHealth;
        col.enabled = true;
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
        rb.AddForce(movementValue * mainCharacter_SO.modifiedForce, ForceMode2D.Force);

        //move the tracker for the camera
        Vector2 trackerPosition = new Vector2
        (
            transform.position.x + movementValue.x,
            transform.position.y + movementValue.y
        );
        cameraTracker.transform.position = trackerPosition;

        //Attack close enemies
        if(bacterias.Count > 0)
        {
            foreach (IDamagable bacteria in bacterias)
            {
                //If there is an enemy chose and we are not exhausted
                if(Vector2.Distance(transform.position, bacteria.Position) <= mainCharacter_SO.attackRange)
                {
                    //Generate damage on the bacteria
                    bacteria.Damage(mainCharacter_SO.modifiedDamage * Time.deltaTime);

                    if(Time.frameCount % 30 == 0)
                    {
                        AS.PlayOneShot(killed);
                    }
                }
            }
        }
    }

    public void Damage(float damage)
    {
        healt -= damage;

        //obtain the amount of healt left
        float percent = healt/mainCharacter_SO.modifiedMaxHealth;
        healthBar.On_damaged(percent);

        //Adjust the sprite color
        Color newColor = new Color(255, 255 * percent, 255 * percent, 255);
        ren.color = newColor;

        if(healt <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        AS.PlayOneShot(killed);

        GameObject ps = Instantiate(onKilled_PS);
        ps.transform.position = transform.position;

        col.enabled = false;
        
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1);
        //Hide the protein Item
        gameObject.SetActive(false);
    }
}
