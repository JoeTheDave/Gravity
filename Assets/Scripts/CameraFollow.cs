using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float smoothSpeed = 0.125f;
    Transform target;
    Ship playerShip;
    
    // void Start() { }
    // void Update() { }

    void LateUpdate() {
        Camera mainCamera = Camera.main;
        float delta = Time.deltaTime;
        if (target == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerShip = player.GetComponent<Ship>();
            target = player.transform;
        }
        if (target != null)
        {
            // Follow
            Vector3 desiredPosition = target.position + new Vector3(0, 0, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Zoom
            float targetZoom = Mathf.Clamp(8 + playerShip.Velocity * 0.5f, 8, 18);
            float zoomStep = (targetZoom - mainCamera.orthographicSize) * delta;
            mainCamera.orthographicSize = mainCamera.orthographicSize + zoomStep;
        }
    }
}
