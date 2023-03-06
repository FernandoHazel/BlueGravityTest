using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "mainCharacterData", menuName = "ScriptableObjects/MainCharacter_SO", order = 1)]
public class MainCharacter_SO : ScriptableObject
{
    public float maxHealt;
    public bool Exhausted;
    public float force;
    public float damage;
    public float attackRange;
}
