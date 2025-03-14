using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInMinimap : MonoBehaviour
{
    Transform[] allTransforms;

    void Start()
    {
        allTransforms = GetComponentsInChildren<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CameraHolder")) 
        {
            foreach (Transform child in allTransforms)
            {
                child.gameObject.layer = 7;
            }
        }
    }
}
