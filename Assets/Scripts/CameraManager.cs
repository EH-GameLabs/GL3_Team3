using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    private static CameraManager Instance;
    public static CameraManager instance
    {
        get { return Instance; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] Transform cameraHolder;
    [SerializeField] private Transform focus;
    [SerializeField] Camera frontCam;
    [SerializeField] Camera backCamera;

    void Update()
    {
        cameraHolder.position = focus.position;
        cameraHolder.rotation = focus.rotation;
    }

    public void SwitchCam() 
    {
        frontCam.gameObject.SetActive(!frontCam.gameObject.activeInHierarchy);
        backCamera.gameObject.SetActive(!frontCam.gameObject.activeInHierarchy);
    }
}
