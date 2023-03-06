using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FriendlyCell : MonoBehaviour, IDamagable
{
    public static List<IDamagable> friendlyCells = new List<IDamagable>();
    private float healt;
    private Rigidbody2D rb;
    private SpriteRenderer ren;
    private Target target;
    [SerializeField] FriendlyCell_SO friendlyCell_SO;
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
        //Add the friendly cell to a static array
        //so let the bacterias can find her
        friendlyCells.Add(this);

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        target = GetComponent<Target>();
        ren = GetComponent<SpriteRenderer>();
    }

    private void Start() 
    {
        //The friendly cell starts at maximum healt
        healt = friendlyCell_SO.maxHealt;
        dead = false;
    }

    public void Damage(float damage)
    {
        healt -= damage;

        //Invoke event on target to update the healtbar
        //target.TargetDamaged(friendlyCell_SO.maxHealt, healt);

        //obtain the amount of healt left
        float percent = healt/friendlyCell_SO.maxHealt;

        //Adjust the sprite color
        Color newColor = new Color(255, 255 * percent, 255 * percent, 255);
        ren.color = newColor;

        //Debug.Log(gameObject.name + percent );

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
