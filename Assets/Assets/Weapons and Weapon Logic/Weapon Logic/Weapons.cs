using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;


public abstract class Weapons : ScriptableObject
{

    public string weaponName;
    public float damage;
    public float attackSpeed;
    public float weaponRange;
    public int maxUpgradeSlots; // Maximum number of upgrade slots
    public List<Upgrade> upgrades = new List<Upgrade>(); // List to hold current upgrades
}

[CreateAssetMenu(fileName = "Worn Sword", menuName = "Weapon/Worn Sword")]
public class WornSword : Weapons
{

    private void OnEnable()
    {
        weaponName = "Worn Sword";
        damage = 10f;
        attackSpeed = 15f;
        maxUpgradeSlots = 2;
    }
}

[CreateAssetMenu(fileName = "War Forged Longsword", menuName = "Weapon/War Forged Longsword")]
public class WarForgedLongsword : Weapons
{
    private void OnEnable()
    {
        weaponName = "War Forged Longsword";
        damage = 25f;
        attackSpeed = 9f;
        maxUpgradeSlots = 3;
    }
}
