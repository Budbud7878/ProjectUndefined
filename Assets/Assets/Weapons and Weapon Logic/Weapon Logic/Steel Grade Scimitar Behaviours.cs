using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelGradeScimitarBehaviours : MonoBehaviour
{

    public float steelGradeScimitarDmg;
    public float steelGradeScimitarRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SteelGradeScimitarAttack(Collider other)
    {
        Debug.Log("Weapon collided with: " + other.name); // Log the name of the collided object
        // Check if the collider has an Enemy component
        TheShattered enemy = other.GetComponent<TheShattered>();
        if (enemy != null)
        {
            Debug.Log("Collided with an enemy: " + other.name); // Log enemy detection
            // Apply damage to the enemy
            enemy.EnemyHealthLogic(steelGradeScimitarDmg);
        }
        else
        {
            Debug.Log("Collided object does not have an Enemy component.");
        }
    }

}
