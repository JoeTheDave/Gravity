using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Collidable {

    // Reference to Main script
    Main main;

    // Particle related
    ParticleSystem engineExhaust;
    ParticleSystem velocityDamperEnergyField;
    float particleBuffer = 0f;

    // Navigation
    float rotationSpeed = 180f;
    float acceleration = 5f;

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

    void ApplyThrust(float delta) {
        float xThrust = Mathf.Cos(direction * Mathf.Deg2Rad) * acceleration * delta;
        float yThrust = Mathf.Sin(direction * Mathf.Deg2Rad) * acceleration * delta;
        XVelocity += xThrust;
        YVelocity += yThrust;

    }

    void DamperVelocity(float delta) {
        float vel = Velocity - acceleration * delta;
        if (vel < 0) {
            vel = 0;
        }
        Velocity = vel;
    }

    void ShootCannon() {
        Bullet bullet = Instantiate(main.bulletPrefab).GetComponent<Bullet>();
        bullet.Mass = 10000f;
        bullet.Trajectory = Direction + Random.Range(-5f, 5f);;
        bullet.Velocity = 50f;
        bullet.XPosition = XPosition;
        bullet.YPosition = YPosition;
        GameData.Instance.Bullets.Add(bullet);
    }

    void Awake() {
        Velocity = 0f;
        Trajectory = 0f;
        XVelocity = 0f;
        YVelocity = 0f;
    }

    void Start() {
        main = GameObject.Find("Game").GetComponent<Main>();
        engineExhaust = GameObject.Find("Thruster").GetComponent<ParticleSystem>();
        velocityDamperEnergyField = GameObject.Find("Damper").GetComponent<ParticleSystem>();
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
            particleBuffer += delta;
            if (particleBuffer > 0.05) {
                engineExhaust.Emit(10);
                particleBuffer -= 0.05f;
            }
            ApplyThrust(delta);
        }
        if (Input.GetKey(KeyCode.S)) {
            particleBuffer += delta;
            if (particleBuffer > 0.05) {
                velocityDamperEnergyField.Emit(50);
                particleBuffer -= 0.05f;
            }
            DamperVelocity(delta);
        }
        if (Input.GetKey(KeyCode.Space)) {
            ShootCannon();
        }
    }
}
