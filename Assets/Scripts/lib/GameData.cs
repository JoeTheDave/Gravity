using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Reference to Main script
    Main main;

    // Constants
    int numberOfAsteroids;
    public int NumberOfAsteroids {
        get {
            return numberOfAsteroids;
        }
        set {
            numberOfAsteroids = value;
        }
    }

    float asteroidSpawnRadius;
    public float AsteroidSpawnRadius {
        get {
            return asteroidSpawnRadius;
        }
        set {
            asteroidSpawnRadius = value;
        }
    }

    float asteroidDestructionRadius;
    public float AsteroidDestructionRadius {
        get {
            return asteroidDestructionRadius;
        }
        set {
            asteroidDestructionRadius = value;
        }
    }

    bool inertiaDamper;
    public bool InertiaDamper {
        get {
            return inertiaDamper;
        }
        set {
            inertiaDamper = value;
        }
    }

    // Background Tiles
    List<GameObject> backgroundTiles;
    public List<GameObject> BackgroundTiles {
        get {
            return backgroundTiles;
        }
        set {
            backgroundTiles = value;
        }
    }

    public void CreateBackgroundTiles() {
        for (int x = 0; x <= 2; x++) {
            for (int y  = 0; y <= 2; y++) {
                GameObject tile = Instantiate(main.backgroundTilePrefab);
                Vector3 position = tile.transform.position;
                position.x = (x - 1) * 40f;
                position.y = (y - 1) * 40f;
                tile.transform.position = position;
                BackgroundTiles.Add(tile);
            }
        }
    }

    public void UpdateGameTilesFromPlayerPosition() {
        for (int x = 0; x <= 2; x++) {
            for (int y  = 0; y <= 2; y++) {
                GameObject tile = GameData.Instance.BackgroundTiles[x * 3 + y];
                Vector3 position = tile.transform.position;
                position.x = (Mathf.Floor((PlayerShip.XPosition + 20) / 40f) + x - 1) * 40f;
                position.y = (Mathf.Floor((PlayerShip.YPosition + 20) / 40f) + y - 1) * 40f;
                tile.transform.position = position;
            }
        }
    }

    // Asteroids
    List<Sprite> asteroidSpriteListA;
    public List<Sprite> AsteroidSpriteListA {
        get {
            return asteroidSpriteListA;
        }
        set {
            asteroidSpriteListA = value;
        }
    }

    List<Sprite> asteroidSpriteListB;
    public List<Sprite> AsteroidSpriteListB {
        get {
            return asteroidSpriteListB;
        }
        set {
            asteroidSpriteListB = value;
        }
    }

    List<Asteroid> asteroids;
    public List<Asteroid> Asteroids {
        get {
            return asteroids;
        }
        set {
            asteroids = value;
        }
    }

    Sprite GenerateAsteroidSpriteFrame(int x, int y, Texture2D texture) {
        Rect rect = new Rect(x * 128, y * 128, 128, 128);
        Sprite sprite = Sprite.Create(texture, rect, Vector2.one * 0.5f);
        return sprite;
    }

    public void GenerateAsteroidSprites() {
        Texture2D asteroidTexture = main.asteroidSpriteMap.texture;
        for (int y = 3; y >= 0; y--) {
            for (int x = 0; x <= 7; x++) {
                asteroidSpriteListA.Add(GenerateAsteroidSpriteFrame(x, y, asteroidTexture));
            }
        }
        for (int y = 7; y >= 4; y--) {
            for (int x = 0; x <= 7; x++) {
                asteroidSpriteListB.Add(GenerateAsteroidSpriteFrame(x, y, asteroidTexture));
            }
        }
    }

    public void CreateAsteroid() {
        Asteroid asteroid = null;
        for (int i = 0; i < 10; i++) {
            float originAngle = Random.Range(0f, 360f);
            float x = playerShip.XPosition + Mathf.Cos(originAngle * Mathf.Deg2Rad) * AsteroidSpawnRadius;
            float y = playerShip.YPosition + Mathf.Sin(originAngle * Mathf.Deg2Rad) * AsteroidSpawnRadius;
            bool isConflictingLocation = false;
            foreach(Asteroid ast in asteroids) {
                if (ast.DistanceFrom(x, y) < 3f) {
                    isConflictingLocation = true;
                    break;
                }
            }
            if (!isConflictingLocation) {
                asteroid = Instantiate(main.asteroidPrefab).GetComponent<Asteroid>();
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
            asteroid.UpdateMassRelativeToSize();
            asteroid.Rotation = Random.Range(-45f, 45f);
            asteroid.ToggleParticleTrail(main.debug);
            Asteroids.Add(asteroid);
        }
    }

    public void DestroyMarkedAsteroids() {
        List<Asteroid> markedAsteroids = new List<Asteroid>();
        foreach (Asteroid ast in Asteroids) {
            if (ast.ShouldDestroy) {
                markedAsteroids.Add(ast);
            }
        }
        foreach (Asteroid ast in markedAsteroids) {
            Asteroids.Remove(ast);
            ast.Destruct();
        }
    }

    // Player Ship
    Ship playerShip;
    public Ship PlayerShip {
        get {
            return playerShip;
        }
        set {
            playerShip = value;
        }
    }

    void CreatePlayerShip() {
        playerShip = Instantiate(main.playerShipPrefab).GetComponent<Ship>();
        playerShip.Mass = 5000000f;
        playerShip.Size = 1f;
    }

    // Bullets
    List<Bullet> bullets;
    public List<Bullet> Bullets {
        get {
            return bullets;
        }
        set {
            bullets = value;
        }
    }

    public void DestroyMarkedBullets() {
        List<Bullet> markedBullets = new List<Bullet>();
        foreach (Bullet bullet in Bullets) {
            if (bullet.ShouldDestroy) {
                markedBullets.Add(bullet);
            }
        }
        foreach (Bullet bullet in markedBullets) {
            Bullets.Remove(bullet);
            bullet.Destruct();
        }
    }

    // Collisions
    float[] Rotate(float[] v, float theta) {
        return new float[] {v[0] * Mathf.Cos(theta) - v[1] * Mathf.Sin(theta), v[0] * Mathf.Sin(theta) + v[1] * Mathf.Cos(theta)};
    }

    void CollisionEffect(Collidable collidable1, Collidable collidable2) {
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

    // Singleton GameData
    static GameData instance;
    public static GameData Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameData>();
                if (FindObjectsOfType<GameData>().Length > 1) {
                    Debug.LogError("More than one Singleton instance found in the scene.");
                    return instance;
                }
                if (instance == null) {
                    GameObject singletonObject = new GameObject("GameData");
                    instance = singletonObject.AddComponent<GameData>();
                    instance.backgroundTiles = new List<GameObject>();
                    instance.asteroidSpriteListA = new List<Sprite>();
                    instance.asteroidSpriteListB = new List<Sprite>();
                    instance.asteroids = new List<Asteroid>();
                    instance.bullets = new List<Bullet>();
                }
            }
            return instance;
        }
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        main = GameObject.Find("Game").GetComponent<Main>();
        CreateBackgroundTiles();
        GenerateAsteroidSprites();
        CreatePlayerShip();
    }

    void Update() {
        // Advance Game objects
        float delta = Time.deltaTime;
        foreach (Asteroid ast in Asteroids) {
            ast.AdvancePosition(delta);
        }
        PlayerShip.AdvancePosition(delta);
        foreach (Bullet bullet in Bullets) {
            bullet.AdvancePosition(delta);
        }

        // Mark stray asteroids for destruction
        foreach (Asteroid ast in Asteroids) {
            if (ast.DistanceFrom(playerShip) > AsteroidDestructionRadius) {
                ast.ShouldDestroy = true;
            }
        }

        // Mark stray bullets for destruction
        foreach (Bullet bullet in Bullets) {
            if (bullet.DistanceFrom(playerShip) > AsteroidDestructionRadius) {
                bullet.ShouldDestroy = true;
            }
        }

        // Determine Collisions
        for (int i = 0; i < Asteroids.Count; i++) {
            for (int j = i; j < Asteroids.Count; j++) {
                if (i != j && Asteroids[i].isCollidingWith(Asteroids[j], 0.8f)) {
                    CollisionEffect(Asteroids[i], Asteroids[j]);
                }
            }
            if (Asteroids[i].isCollidingWith(PlayerShip, 1f)) {
                CollisionEffect(Asteroids[i], PlayerShip);
            }
            for (int j = 0; j < Bullets.Count; j++) {
                if (Asteroids[i].isCollidingWith(Bullets[j], 0.8f)) {
                    CollisionEffect(Asteroids[i], Bullets[j]);
                    Bullets[j].ShouldDestroy = true;
                }
            }
        }

        // Background
        UpdateGameTilesFromPlayerPosition();

        // Clean up destroyed objects
        DestroyMarkedAsteroids();
        DestroyMarkedBullets();

        // Add Asteroids to specified amount
        if (Asteroids.Count < NumberOfAsteroids) {
            CreateAsteroid();
        }
    }
}
