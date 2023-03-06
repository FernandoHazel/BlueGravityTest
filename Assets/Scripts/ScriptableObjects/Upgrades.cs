using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "ScriptableObjects/Upgrades_SO", order = 4)]
public class Upgrades : ScriptableObject
{
    public Upgrade[] upgrades;
}

[System.Serializable]
public class Upgrade
{
    [Tooltip("The sprite of the shop")]
    public Sprite sprite;
    public float cost;
    [Tooltip("The Object we are going to add to the look")]
    public GameObject accessory;
    [Tooltip("Which skill we are going to improve")]
    public string skillName;
    [Tooltip("Which is the name of the upgrade")]
    public string UpgradeName;
    [Tooltip("How much we are going to improve")]
    public float Upercent;
    [Tooltip("What is the seller going to say when we select the upgrade")]
    [TextArea]
    public string DCDialogue;
    [Tooltip("Is this upgrade already sold?")]
    public bool sold;
}