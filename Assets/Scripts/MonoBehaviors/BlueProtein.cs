using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class BlueProtein : MonoBehaviour
{
    private HealthBar healthBar;
    private Renderer ren;
    [SerializeField] MainCharacter_SO mainCharacter_SO;
    [SerializeField] GameObject OnCollected_PS;
    [SerializeField] AudioClip CollectedSound;
    private AudioSource AS;
    private void Awake() 
    {
        AS = GetComponent<AudioSource>();
        healthBar = GameObject.FindObjectOfType<HealthBar>();
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

        //Add the health to the player but avoid to exceed
        MainCharacter.healt += mainCharacter_SO.BlueProteinValue;
        if(MainCharacter.healt > mainCharacter_SO.modifiedMaxHealth)
        {
            MainCharacter.healt = mainCharacter_SO.modifiedMaxHealth;
        }

        //Update the bar
        float percent = MainCharacter.healt/mainCharacter_SO.modifiedMaxHealth;
        healthBar.On_damaged(percent);

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
    
