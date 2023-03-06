using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BacteriaData", menuName = "ScriptableObjects/Bacteria_SO", order = 3)]
public class Bacteria_SO : ScriptableObject
{
    public float maxHealt;
    public float force;
    public float damage;
    public float attackRange;
    [Tooltip("Time in seconds to start looking for another target")]
    public float ReselectTargetTime;
}


