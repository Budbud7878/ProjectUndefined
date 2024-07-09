using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceLogic : MonoBehaviour
{

    [SerializeField] private float currentEssence;
    private float maxEssence = 50f;
    private float refillRate = 15f; // This is just for testing right now. The essence refill rate will be affected by various upgrades and items. Same for the cooldown.
    private float fillCooldown = 5f;

    [SerializeField] private bool canFill = true;

    private PlayerController pC;

    // Start is called before the first frame update
    void Start()
    {
        currentEssence = maxEssence;
        pC = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        EssenceRefill();
    }

    void EssenceRefill()
    {

        if(!pC.isHit) 
        {
            StartCoroutine(Refill());
        }

        if (pC.isHit == true)
        {
            StopCoroutine(Refill());
        }
        if (currentEssence == maxEssence)
        {
            StopCoroutine(Refill());
        }
    }

    IEnumerator RefillDelay()
    {
        yield return new WaitForSeconds(5f);
        canFill = true;
    }

    IEnumerator Refill()
    {
        while (currentEssence < maxEssence && !pC.isHit && canFill)
        {
            currentEssence += refillRate;

            canFill = false;
            StartCoroutine(RefillDelay());

            if (currentEssence >= maxEssence)
            {
                currentEssence = maxEssence;
                break; // Exit the loop if maxEssence is reached
            }
            yield return null;
            Debug.Log("After return");
        }
        
    }
}
