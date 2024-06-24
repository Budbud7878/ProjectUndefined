using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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

    public float physicalDamage = 10f; //For now lets say this is how much damage hand to hand combat does.
    public float pD2 = 20f;
    [SerializeField] private float damageTaken; //Variable might need to be moved.
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackSpeed; //Useless until we implement animations.
    [SerializeField] private float specialCd; //Useless also.
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float invulnerabilityTimer;


    private bool isInRange;
    public bool isHit;
    public bool isVulnerable = true;

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
    private EnemyTypes enemyType;
    private TheShattered theShattered;
    private float lastDashTime;
    #endregion

    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Player");

        enemyLayer = LayerMask.GetMask("Enemy");

        rb = GetComponent<Rigidbody>();

        GameObject shattered = GameObject.FindWithTag("Shattered");
        theShattered = shattered.GetComponent<TheShattered>();

        currentHealth = maxHealth;
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

        Attack();

        PlayerHealthLogic(); 

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastDashTime > dashCooldown)
        {
            // Perform dash
            Dash();
        }


        if (Input.GetKeyDown(KeyCode.B))
        {
            isVulnerable = false;
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

    void Attack()
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
                TheShattered shattered = hitInfo.collider.GetComponent<TheShattered>();

                if (isInRange == true)
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        shattered.isHit = true;
                        shattered.WhatAttack(DamageTypes.punchDamage);
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        shattered.isHit = true;
                        shattered.WhatAttack(DamageTypes.slashDamage);
                    }
                    Debug.Log("Player has inflicted " + physicalDamage + " on " + hitInfo.collider.name);

                }
            }

            // Visualize the attack ray in the Scene view
            Debug.DrawRay(attackRangeOrigin, rayDirection * attackRange, Color.yellow);
        }
    }

    public void PlayerHealthLogic() //Game manager should alter what damage was taken depending on enemy.
    {

        switch (enemyType)
        {
            case EnemyTypes.theShattered:

                if (isHit == true && isVulnerable == true)
                {
                    currentHealth -= theShattered.simpleAttack;
                    if (currentHealth <= 0f)
                    {
                        Death();
                    }
                }

                break;

            default:

                Debug.Log("Unknown damage type");

                break;
        }
    }

    public void WhatAttacked(EnemyTypes newEnemy)
    {
        enemyType = newEnemy;
        Debug.Log("Enemy type is " + enemyType);
    }

    void Death()
    {
        Destroy(gameObject);  //It'll only destroy the player game object for now, to be changed later.
    }

}
