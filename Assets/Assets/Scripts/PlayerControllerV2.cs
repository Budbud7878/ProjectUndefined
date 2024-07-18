using System.Collections;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private float GRAVITY; 


    [SerializeField] private Cooldown cooldown;
    private CameraMovement cameraMovement => FindObjectOfType<CameraMovement>();
    public Rigidbody rb => GetComponent<Rigidbody>();
    private CapsuleCollider capsuleCollider => GetComponent<CapsuleCollider>();

    private Vector3 Movement;
    private Vector3 currentVelocity = Vector3.zero;
    [SerializeField] private float NormalSpeed = 50f;
    [SerializeField] private float SprintSpeed = 70f;
    [SerializeField] private float smoothTime = 0.1f; // Smoothing time for movement
    [SerializeField] private LayerMask groundLayer; // Layer mask for ground detection
    private float horizontal;
    private float vertical;
    private bool rightclick;

    private bool isDashing = false;
    public float dashDistance = 5f; // Example dash distance
    public float dashDuration = 0.5f; // Example dash duration

    #endregion

    #region MONOBEHAVIOUR_METHODS

    void Start()
    {
        Physics.gravity *= GRAVITY;

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Enable interpolation for smoother movement

        // Assign a physics material with high friction to the capsule collider
        PhysicMaterial highFrictionMaterial = new PhysicMaterial();
        highFrictionMaterial.dynamicFriction = 1f;
        highFrictionMaterial.staticFriction = 1f;
        highFrictionMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
        capsuleCollider.material = highFrictionMaterial;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        rightclick = Input.GetMouseButtonDown(1);

        RotatePlayerTowardsMouse();

        if ((horizontal != 0 || vertical != 0) && rightclick && !isDashing && !cooldown.IsCoolingDown)
        {
            isDashing = true;
            StartCoroutine(PerformDash());
        }
    }

    void FixedUpdate()
    {
        Move();
        PreventSliding();
    }

    #endregion

    #region PLAYER_ACTIONS

    void Move()
    {
        Movement = new Vector3(horizontal, 0f, vertical).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : NormalSpeed;

        // Perform raycast to check for potential collisions
        Vector3 targetPosition = rb.position + Movement * speed * Time.fixedDeltaTime;
        Ray ray = new Ray(transform.position, Movement);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, (targetPosition - transform.position).magnitude))
        {
            // Use Vector3.SmoothDamp for smoother movement
            Vector3 newPosition = Vector3.SmoothDamp(rb.position, targetPosition, ref currentVelocity, smoothTime);
            rb.MovePosition(newPosition);
        }
    }

    void PreventSliding()
    {
        if (IsGrounded() && rb.velocity.y < 0.1f)
        {
            // Calculate the slope's normal
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
            {
                Vector3 slopeNormal = hit.normal;
                Vector3 slopeDirection = Vector3.Cross(Vector3.Cross(slopeNormal, Vector3.up), slopeNormal).normalized;

                // Project the velocity onto the slope's plane and apply counter force
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(rb.velocity, slopeNormal);
                Vector3 counterForce = -projectedVelocity * rb.mass;

                rb.AddForce(counterForce, ForceMode.Acceleration);
            }
        }
    }

    bool IsGrounded()
    {
        float rayLength = 1.1f;
        return Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
    }

    void RotatePlayerTowardsMouse()
    {
        if (cameraMovement == null) return;

        Vector3 mousePosition = cameraMovement.WorldPosition;
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    #endregion

    #region DASH_METHOD

    IEnumerator PerformDash()
    {
        float dashSpeed = dashDistance / dashDuration;

        Vector3 dashDirection = Movement.normalized; // Dash direction based on current movement input
        Vector3 dashTarget = transform.position + dashDirection * dashDistance;

        rb.velocity = dashDirection * dashSpeed; // Apply initial dash velocity

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            yield return null;
        }

        rb.velocity = Vector3.zero; // Stop the player after the dash
        isDashing = false; // Reset after dash is complete
        cooldown.StartCoolDown(); // Start cooldown after dash
    }

    #endregion
}
