using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Target))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Bacteria : MonoBehaviour, IDamagable
{
    public delegate void ActionDie();
    public static event ActionDie bacteriaDied;
    public delegate void ActionBorn();
    public static event ActionBorn bacteriaBorn;
    public delegate void ActionWin();
    public static event ActionWin Won;
    public static List<IDamagable> friendlyCells = new List<IDamagable>();
    private Vector3 targetPos;
    private Rigidbody2D rb;
    private GameObject player;
    private IDamagable IPlayer;
    private float healt;
    private SpriteRenderer ren;
    private Collider2D col;
    private Target target;
    [Tooltip("This is the time the bacteria will take to look for another target")]
    [SerializeField] Bacteria_SO bacteria_SO;
    [SerializeField] GameObject OnKilled_PS;
    
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
        player = GameObject.Find("Player");
        IPlayer = GameObject.Find("Player").GetComponent<IDamagable>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable() 
    {
        col.enabled = true;

        //Register the spawn of the bacteria on the GameManager
        GameManager.activeEnemies++;
        if(bacteriaBorn != null)
        bacteriaBorn();

        //Register in the list of bacterias so the main character
        //can find and destroy them
        MainCharacter.bacterias.Add(this);

        //The bacteria starts with full healt 
        //And looking for a target
        healt = bacteria_SO.maxHealt;
        ChooseTarget();
    }

    private void FixedUpdate() 
    {
        //If the bacteria is dead can't do anything
 
        //Move the bacteria to the target
        Vector2 vectorToTarget = targetPos - transform.position;
        rb.AddForce(vectorToTarget * bacteria_SO.force, ForceMode2D.Force);

        //Check if the player is available to be attacked
        HuntPlayer();

        if(friendlyCells.Count > 0)
        {
            //Attack close enemies
            foreach (IDamagable cell in friendlyCells)
            {
                if(cell != null)
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
    }

    private void ChooseTarget()
    {
        if(friendlyCells.Count > 0)
        {
            //The bacteria choose a random friendly cell to attack
            int cell = Random.Range(0, friendlyCells.Count);
            targetPos = friendlyCells[cell].Position;
        }
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
        if(player.activeSelf)
        {
            float distanceToPlayer = 
            Vector2.Distance(transform.position, IPlayer.Position);

            if(distanceToPlayer < 2)
            {
                //Debug.Log(gameObject.name+" hunting player");
                targetPos = IPlayer.Position;

                //Hit the player
                if(distanceToPlayer <= bacteria_SO.attackRange)
                {
                    //Generate damage on the player
                    IPlayer.Damage(bacteria_SO.damage * Time.deltaTime);
                }
            }
        }
        
    }

    public void Damage(float damage)
    {
        healt -= damage;

        if(healt <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDisable() 
    {
        MainCharacter.bacterias.Remove(this);
        GameManager.activeEnemies--;
        
        GameObject ps = Instantiate(OnKilled_PS);
        ps.transform.position = transform.position;

        //If all the enemies are dead player wins
        if(GameManager.activeEnemies <= 0 && GameManager.canWin && Won != null)
        {
            Won();
        }

        if(bacteriaDied != null)
        bacteriaDied();

        col.enabled = false;
    }
}
