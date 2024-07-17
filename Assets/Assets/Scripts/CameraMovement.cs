using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    #region VARIABLES
    public Vector3 ScreenPos;
    public Vector3 WorldPosition;
    public LayerMask LayerToHit;
    public GameObject Mouse;


    public Transform Player; // Reference to the player


    public float MaxDistance = 10f; // Maximum allowed distance from the player
    private Camera activeCamera;
    #endregion

    void Start()
    {
        // Get the active camera from CinemachineBrain
        CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        if (cinemachineBrain != null)
        {
            activeCamera = cinemachineBrain.OutputCamera;
        }
    }

    void Update()
    {
        if (Mouse == null || activeCamera == null || Player == null) return;

        ScreenPos = Input.mousePosition;

        Ray ray = activeCamera.ScreenPointToRay(ScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hitdata, 100, LayerToHit))
        {
            WorldPosition = hitdata.point;

            // Calculate distance from player to target position
            float distance = Vector3.Distance(Player.position, WorldPosition);

            // If the distance is greater than the maximum allowed distance, clamp the position
            if (distance > MaxDistance)
            {
                // Calculate the clamped position
                Vector3 direction = (WorldPosition - Player.position).normalized;
                WorldPosition = Player.position + direction * MaxDistance;
            }

            Mouse.transform.position = WorldPosition;
        }
    }
}
