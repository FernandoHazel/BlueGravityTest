using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "mainCharacterData", menuName = "ScriptableObjects/MainCharacter_SO", order = 1)]
public class MainCharacter_SO : ScriptableObject
{
    [Header("Skills")]
    public float maxHealth;
    public float modifiedMaxHealth;
    public float force;
    public float modifiedForce;
    public float damage;
    public float modifiedDamage;
    public float attackRange;
    public float modifiedAttackRange;

    [Header("properties")]
    public bool Exhausted;
    public int defaultProteins;
    [HideInInspector] public int inGameProteins;
}



//The visible skills in the inspector are the default values
//The modified values are the result of upgrading the default value
//I use the defaults to return to these values if the player unequip an upgrade

