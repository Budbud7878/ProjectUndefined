using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheShatteredBehaviours : MonoBehaviour
{

    private float simpleAttack = 15f;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float currentCooldown;


    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Collider other)
    {
        Debug.Log("Collided with: " + other.name); // Log the name of the collided object

        // Check if the collider has a PlayerControllerV2 component
        PlayerControllerV2 player = other.GetComponent<PlayerControllerV2>();
        if (player != null)
        {
            Debug.Log("Collided with the player: " + other.name); // Log player detection
            // Apply damage to the player
            player.Health(simpleAttack);

        }
        else
        {
            Debug.Log("Collided object does not have a Player component.");
        }
    }
}
