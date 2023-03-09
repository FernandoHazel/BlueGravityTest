using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class YellowProtein : MonoBehaviour, ICollectable
{
    public delegate void ActionCollect();
    public static event ActionCollect proteinCollected;
    [SerializeField] GameObject OnCollected_PS;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        GameObject ps = Instantiate(OnCollected_PS);
        ps.transform.position = transform.position;

        if(proteinCollected != null)
        proteinCollected();

        gameObject.SetActive(false);
    }
}
