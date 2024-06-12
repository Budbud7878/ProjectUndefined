using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public Camera playerCam;

    public GameObject player;

    public float moveSpeed;
    public float sprintSpeed;

    public LayerMask enemyLayer;

    [SerializeField] private float rayLength;
    [SerializeField] private float attackRange;

    private bool isInRange;

    private Plane groundPlane;

    private Ray camRay;

    private Vector3 pointToLook;
    private Vector3 attackRangeOrigin;
    private Vector3 directionOfAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Player");

        enemyLayer = LayerMask.GetMask("Enemy");
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

        if (Physics.Raycast(attackRangeOrigin, directionOfAttack, out RaycastHit hitInfo, attackRange, enemyLayer))
        {
            // If the ray hits an object, you can access the hit information via hitInfo
            Debug.Log("Raycast hit: " + hitInfo.collider.name);

            isInRange = true;

            if(isInRange == true & Input.GetMouseButtonDown(0))
            {
                Debug.Log("Enemy has been attacked");
            }
        }

        // Update attack range origin and direction of attack each frame
        attackRangeOrigin = transform.position;
        directionOfAttack = transform.forward;

        // Visualize the ray in the Scene view
        Debug.DrawRay(attackRangeOrigin, directionOfAttack * attackRange, Color.yellow);
    }
}
