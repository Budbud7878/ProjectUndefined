using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceLogic : MonoBehaviour
{

    [SerializeField] private float currentEssence;
    private float maxEssence = 50f;
    private float refillRate = 1f; // This is just for testing right now. The essence refill rate will be affected by various upgrades and items. Same for the cooldown.
    private float fillCooldown = 5f;

    private bool canFill;

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

        if(Input.GetKeyDown(KeyCode.L)) 
        {
            currentEssence -= 20f;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            pC.isHit = true;
        }
    }

    void EssenceRefill()
    {

        if(!pC.isHit) 
        {
            StartCoroutine(Refill());
        }

        if (pC.isHit == true)
        {
            StartCoroutine(RefillCooldown());
            StopCoroutine(Refill());
        }
        if (currentEssence == maxEssence)
        {
            StopCoroutine(Refill());
        }
    }
    

    IEnumerator RefillCooldown()
    {
        yield return new WaitForSeconds(fillCooldown); // Wait for cooldown duration
    }
    IEnumerator Refill()
    {
       
        while (currentEssence < maxEssence & !pC.isHit)
        {
            currentEssence += refillRate * Time.deltaTime;
            yield return new WaitForSeconds(7f);
        }

        if (currentEssence > maxEssence)
        {
            currentEssence = maxEssence;
        }
    }

    /*IEnumerator RefillDelay()
    {
        if()
    }*/
}
