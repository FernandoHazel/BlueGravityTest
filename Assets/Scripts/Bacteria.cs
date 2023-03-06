using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bacteria : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private GameObject player;
    [SerializeField] float force;
    [Tooltip("This is the time the bacteria will take to look for another target")]
    [SerializeField] float ReselectTargetTime;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        player = GameObject.Find("Player");
    }
    private void Start() 
    {
        ChooseTarget();
    }

    private void FixedUpdate() 
    {
        //Move the bacteria to the target
        Vector2 vectorToTarget = target.position - transform.position;
        rb.AddForce(vectorToTarget * force, ForceMode2D.Force);

        HuntPlayer();
    }

    private void ChooseTarget()
    {
        //The bacteria choose a random friendly cell to attack
        int cell = Random.Range(0, FriendlyCell.friendlyCells.Count);
        target = FriendlyCell.friendlyCells[cell].transform;
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
        Vector2.Distance(transform.position, player.transform.position);

        if(distanceToPlayer < 4 && MainCharacter.isExhausted)
        {
            target = player.transform;
        }
    }
        

}
