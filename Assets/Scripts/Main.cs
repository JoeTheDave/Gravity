using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] bool debug;

    [SerializeField] Sprite asteroidSpriteMap;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject playerShipPrefab;
    [SerializeField] GameObject backgroundTilePrefab;
    

    // Asteroid field specs
    [SerializeField] int numberOfAsteroids;
    [SerializeField] float asteroidSpawnRadius;
    [SerializeField] float asteroidDestructionRadius;

    // Asteroid sprite animation
    public List<Sprite> asteroidSpriteListA;
    public List<Sprite> asteroidSpriteListB;
    Texture2D asteroidTexture;

    // Game Objects
    List<GameObject> backgroundTiles;
    List<Asteroid> asteroids;
    Ship playerShip;

    Sprite GenerateAsteroidSpriteFrame(int x, int y) {
        Rect rect = new Rect(x * 128, y * 128, 128, 128);
        Sprite sprite = Sprite.Create(asteroidTexture, rect, Vector2.one * 0.5f);
        return sprite;
    }

    void GenerateSprites() {
        asteroidTexture = asteroidSpriteMap.texture;
        for (int y = 3; y >= 0; y--) {
            for (int x = 0; x <= 7; x++) {
                asteroidSpriteListA.Add(GenerateAsteroidSpriteFrame(x, y));
            }
        }
        for (int y = 7; y >= 4; y--) {
            for (int x = 0; x <= 7; x++) {
                asteroidSpriteListB.Add(GenerateAsteroidSpriteFrame(x, y));
            }
        }
    }

    void CreateBackgroundTiles() {
        for (int x = 0; x <= 2; x++) {
            for (int y  = 0; y <= 2; y++) {
                GameObject tile = Instantiate(backgroundTilePrefab);
                Vector3 position = tile.transform.position;
                position.x = (x - 1) * 40f;
                position.y = (y - 1) * 40f;
                tile.transform.position = position;
                backgroundTiles.Add(tile);
            }
        }
        
    }

    void CreateAsteroid() {
        Asteroid asteroid = null;
        for (int i = 0; i < 10; i++) {
            float originAngle = Random.Range(0f, 360f);
            float x = playerShip.XPosition + Mathf.Cos(originAngle * Mathf.Deg2Rad) * asteroidSpawnRadius;
            float y = playerShip.YPosition + Mathf.Sin(originAngle * Mathf.Deg2Rad) * asteroidSpawnRadius;
            bool isConflictingLocation = false;
            foreach(Asteroid ast in asteroids) {
                if (ast.DistanceFrom(x, y) < 3f) {
                    isConflictingLocation = true;
                    break;
                }
            }
            if (!isConflictingLocation) {
                asteroid = Instantiate(asteroidPrefab).GetComponent<Asteroid>();
                asteroid.XPosition = x;
                asteroid.YPosition = y;
                break;
            }
        }
        if (asteroid != null) {
            asteroid.FramesPerSecond = Random.Range(8, 33);
            asteroid.AnimationType = Random.Range(0, 2);

            float direction = Mathf.Atan2(asteroid.YPosition, asteroid.XPosition) * Mathf.Rad2Deg + 180;
            asteroid.Trajectory = Random.Range(direction - 30, direction + 30);
            asteroid.Velocity = Random.Range(0.5f, 4f);
            asteroid.Size = Random.Range(0.25f, 2f);
            asteroid.Rotation = Random.Range(-45f, 45f);
            asteroid.ToggleParticleTrail(debug);
            asteroids.Add(asteroid);
        }
    }

    void Start()
    {
        backgroundTiles = new List<GameObject>();
        CreateBackgroundTiles();
        asteroids = new List<Asteroid>();
        GenerateSprites();
        playerShip = Instantiate(playerShipPrefab).GetComponent<Ship>();
        for (int i = 0; i < numberOfAsteroids; i++) {
            CreateAsteroid();
        }
    }

    float[] Rotate(float[] v, float theta) {
        return new float[] {v[0] * Mathf.Cos(theta) - v[1] * Mathf.Sin(theta), v[0] * Mathf.Sin(theta) + v[1] * Mathf.Cos(theta)};
    }

    void CollisionEffect(ICollidable collidable1, ICollidable collidable2) {
        float m1 = collidable1.Mass;
        float m2 = collidable2.Mass;
        float theta = -Mathf.Atan2(collidable2.YPosition - collidable1.YPosition, collidable2.XPosition - collidable1.XPosition);
        float[] v1 = Rotate(new float[] {collidable1.XVelocity, collidable1.YVelocity}, theta);
        float[] v2 = Rotate(new float[] {collidable2.XVelocity, collidable2.YVelocity}, theta);
        float[] u1 = Rotate(new float[] {v1[0] * (m1 - m2)/(m1 + m2) + v2[0] * 2 * m2/(m1 + m2), v1[1]}, -theta);
        float[] u2 = Rotate(new float[] {v2[0] * (m2 - m1)/(m1 + m2) + v1[0] * 2 * m1/(m1 + m2), v2[1]}, -theta);
        collidable1.XVelocity = u1[0];
        collidable1.YVelocity = u1[1];
        collidable2.XVelocity = u2[0];
        collidable2.YVelocity = u2[1];
    }

    void Update()
    {
        // Add astroids
        if (asteroids.Count < numberOfAsteroids) {
            CreateAsteroid();
        }

        // Advance Game objects
        float delta = Time.deltaTime;

        List<Asteroid> destroyedAsteroids = new List<Asteroid>();
        foreach (Asteroid ast in asteroids) {
            ast.AdvancePosition(delta);
            if (ast.DistanceFrom(playerShip) > asteroidDestructionRadius) {
                destroyedAsteroids.Add(ast);
            }
        }
        foreach (Asteroid ast in destroyedAsteroids) {
            ast.Destruct();
            asteroids.Remove(ast);
        }
        playerShip.AdvancePosition(delta);

        // Determine Collisions
        for (int i = 0; i < asteroids.Count; i++) {
            for (int j = i; j < asteroids.Count; j++) {
                if (i != j && asteroids[i].isCollidingWith(asteroids[j], 0.8f)) {
                    CollisionEffect(asteroids[i], asteroids[j]);
                }
            }
            if (asteroids[i].isCollidingWith(playerShip, 1f)) {
                Debug.Log(string.Format("ship: {0}, asteroid: {1}", playerShip.Mass, asteroids[i].Mass));
                CollisionEffect(asteroids[i], playerShip);
            }
        }

        // Update Background Tile locations
        for (int x = 0; x <= 2; x++) {
            for (int y  = 0; y <= 2; y++) {
                GameObject tile = backgroundTiles[x * 3 + y];
                Vector3 position = tile.transform.position;
                position.x = (x - 1 + Mathf.Floor(playerShip.XPosition / 40)) * 40f;
                position.y = (y - 1 + Mathf.Floor(playerShip.YPosition / 40)) * 40f;
                tile.transform.position = position;
                backgroundTiles.Add(tile);
            }
        }
    }
}
