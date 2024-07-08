using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceLogic : MonoBehaviour
{

    private float currentEssence;
    private float maxEssence = 50f;
    private float refillRate = 1f; // This is just for testing right now. The essence refill rate will be affected by various upgrades and items. Same for the cooldown.
    private float fillCooldown = 5f;

    private bool canFill;

    // Start is called before the first frame update
    void Start()
    {
        currentEssence = maxEssence;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EssenceRefill()
    {
        if (currentEssence < maxEssence)
        {
            
        }
    }
    

    IEnumerator RefillCooldown()
    {
        canFill = false;
        yield return new WaitForSeconds(fillCooldown); // Wait for cooldown duration
        canFill = true;
    }
    IEnumerator RefillRate()
    {
        canFill = true;

        while (currentEssence < maxEssence)
        {
            currentEssence += refillRate * Time.deltaTime;
            yield return null;
        }

        canFill = false;
    }
}
