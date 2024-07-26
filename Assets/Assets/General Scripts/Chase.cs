using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float distance = 3f;

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned.");
            return;
        }

        // Calculate the direction to the player
        Vector3 direction = player.position - transform.position;

        // Keep the object at a certain distance from the player
        if (direction.magnitude > distance)
        {
            // Calculate the movement vector
            Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;

            // Move the object towards the player
            transform.position += moveDirection;
        }
    }
}
