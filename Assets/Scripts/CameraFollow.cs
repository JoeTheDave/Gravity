using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float smoothSpeed = 0.125f;
    Transform target;
    
    // void Start() { }
    // void Update() { }

    void LateUpdate() {
        if (target == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            target = player.transform;
        }
        if (target != null)
        {
            Vector3 desiredPosition = target.position + new Vector3(0, 0, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
