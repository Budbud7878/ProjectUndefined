using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlintlockEquip : MonoBehaviour
{
    public GameObject playerGrip;   // Reference to the PlayerGrip GameObject
    public GameObject flintlockPrefab; // Reference to the Flintlock prefab

    void OnDestroy()
    {
        if (playerGrip != null && flintlockPrefab != null)
        {
            // Instantiate Flintlock object at the position of PlayerGrip
            Instantiate(flintlockPrefab, playerGrip.transform.position, playerGrip.transform.rotation);

            // Set the PlayerGrip as the parent of the instantiated Flintlock
            flintlockPrefab.transform.parent = playerGrip.transform;
        }
        else
        {
            Debug.LogWarning("PlayerGrip or FlintlockPrefab is not assigned!");
        }
    }
}
