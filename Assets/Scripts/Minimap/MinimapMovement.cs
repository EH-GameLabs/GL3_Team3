using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapMovement : MonoBehaviour
{
    [Tooltip("Punto attorno a cui la camera ruota")]
    public Transform target;

    [Tooltip("Distanza iniziale dalla target")]
    public float distance = 10.0f;

    [Tooltip("Velocità di zoom")]
    public float zoomSpeed = 2.0f;

    [Tooltip("Velocità di rotazione")]
    public float rotationSpeed = 100.0f;

    [Tooltip("Distanza minima consentita")]
    public float minDistance = 2.0f;

    [Tooltip("Distanza massima consentita")]
    public float maxDistance = 100.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Nessun target assegnato per la CameraOrbit!");
            return;
        }

        // Inizializza gli angoli con la rotazione attuale della camera
        currentX = transform.eulerAngles.y;
        currentY = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotazione della camera con tasto destro del mouse
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        }

        // Zoom con la rotellina del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calcola la rotazione e la nuova posizione della camera
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
