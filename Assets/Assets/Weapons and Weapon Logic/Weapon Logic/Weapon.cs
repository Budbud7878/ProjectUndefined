using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;


public abstract class Weapon : ScriptableObject
{

    public string weaponName;
    public float damage;
    public float attackSpeed;
    public float weaponRange;
    public int maxUpgradeSlots; // Maximum number of upgrade slots
    public List<Upgrade> upgrades = new List<Upgrade>(); // List to hold current upgrades


}
