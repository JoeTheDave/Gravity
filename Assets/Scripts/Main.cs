using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] Sprite spriteMap;

    public List<Sprite> asteroidSpriteListA;
    public List<Sprite> asteroidSpriteListB;

    Texture2D texture;

    void Start()
    {
        texture = spriteMap.texture;
        for (int y = 3; y >= 0; y--) {
            for (int x = 0; x <= 7; x++) {
                asteroidSpriteListA.Add(generateSpriteFrame(x, y));
            }
        }
        for (int y = 7; y >= 4; y--) {
            for (int x = 0; x <= 7; x++) {
                asteroidSpriteListB.Add(generateSpriteFrame(x, y));
            }
        }
    }

    Sprite generateSpriteFrame(int x, int y) {
        Rect rect = new Rect(x * 128, y * 128, 128, 128);
        Sprite sprite = Sprite.Create(texture, rect, Vector2.one * 0.5f);
        return sprite;
    }

    void Update()
    {
        
    }
}
