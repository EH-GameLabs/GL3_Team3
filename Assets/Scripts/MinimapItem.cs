using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapItem : MonoBehaviour
{
    private GameObject minimapCamera;

    private void Start()
    {
        minimapCamera = FindAnyObjectByType<MinimapMovement>(FindObjectsInactive.Include).gameObject;
    }

    private void Update()
    {
        transform.LookAt(minimapCamera.transform.position);
    }
}
