using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomXboxMover : MonoBehaviour
{
    Vector3 pos;

    public float tx = 0;
    public float ty = 0;

    public float ChangeX = 1;
    public float ChangeY = 1;
    public float Width = 100;
    public float Height = 100;

    public float x = 10;
    public float y = -10;

    void Start()
    {
        this.pos = this.gameObject.transform.position;
    }

    float C(float c)
    {
        return c * 2 - 1;
    }

    void Update()
    {
        this.tx += Time.deltaTime * this.ChangeX;
        this.ty += Time.deltaTime * this.ChangeY;

        var dx = C(Mathf.PerlinNoise(this.x, this.tx)) * this.Width;
        var dy = C(Mathf.PerlinNoise(this.y, this.ty)) * this.Height;
        this.gameObject.transform.position = this.pos + new Vector3(dx, dy);
    }
}
