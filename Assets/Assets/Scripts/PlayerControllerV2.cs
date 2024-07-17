using System.Collections;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private Cooldown cooldown;
    private CameraMovement cameraMovement => FindObjectOfType<CameraMovement>();
    public Rigidbody rb => GetComponent<Rigidbody>();

    private Vector3 Movement;
    [SerializeField] private float NormalSpeed = 15f;
    [SerializeField] private float SprintSpeed = 20f;
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
        rb.freezeRotation = true;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        rightclick = Input.GetMouseButtonDown(1);

        Move();
        RotatePlayerTowardsMouse();

        if ((horizontal != 0 || vertical != 0) && rightclick && !isDashing && !cooldown.IsCoolingDown)
        {
            isDashing = true;
            StartCoroutine(PerformDash());
        }
    }

    void FixedUpdate()
    {
        // Any fixed update logic you might add
    }

    #endregion

    #region PLAYER_ACTIONS

    void Move()
    {
        Movement = new Vector3(horizontal, 0f, vertical).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : NormalSpeed;
        transform.position += Movement * speed * Time.deltaTime;
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
