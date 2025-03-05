using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform focus;

    void Update()
    {
        transform.position = focus.position;
        transform.rotation = focus.rotation;
    }
}
