using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Reference to the player's transform
    public Transform player;

    // Speed at which the object follows the player
    public float followSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        // Check if the player reference is set
        if (player != null)
        {
            // Calculate the direction from the object to the player
            Vector3 direction = player.position - transform.position;
            direction.Normalize();

            // Calculate the movement vector
            Vector3 move = direction * followSpeed * Time.deltaTime;

            // Move the object towards the player
            transform.position += move;
        }
    }
}
