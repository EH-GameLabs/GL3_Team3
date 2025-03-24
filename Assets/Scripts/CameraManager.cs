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
    [SerializeField] Camera minimapCamera;

    [SerializeField] private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        //cameraHolder.position = focus.position;
        //cameraHolder.rotation = focus.rotation;

        // Interpola la posizione della camera verso quella del focus
        cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position, focus.position, ref velocity, smoothTime);

        // Interpola la rotazione della camera verso quella del focus (opzionale)
        cameraHolder.rotation = Quaternion.Lerp(cameraHolder.rotation, focus.rotation, Time.deltaTime * 999);
    }

    public void SwitchCam()
    {
        frontCam.gameObject.SetActive(!frontCam.gameObject.activeInHierarchy);
        backCamera.gameObject.SetActive(!frontCam.gameObject.activeInHierarchy);
    }

    public void SwitchMinimapCam() 
    {
        cameraHolder.gameObject.SetActive(!cameraHolder.gameObject.activeInHierarchy);
        minimapCamera.gameObject.SetActive(!minimapCamera.gameObject.activeInHierarchy);
    }
}
