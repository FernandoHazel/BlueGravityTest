using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyCell : MonoBehaviour, IDamagable
{
    public static List<IDamagable> friendlyCells = new List<IDamagable>();
    private float healt;
    [SerializeField] FriendlyCell_SO friendlyCell_SO;
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
        //Add the friendly cell to a static array
        //so let the bacterias can find her
        friendlyCells.Add(this);
    }

    private void Start() 
    {
        //The friendly cell starts at maximum healt
        healt = friendlyCell_SO.maxHealt;
    }

    public void Damage(float damage)
    {
        healt -= damage;
        healtBar.On_damaged(friendlyCell_SO.maxHealt, healt);
    }

}
