using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "War Forged Longsword", menuName = "Weapon/War Forged Longsword")]
public class WarForgedLongsword : Weapon
{
    private void OnEnable()
    {
        weaponName = "War Forged Longsword";
        damage = 25f;
        attackSpeed = 9f;
        maxUpgradeSlots = 3;
    }
}
