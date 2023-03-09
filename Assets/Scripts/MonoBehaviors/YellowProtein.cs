using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class YellowProtein : MonoBehaviour, ICollectable
{
    public delegate void ActionCollect();
    public static event ActionCollect proteinCollected;
    [SerializeField] GameObject OnCollected_PS;
    [SerializeField] AudioClip CollectedSound;
    private Renderer ren;
    private AudioSource AS;

    private void Awake() 
    {
        AS = GetComponent<AudioSource>();
        ren = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        AS.PlayOneShot(CollectedSound);

        GameObject ps = Instantiate(OnCollected_PS);
        ps.transform.position = transform.position;

        if(proteinCollected != null)
        proteinCollected();

        ren.enabled = false;

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1);
        //Hide the protein Item
        gameObject.SetActive(false);
    }
}
