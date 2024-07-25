using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttacker : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;
    public float maintainDistance = 3f;

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned.");
            return;
        }

        // Calculate the direction and distance to the player
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Normalize direction vector
        Vector3 direction = directionToPlayer.normalized;

        if (distanceToPlayer > maintainDistance)
        {
            // Move towards the player if we are further away than maintainDistance
            Vector3 moveDirection = direction * followSpeed * Time.deltaTime;
            transform.position += moveDirection;
        }
        else if (distanceToPlayer < maintainDistance)
        {
            // Move away from the player if we are closer than maintainDistance
            Vector3 moveDirection = -direction * followSpeed * Time.deltaTime;
            transform.position += moveDirection;
        }
    }
}
