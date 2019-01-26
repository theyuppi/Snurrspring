using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDudeMove : MonoBehaviour
{
    public PathCreator path;

    Vector2[] points;
    int p = 0;
    float t = 0;
    public float speed = 10;

    void Start()
    {
        points = path.path.CalculateEvenlySpacedPoints(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * speed;

        while (t > 1)
        {
            p = (p + 1) % points.Length;
            t -= 1;
        }

        var pos = points[p];
        this.gameObject.transform.position = new Vector3(pos.x, pos.y);
        this.gameObject.transform.rotation = DudeOrientation.CalcOrientation(pos, points[(p + 1) % points.Length]);
    }
}
