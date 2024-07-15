using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerControllerV2 : MonoBehaviour
{
    #region VARIABLES
    private CameraMovement cameraMovement;

    public Rigidbody rb;

    private Vector3 Movement;
    [SerializeField] private float NormalSpeed = 4f;
    [SerializeField] private float SprintSpeed = 5.5f;
    #endregion

    void Start()
    {
        // Find the CameraMovement script
        cameraMovement = FindObjectOfType<CameraMovement>();

        if (cameraMovement == null)
        {
            Debug.LogError("CameraMovement script not found. Ensure it is attached to a GameObject in the scene.");
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Movement = new Vector3(horizontal, 0f, vertical).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : NormalSpeed;

        transform.position += Movement * speed * Time.deltaTime;

        // Rotate the player to face the mouse position
        RotatePlayerTowardsMouse();
    }

    void RotatePlayerTowardsMouse()
    {
        if (cameraMovement == null) return;

        // Get the position of the mouse in the world
        Vector3 mousePosition = cameraMovement.WorldPosition;

        // Calculate the direction from the player to the mouse position
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.y = 0; // Keep the rotation in the horizontal plane

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the player towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}
