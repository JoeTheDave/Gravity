using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    ParticleSystem engineExhaust;
    float engineExhaustBuffer = 0f;
    float rotationSpeed = 360f;
    float acceleration = 10f;
    // float terminalVelocity = 6f;

    float xPosition;
    public float XPosition {
        get {
            return xPosition;
        }
        set {
            xPosition = value;
            Vector3 position = this.gameObject.transform.position;
            position.x = value;
            transform.position = position;
        }
    }

    float yPosition;
    public float YPosition {
        get {
            return yPosition;
        }
        set {
            yPosition = value;
            Vector3 position = this.gameObject.transform.position;
            position.y = value;
            transform.position = position;
        }
    }

    float velocity;
    public float Velocity {
        get {
            return velocity;
        }
        set {
            velocity = value;
            UpdateComponentVelocitiesFromPolarVector();
        }
    }

    float trajectory;
    public float Trajectory {
        get {
            return trajectory;
        }
        set {
            trajectory = value;
            UpdateComponentVelocitiesFromPolarVector();
        }
    }

    float xVelocity;
    public float XVelocity {
        get {
            return xVelocity;
        }
        set {
            xVelocity = value;
            UpdatePolarVectorFromComponentVelocities();
        }
    }

    float yVelocity;
    public float YVelocity {
        get {
            return yVelocity;
        }
        set {
            yVelocity = value;
            UpdatePolarVectorFromComponentVelocities();
        }
    }

    float mass = 10000f;
    public float Mass {
        get {
            return mass;
        }
    }

    float direction;
    public float Direction {
        get {
            return direction;
        }
        set {
            direction = value;
            transform.eulerAngles = new Vector3(0, 0, value);
        }
    }

    void UpdateComponentVelocitiesFromPolarVector() {
        xVelocity = Mathf.Cos(trajectory * Mathf.Deg2Rad) * velocity;
        yVelocity = Mathf.Sin(trajectory * Mathf.Deg2Rad) * velocity;
    }

    void UpdatePolarVectorFromComponentVelocities() {
        trajectory = Mathf.Atan2(YVelocity, XVelocity) * Mathf.Rad2Deg;
        velocity = Mathf.Sqrt(Mathf.Pow(xVelocity, 2) + Mathf.Pow(yVelocity, 2));
    }

    void ApplyThrust(float delta) {
        float xThrust = Mathf.Cos(direction * Mathf.Deg2Rad) * acceleration * delta;
        float yThrust = Mathf.Sin(direction * Mathf.Deg2Rad) * acceleration * delta;
        XVelocity += xThrust;
        YVelocity += yThrust;
        // TODO: account for terminal velocity
    }

    public void AdvancePosition(float delta) {
        xPosition += xVelocity * delta;
        yPosition += yVelocity * delta;
        Vector3 position = this.gameObject.transform.position;
        position.x = xPosition;
        position.y = yPosition;
        transform.position = position;
    }

    // Constructor
    public Ship()
    {
        velocity = 0f;
        trajectory = 0f;
        xVelocity = 0f;
        yVelocity = 0f;
    }

    void Start() {
        engineExhaust = GetComponent<ParticleSystem>();
    }

    void Update() {
        float delta = Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) {
            Direction += rotationSpeed * delta;
        }
        if (Input.GetKey(KeyCode.D)) {
            Direction -= rotationSpeed * delta;
        }
        if (Input.GetKey(KeyCode.W)) {
            engineExhaustBuffer += delta;
            if (engineExhaustBuffer > 0.05) {
                engineExhaust.Emit(10);
                engineExhaustBuffer -= 0.05f;
            }
            ApplyThrust(delta);
        }
        // if (Input.GetKey(KeyCode.S)) {
        //     engineExhaustBuffer += delta;
        //     if (engineExhaustBuffer > 0.05) {
        //         engineExhaust.Emit(10);
        //         engineExhaustBuffer -= 0.05f;
        //     }
        //     ApplyThrust(delta);
        // }
    }
}
