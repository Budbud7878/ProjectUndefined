using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheShattered : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    public float simpleAttack = 15f;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackCooldown;

    [SerializeField] private float rayLength;
    [SerializeField] private int numberOfRays = 5; // Number of rays in the spread
    [SerializeField] private float spreadAngle = 45f; // Spread angle in degrees

    private bool isInRange;
    public bool isHit;
    private bool canAttack = true;

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
        GameObject playerObject = GameObject.FindWithTag("Player"); // Assuming Player has "Player" tag
        pC = playerObject.GetComponent<PlayerController>();

        playerLayer = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Attack();
    }

    public void EnemyHealthLogic(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void WhatAttack(DamageTypes newDamageType)
    {
        damageTypes = newDamageType;
    }

    void Attack()
    {
        if (!canAttack) return;

        float halfAngle = spreadAngle / 2f;
        isInRange = false;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = Mathf.Lerp(-halfAngle, halfAngle, i / (float)(numberOfRays - 1));
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hitInfo, attackRange, playerLayer))
            {
                isInRange = true;
                PlayerController player = hitInfo.collider.GetComponent<PlayerController>();

                if (player != null && isInRange)
                {
                    if (player.isVulnerable && !player.isHit)
                    {
                        player.isHit = true;
                        player.WhatAttacked(EnemyTypes.theShattered);
                        StartCoroutine(AttackCooldown()); // Start cooldown
                    }
                }
            }
            Debug.DrawRay(transform.position, rayDirection * attackRange, Color.yellow);
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false; // Set flag to prevent attacking
        yield return new WaitForSeconds(attackCooldown); // Wait for cooldown duration
        canAttack = true; // Reset flag after cooldown
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
