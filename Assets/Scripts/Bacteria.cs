using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bacteria : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    [SerializeField] float force;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    private void Start() 
    {
        //The bacteria choose a random friendly cell to attack
        int cell = Random.Range(0, FriendlyCell.friendlyCells.Count);
        Debug.Log("cell " + cell);
        target = FriendlyCell.friendlyCells[cell].transform;

        Debug.Log("target " + target.name);
    }

    private void FixedUpdate() 
    {
        //Move the bacteria to the target
        Vector2 vectorToTarget = target.position - transform.position;
        rb.AddForce(vectorToTarget * force, ForceMode2D.Force);
    }

}
