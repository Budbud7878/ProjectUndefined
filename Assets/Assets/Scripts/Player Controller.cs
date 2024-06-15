using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public Camera playerCam;
    public GameObject player;

    public float moveSpeed;
    public float sprintSpeed;

    public LayerMask enemyLayer;

    [SerializeField] private float rayLength;
    [SerializeField] private float attackRange;
    [SerializeField] private int numberOfRays = 5; // Number of rays in the spread
    [SerializeField] private float spreadAngle = 45f; // Spread angle in degrees

    [SerializeField] private float physicalDamage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackSpeed; //Useless until we implement animations.
    [SerializeField] private float specialCd; //Useless also.
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    private bool isInRange;
    private bool isHit;

    private Plane groundPlane;

    private Ray camRay;

    private Vector3 pointToLook;
    private Vector3 attackRangeOrigin;
    private Vector3 directionOfAttack;

    public float dashDistance = 5f; // Distance to dash
    public float dashTime = 0.2f; // Duration of dash
    public float dashCooldown = 1f; // Cooldown between dashes

    private Rigidbody rb;
    private bool isDashing = false;
    private Vector3 dashDirection;
    private float lastDashTime;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Player");

        enemyLayer = LayerMask.GetMask("Enemy");

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Assuming this is 3rd-person movement and the default Input Manager configuration is used.
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        //Use the value of "sprintSpeed" if left-shift is held down, otherwise use the value of "moveSpeed";
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        //Update the GameObject's position with the detected move direction and speed.
        transform.position += moveDirection * speed * Time.deltaTime;

        groundPlane = new Plane(Vector3.up, Vector3.zero);      //Sets the ground plane

        camRay = playerCam.ScreenPointToRay(Input.mousePosition);   //Makes the ray point in the direction of the mouse.

        Debug.DrawRay(camRay.origin, camRay.direction * rayLength, Color.red);  //Creates a visual Representation of the raycast.

        if (groundPlane.Raycast(camRay, out rayLength))     //Forbids the ray from going beyond the set ground plane.
        {
            pointToLook = camRay.GetPoint(rayLength);
        }

        transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));  //Makes player game object rotate in the direction of the raycast.

        // Update attack range origin and direction of attack each frame
        attackRangeOrigin = transform.position;
        directionOfAttack = transform.forward;


        AttackRangeDetection();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastDashTime > dashCooldown)
        {
            // Perform dash
            Dash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            // Move the player along the dash direction
            rb.MovePosition(transform.position + dashDirection * dashDistance * Time.fixedDeltaTime / dashTime);
        }
    }

    void Dash()
    {
        // Set dash direction based on player input or current facing direction
        dashDirection = transform.forward; // For simplicity, using forward direction as dash direction

        // Start dash
        isDashing = true;
        lastDashTime = Time.time;

        // Invoke method to stop dash after dash time
        Invoke("StopDash", dashTime);
    }

    void StopDash()
    {
        // End dash
        isDashing = false;
    }

    void AttackRangeDetection()
    {
        float halfAngle = spreadAngle / 2f;
        for (int i = 0; i < numberOfRays; i++)
        {
            // Calculate the angle for this ray
            float angle = Mathf.Lerp(-halfAngle, halfAngle, i / (float)(numberOfRays - 1));
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * directionOfAttack;

            // Perform the raycast
            if (Physics.Raycast(attackRangeOrigin, rayDirection, out RaycastHit hitInfo, attackRange, enemyLayer))
            {     
                // If the ray hits an object, you can access the hit information via hitInfo
                Debug.Log("Raycast hit: " + hitInfo.collider.name);

                isInRange = true;

                if (isInRange == true & Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Enemy has been attacked");
                }
            }

            // Visualize the attack ray in the Scene view
            Debug.DrawRay(attackRangeOrigin, rayDirection * attackRange, Color.yellow);
        }
    }

    void PlayerHealth(float damageTaken)
    {
        if (isHit == true)
        {
            currentHealth -= damageTaken;
        }
    }

}
