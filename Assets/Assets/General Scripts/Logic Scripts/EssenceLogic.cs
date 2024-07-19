using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceLogic : MonoBehaviour
{

    public float currentEssence;
    private float maxEssence = 50f;
    private float refillRate = 15f; // This is just for testing right now. The essence refill rate will be affected by various upgrades and items. Same for the cooldown.
    private float startRefillDelay = 10f;

    [SerializeField] private bool canFill = true;
    [SerializeField] private bool canBegin = false;

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

        if (Input.GetKeyUp(KeyCode.L))
        {
            currentEssence -= 20f;
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
            StopCoroutine(Refill());
        }
        if (currentEssence == maxEssence)
        {
            StopCoroutine(Refill());
        }
        if (currentEssence >= maxEssence)
        {
            currentEssence = maxEssence;
            canBegin = false;
        }
        if (currentEssence < 0f)
        {
            currentEssence = 0f;
        }
    }

    IEnumerator DuringRefillDelay()
    {
        yield return new WaitForSeconds(5f);
        canFill = true;
    }

    IEnumerator RefillStartDelay()
    {
        yield return new WaitForSeconds(startRefillDelay);
        canBegin = true;
    }

    IEnumerator Refill()
    {
        while (currentEssence < maxEssence && !pC.isHit && canFill)
        {
            StartCoroutine(RefillStartDelay());

            if (canBegin)
            {
                currentEssence += refillRate;

                canFill = false;
                StartCoroutine(DuringRefillDelay());
            }

            if (currentEssence > maxEssence)
            {
                currentEssence = maxEssence;
                break;
            }
            yield return null;
            Debug.Log("After return");
        }
        
    }
}
