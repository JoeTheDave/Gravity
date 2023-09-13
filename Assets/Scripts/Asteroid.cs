using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, ICollidable {

    SpriteRenderer spriteRenderer;
    List<Sprite> spriteListA;
    List<Sprite> spriteListB;
    float elapsedTime;
    
    int frame;
    public int Frame {
        get {
            return frame;
        }
        set {
            frame = value;
        }
    }

    int framesPerSecond;
    public int FramesPerSecond {
        get {
            return framesPerSecond;
        }
        set {
            framesPerSecond = value;
        }
    }

    int animationType;
    public int AnimationType {
        get {
            return animationType;
        }
        set {
            animationType = value;
        }
    }

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
            transform.localScale = new Vector3(size, size, 1f);
            ParticleSystem trail = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = trail.main;
            mainModule.startSize = (1 / size) * 0.01f;
            mass = (4f / 3f) * Mathf.PI * Mathf.Pow((size * 100f), 3);
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

    float rotation;
    public float Rotation {
        get {
            return rotation;
        }
        set {
            rotation = value;
            transform.localEulerAngles = new Vector3(1f, 1f, rotation);
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

    public void AdvancePosition(float delta) {
        xPosition += xVelocity * delta;
        yPosition += yVelocity * delta;
        Vector3 position = this.gameObject.transform.position;
        position.x = xPosition;
        position.y = yPosition;
        transform.position = position;
    }

    public void ToggleParticleTrail(bool active) {
        ParticleSystem trail = GetComponent<ParticleSystem>();
        if (active) {
            trail.Play();
        } else {
            trail.Stop();
        }
    }

    public void Destruct() {
        Destroy(gameObject);
    }

    public float DistanceFrom(ICollidable other) {
        return Mathf.Sqrt(Mathf.Pow(other.XPosition - this.XPosition, 2) + Mathf.Pow(other.YPosition - this.YPosition, 2));
    }

    public float DistanceFrom(float x, float y) {
        return Mathf.Sqrt(Mathf.Pow(x - this.XPosition, 2) + Mathf.Pow(y - this.YPosition, 2));        
    }

    public bool isCollidingWith(ICollidable other, float sizeCoefficient) {
        float distance = DistanceFrom(other);
        float radii = (this.Size + other.Size) / 2f * sizeCoefficient;
        return distance < radii;
    }

    // Constructor
    public Asteroid()
    {
        elapsedTime = 0;
        frame = 0;
        framesPerSecond = 16;
        animationType = 0;
        velocity = 0;
        trajectory = 0;
        xVelocity = 0;
        yVelocity = 0;
        size = 1;
        rotation = 0;
    }

    void Start() {
        spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -10;
        Main script = GameObject.Find("Game").GetComponent<Main>();
        spriteListA = script.asteroidSpriteListA;
        spriteListB = script.asteroidSpriteListB;
        spriteRenderer.sprite = spriteListA[frame];
    }
    
    void Update() {
        float frameDuration = 1.0f / (float)framesPerSecond;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= frameDuration) {
            elapsedTime -= frameDuration;
            frame++;
            if (frame > 30) {
                frame = 0;
            }
            if (animationType == 0) {
                spriteRenderer.sprite = spriteListA[frame];
            } else {
                spriteRenderer.sprite = spriteListB[frame];
            }
        }
    }

    
}
