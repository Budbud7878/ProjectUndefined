using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Worn Sword", menuName = "Weapon/Worn Sword")]
public class WornSword : Weapon
{
    
    private void OnEnable()
    {
        weaponName = "Worn Sword";
        damage = 10f;
        attackSpeed = 15f;
        maxUpgradeSlots = 2;
    }
}
