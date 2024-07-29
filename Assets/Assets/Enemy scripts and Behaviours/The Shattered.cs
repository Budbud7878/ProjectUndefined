using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TheShattered : MonoBehaviour
{

    [SerializeField] private UnityEvent onAttack;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float currentCooldown;

    public bool isHit;
    public bool canAttack = false;

    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log("Awake - Current Health: " + currentHealth);
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player"); // Assuming Player has "Player" tag

        playerLayer = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canAttack)
        {
            onAttack?.Invoke();
        }
    }

    public void EnemyHealthLogic(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
