using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collidable : MonoBehaviour {

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

    float size;
    public float Size {
        get {
            return size;
        }
        set {
            size = value;
        }
    }

    float mass;
    public float Mass {
        get {
            return mass;
        }
        set {
            mass = value;
        }
    }

    bool shouldDestroy;
    public bool ShouldDestroy {
        get {
            return shouldDestroy;
        }
        set {
            shouldDestroy = value;
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

    public float DistanceFrom(Collidable other) {
        return Mathf.Sqrt(Mathf.Pow(other.XPosition - this.XPosition, 2) + Mathf.Pow(other.YPosition - this.YPosition, 2));
    }

    public float DistanceFrom(float x, float y) {
        return Mathf.Sqrt(Mathf.Pow(x - this.XPosition, 2) + Mathf.Pow(y - this.YPosition, 2));        
    }

    public bool isCollidingWith(Collidable other, float sizeCoefficient) {
        float distance = DistanceFrom(other);
        float radii = (this.Size + other.Size) / 2f * sizeCoefficient;
        return distance < radii;
    }

    public void AdvancePosition(float delta) {
        xPosition += xVelocity * delta;
        yPosition += yVelocity * delta;
        Vector3 position = this.gameObject.transform.position;
        position.x = xPosition;
        position.y = yPosition;
        transform.position = position;
    }

    public void Destruct() {
        Destroy(gameObject);
    }

    void Start() {

    }
    void Update() { }
}
