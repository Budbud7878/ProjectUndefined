using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlamingDelight : MonoBehaviour
{
    [SerializeField] private UnityEvent onAttack;

    private float attackCooldown;
    private float currentCooldown;

    private bool canAttack;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentCooldown <= 0f)
                {
                    onAttack?.Invoke();
                    currentCooldown = attackCooldown;
                }
            }
        }
    }
}
