using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bacteria : MonoBehaviour, IDamagable
{
    private Vector3 target;
    private Rigidbody2D rb;
    private MainCharacter player;
    private float healt;
    [Tooltip("This is the time the bacteria will take to look for another target")]
    [SerializeField] Bacteria_SO bacteria_SO;
    [SerializeField] HealtBar healtBar;
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
    }

    private void OnEnable() 
    {
        //Register in the list of bacterias so the main character
        //can find and destroy them
        MainCharacter.bacterias.Add(this);
    }
    private void Start() 
    {
        //The bacteria starts with full healt 
        //And looking for a target
        healt = bacteria_SO.maxHealt;
        ChooseTarget();
    }

    private void FixedUpdate() 
    {
        //Move the bacteria to the target
        Vector2 vectorToTarget = target - transform.position;
        rb.AddForce(vectorToTarget * bacteria_SO.force, ForceMode2D.Force);

        //Check if the player is available to be attacked
        HuntPlayer();

        //Attack close enemies
        foreach (IDamagable cell in FriendlyCell.friendlyCells)
        {
            //If there is a cell close
            if(Vector2.Distance(transform.position, cell.Position) <= bacteria_SO.attackRange)
            {
                //Generate damage on the bacteria
                cell.Damage(bacteria_SO.damage);
            }
        }

        //Hit the player
        if(Vector2.Distance(transform.position, player.Position) <= bacteria_SO.attackRange)
        {
            //Generate damage on the player
            player.Damage(bacteria_SO.damage);
        }
    }

    private void ChooseTarget()
    {
        //The bacteria choose a random friendly cell to attack
        int cell = Random.Range(0, FriendlyCell.friendlyCells.Count);
        target = FriendlyCell.friendlyCells[cell].Position;
    }

    IEnumerator ReselectTarget()
    {
        yield return new WaitForSeconds(10);
        ChooseTarget();
        StartCoroutine(ReselectTarget());
    }

    //If the player is close and exhausted we attack him
    private void HuntPlayer()
    {
        float distanceToPlayer = 
        Vector2.Distance(transform.position, player.Position);

        if(distanceToPlayer < 4 && MainCharacter.isExhausted)
        {
            target = player.Position;
        }
    }

    public void Damage(float damage)
    {
        healt -= damage;
        healtBar.On_damaged(bacteria_SO.maxHealt, healt);
    }

}
