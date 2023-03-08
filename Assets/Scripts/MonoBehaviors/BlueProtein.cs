using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlueProtein : MonoBehaviour
{
    private HealthBar healthBar;
    [SerializeField] MainCharacter_SO mainCharacter_SO;
    private void Start() 
    {
        healthBar = GameObject.FindObjectOfType<HealthBar>();
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
        //Add the health to the player but avoid to exceed
        MainCharacter.healt += mainCharacter_SO.BlueProteinValue;
        if(MainCharacter.healt > mainCharacter_SO.modifiedMaxHealth)
        {
            MainCharacter.healt = mainCharacter_SO.modifiedMaxHealth;
        }

        //Update the bar
        float percent = MainCharacter.healt/mainCharacter_SO.modifiedMaxHealth;
        healthBar.On_damaged(percent);

        //Hide the protein Item
        gameObject.SetActive(false);
    }
}
    
