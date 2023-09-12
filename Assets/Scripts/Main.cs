using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] Sprite asteroidSpriteMap;
    [SerializeField] GameObject asteroidPrefab;

    public List<Sprite> asteroidSpriteListA;
    public List<Sprite> asteroidSpriteListB;

    Texture2D asteroidTexture;
    List<Asteroid> asteroids;

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

    void CreateAsteroid() {
        Asteroid asteroid = Instantiate(asteroidPrefab).GetComponent<Asteroid>();
        asteroid.FramesPerSecond = Random.Range(8, 33);
        asteroid.AnimationType = Random.Range(0, 2);
        asteroid.XPosition = Random.Range(-10f, 11f);
        asteroid.YPosition = Random.Range(-10f, 11f);
        asteroid.Velocity = Random.Range(0.25f, 3f);
        asteroid.Trajectory = Random.Range(0f, 365f);
        asteroid.Size = Random.Range(0.25f, 2f);
        asteroid.Rotation = Random.Range(-45f, 45f);

        asteroids.Add(asteroid);
    }

    void Start()
    {
        asteroids = new List<Asteroid>();
        GenerateSprites();
        // for (int i = 0; i < 100; i++) {
        //     CreateAsteroid();
        // }
        CreateAsteroid();
        CreateAsteroid();
        asteroids[0].XPosition = -3f;
        asteroids[0].YPosition = -3f;
        asteroids[0].Size = 0.75f;
        asteroids[0].Trajectory = 40f;
        asteroids[0].Velocity = 0.5f;

        asteroids[1].XPosition = 2f;
        asteroids[1].YPosition = 5f;
        asteroids[1].Size = 0.3f;
        asteroids[1].Trajectory = 250f;
        asteroids[1].Velocity = 0.7f;


    }

    

    void Update()
    {
        float delta = Time.deltaTime;
        foreach(Asteroid ast in asteroids) {
            ast.AdvancePosition(delta);
        }
    }
}
