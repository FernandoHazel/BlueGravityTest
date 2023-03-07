using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bacteria : MonoBehaviour, IDamagable
{
    private Vector3 targetPos;
    private Rigidbody2D rb;
    private IDamagable player;
    private float healt;
    private SpriteRenderer ren;
    private Target target;
    [Tooltip("This is the time the bacteria will take to look for another target")]
    [SerializeField] Bacteria_SO bacteria_SO;
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
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        target = GetComponent<Target>();
        ren = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() 
    {
        //Register in the list of bacterias so the main character
        //can find and destroy them
        MainCharacter.bacterias.Add(this);

        player = GameObject.Find("Player").GetComponent<IDamagable>();
    }
    private void Start() 
    {
        //The bacteria starts with full healt 
        //And looking for a target
        healt = bacteria_SO.maxHealt;
        dead = false;
        ChooseTarget();
    }

    private void FixedUpdate() 
    {
        //Move the bacteria to the target
        Vector2 vectorToTarget = targetPos - transform.position;
        rb.AddForce(vectorToTarget * bacteria_SO.force, ForceMode2D.Force);

        //Check if the player is available to be attacked
        HuntPlayer();

        //Attack close enemies
        foreach (IDamagable cell in FriendlyCell.friendlyCells)
        {
            //Verify if the cell is not killed in the previous frame
            if (!cell.isDead)
            {
                //If there is a cell close
                if(Vector2.Distance(transform.position, cell.Position) <= bacteria_SO.attackRange)
                {
                    //Generate damage on the bacteria
                    cell.Damage(bacteria_SO.damage * Time.deltaTime);
                }
            }
            
        }

        
    }

    private void ChooseTarget()
    {
        //The bacteria choose a random friendly cell to attack
        int cell = Random.Range(0, FriendlyCell.friendlyCells.Count);
        targetPos = FriendlyCell.friendlyCells[cell].Position;
    }

    IEnumerator ReselectTarget()
    {
        yield return new WaitForSeconds(20);
        ChooseTarget();
        StartCoroutine(ReselectTarget());
    }

    //If the player is close and exhausted we attack him
    private void HuntPlayer()
    {
        if(player.isDead == false)
        {
            float distanceToPlayer = 
            Vector2.Distance(transform.position, player.Position);

            if(distanceToPlayer < 4 && MainCharacter.isExhausted)
            {
                Debug.Log(gameObject.name+" hunting player");
                targetPos = player.Position;

                //Hit the player
                if(distanceToPlayer <= bacteria_SO.attackRange)
                {
                    //Generate damage on the player
                    player.Damage(bacteria_SO.damage * Time.deltaTime);
                }
            }
        }   
    }

    public void Damage(float damage)
    {
        healt -= damage;
        
        //Invoke event on target to update the healtbar
        //target.TargetDamaged(bacteria_SO.maxHealt, healt);

        //obtain the amount of healt left
        float percent = healt/bacteria_SO.maxHealt;

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
        dead = true;
        StartCoroutine(DestroyObject());
    }
    
    IEnumerator DestroyObject()
    {
        //wait top destroy to avoid missing reference exceptions
        //In the damage process
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }

    

}
