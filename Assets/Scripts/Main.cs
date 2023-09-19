using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] public bool debug;

    [SerializeField] public Sprite asteroidSpriteMap;
    [SerializeField] public GameObject asteroidPrefab;
    [SerializeField] public GameObject playerShipPrefab;
    [SerializeField] public GameObject backgroundTilePrefab;
    [SerializeField] public GameObject bulletPrefab;

    // Asteroid field specs
    [SerializeField] int numberOfAsteroids;
    [SerializeField] float asteroidSpawnRadius;
    [SerializeField] float asteroidDestructionRadius;
    [SerializeField] public bool inertiaDamper;

    void InjectConstantValuesFromSerializedFields() {
        GameData.Instance.NumberOfAsteroids = numberOfAsteroids;
        GameData.Instance.AsteroidSpawnRadius = asteroidSpawnRadius;
        GameData.Instance.AsteroidDestructionRadius = asteroidDestructionRadius;
        GameData.Instance.InertiaDamper = inertiaDamper;
    }

    void Start() {
        InjectConstantValuesFromSerializedFields();
    }

    void Update() {
        InjectConstantValuesFromSerializedFields();
    }
}
