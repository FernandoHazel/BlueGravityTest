using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class YellowProtein : MonoBehaviour, ICollectable
{
    public delegate void ActionCollect();
    public static event ActionCollect proteinCollected;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if(proteinCollected != null)
        proteinCollected();

        gameObject.SetActive(false);
    }
}
