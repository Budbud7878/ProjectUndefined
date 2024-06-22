using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheShattered : MonoBehaviour
{

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    public float simpleAttack = 15f;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackSpeed;
   
    [SerializeField] private float rayLength;
    [SerializeField] private int numberOfRays = 5; // Number of rays in the spread
    [SerializeField] private float spreadAngle = 45f; // Spread angle in degrees

    private bool isInRange;
    public bool isHit;

    public LayerMask playerLayer;

    private PlayerController pC;
    private DamageTypes damageTypes;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log("Awake - Current Health: " + currentHealth);
    }
    void Start()
    {
        Debug.Log("Start - Current Health: " + currentHealth);
        GameObject playerObject = GameObject.Find("Player");
        pC = playerObject.GetComponent<PlayerController>();

        playerLayer = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHealthLogic();
        Attack();
    }
    public void EnemyHealthLogic()
    {
        Debug.Log("Current Health: " + currentHealth + ", Max Health: " + maxHealth);
        Debug.Log("Is Hit: " + isHit + ", Is In Range: " + isInRange + ", Damage Type: " + damageTypes);

        switch (damageTypes)
        {
            case DamageTypes.punchDamage:

                if (isHit == true)
                {
                    currentHealth -= pC.physicalDamage;
                    if (currentHealth <= 0f)
                    {
                        Death();
                    }
                }

                break;

            case DamageTypes.slashDamage:

                if (isHit == true)
                {
                    currentHealth -= pC.pD2;
                    if (currentHealth <= 0f)
                    {
                        Death();
                    }
                }

                break;

            default:

                Debug.Log("Unknown damage type");
                
                break;
        }
    }

    public void WhatAttack(DamageTypes newDamageType)
    {
        damageTypes = newDamageType;
    }


    void Attack()
    {
        float halfAngle = spreadAngle / 2f;
        isInRange = false;

        for (int i = 0; i < numberOfRays; i++)
        {
            // Calculate the angle for this ray
            float angle = Mathf.Lerp(-halfAngle, halfAngle, i / (float)(numberOfRays - 1));
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;

            // Perform the raycast
            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hitInfo, attackRange, playerLayer))
            {
                    isInRange = true;
                    PlayerController player = hitInfo.collider.GetComponent<PlayerController>();

                if (player != null && isInRange)
                {
                    player.isHit = true;

                    if (isHit)
                    {
                        player.WhatAttacked(EnemyTypes.theShattered);
                        
                    }
                }
            }  

            // Visualize the ray in the Scene view
            Debug.DrawRay(transform.position, rayDirection * attackRange, Color.yellow);
        }
    }


    void Death()
    {
        Destroy(gameObject);
    }
}
