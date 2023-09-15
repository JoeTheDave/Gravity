using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Collidable {

    SpriteRenderer spriteRenderer;
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

    public void UpdateMassRelativeToSize() {
        transform.localScale = new Vector3(Size, Size, 1f);
        ParticleSystem trail = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = trail.main;
        mainModule.startSize = (1 / Size) * 0.01f;
        Mass = (4f / 3f) * Mathf.PI * Mathf.Pow((Size * 100f), 3);
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

    public void ToggleParticleTrail(bool active) {
        ParticleSystem trail = GetComponent<ParticleSystem>();
        if (active) {
            trail.Play();
        } else {
            trail.Stop();
        }
    }

    void Awake() {
        elapsedTime = 0;
        Frame = 0;
        FramesPerSecond = 16;
        AnimationType = 0;
        Velocity = 0;
        Trajectory = 0;
        XVelocity = 0;
        YVelocity = 0;
        Size = 1;
        Rotation = 0;
    }

    void Start() {
        spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -10;
        spriteRenderer.sprite = GameData.Instance.AsteroidSpriteListA[frame];
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
                spriteRenderer.sprite = GameData.Instance.AsteroidSpriteListA[frame];
            } else {
                spriteRenderer.sprite = GameData.Instance.AsteroidSpriteListB[frame];
            }
        }
    }
}
