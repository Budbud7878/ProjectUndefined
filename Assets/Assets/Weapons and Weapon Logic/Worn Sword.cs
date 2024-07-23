using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WornSword : MonoBehaviour
{

    public UnityEvent onAttack;
    public float attackCooldown;

    public float currentCooldown;

    public bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = attackCooldown;
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
                    onAttack.Invoke();
                    currentCooldown = attackCooldown;
                }
            }
        }
         

    }
}
