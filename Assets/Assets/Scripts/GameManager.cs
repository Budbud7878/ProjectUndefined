using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera MainCamera;
    void Start()
    {
        // Ignore this debug.logs() adding them for structure... sort off. (I like to type stuff it let's my mind work faster so I typed this as warm up.)
        Debug.Log("GameManager loading...");


        Debug.Log("GameManager's has finished all initial tasks.");
    }

    void Update()
    {
        
    }
}
