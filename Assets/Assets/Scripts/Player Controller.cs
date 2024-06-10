using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Camera playerCam;

    public GameObject player;

    public float moveSpeed;
    public float sprintSpeed;
    [SerializeField] private float rayLength;
    [SerializeField] private float attackRange;

    private Plane groundPlane;

    private Ray camRay;
    private Ray attackRay;

    private Vector3 pointToLook;
    private Vector3 rangeOfAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Player");
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

        rangeOfAttack = transform.forward * attackRange;    /* Could use help with this one Boris, or Eder if you can, basically I was thinking of using this little calculation
                                                            to create raycast that moves from the players Z axis till it reaches a set range but im stumped, I need this ray because 
                                                              it will be used to detect whether an enemy is in range to get hit by an attack or not so yeah */

        Debug.DrawRay(camRay.origin, camRay.direction * rayLength, Color.red);  //Creates a visual Representation of the raycast.

        if (groundPlane.Raycast(camRay, out rayLength))     //Forbids the ray from going beyond the set ground plane.
        {
            pointToLook = camRay.GetPoint(rayLength);
        }

        transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));  //Makes player game object rotate in the direction of the raycast.
    }
}
