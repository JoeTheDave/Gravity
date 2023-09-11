using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    List<Sprite> spriteListA;
    List<Sprite> spriteListB;
    float elapsedTime;
    int frame;

    // object props
    int framesPerSecond = 16;
    int animationType = 1;

    void Start()
    {
        spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        Main script = GameObject.Find("Game").GetComponent<Main>();
        spriteListA = script.asteroidSpriteListA;
        spriteListB = script.asteroidSpriteListB;
        elapsedTime = 0;
        frame = 0;
        spriteRenderer.sprite = spriteListA[frame];
        if (animationType == 0) {
            spriteRenderer.sprite = spriteListA[frame];
        } else {
            spriteRenderer.sprite = spriteListB[frame];
        }
    }

    
    void Update()
    {
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
