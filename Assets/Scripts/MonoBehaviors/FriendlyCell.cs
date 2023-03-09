using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Target))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class FriendlyCell : MonoBehaviour, IDamagable
{
    public delegate void ActionDie();
    public static event ActionDie FriendlyCellDied;
    public delegate void ActionLoose();
    public static event ActionLoose lost;
    private float healt;
    private Rigidbody2D rb;
    private SpriteRenderer ren;
    private Target target;
    private Collider2D col;
    [SerializeField] FriendlyCell_SO friendlyCell_SO;
    [SerializeField] GameObject onKilled_PS;
    public Vector3 Position
   {
       get
       {
           return transform.position;
       }
   }
    private void Awake() 
    {
        //Add the friendly cell to a static array
        //so let the bacterias can find her
        Bacteria.friendlyCells.Add(this);

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        target = GetComponent<Target>();
        ren = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        GameManager.friendlyCells++;
    }

    private void Start() 
    {
        //The friendly cell starts at maximum healt
        healt = friendlyCell_SO.maxHealt;
        col.enabled = true;

    }

    public void Damage(float damage)
    {
        healt -= damage;

        //obtain the amount of healt left
        float percent = healt/friendlyCell_SO.maxHealt;

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
        GameObject ps = Instantiate(onKilled_PS);
        ps.transform.position = transform.position;

        Bacteria.friendlyCells.Remove(this);
        GameManager.friendlyCells--;

        //If all the cells are dead player loose
        if(GameManager.friendlyCells <= 0 && lost != null)
        {
            lost();
        }


       if(FriendlyCellDied != null)
        FriendlyCellDied();

        col.enabled = false;
        gameObject.SetActive(false);
    }
}
