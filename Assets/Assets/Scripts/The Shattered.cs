using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheShattered : MonoBehaviour
{

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageTaken;
    [SerializeField] private float physicalDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackSpeed;
   
    [SerializeField] private float rayLength;
    [SerializeField] private int numberOfRays = 5; // Number of rays in the spread
    [SerializeField] private float spreadAngle = 45f; // Spread angle in degrees

    private bool isInRange;
    private bool isHit;

    private PlayerController pC;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        pC = playerObject.GetComponent<PlayerController>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHeathLogic(pC.physicalDamage);
    }

    public void EnemyHeathLogic(float DamageTaken)
    {
        if (isHit == true)
        {
            currentHealth -= DamageTaken;
            if (currentHealth < 0f)
            {
                Death();
            }
        }
    }


    void Attack()
    {
               if (isInRange == true)
               {
                    
               }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
