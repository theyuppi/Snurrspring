using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public SpriteRenderer sb;
    public Sprite sprite;

    void Start()
    {
        sb = this.GetComponent<SpriteRenderer>();
        sprite = this.GetComponent<Sprite>();
    }
}
