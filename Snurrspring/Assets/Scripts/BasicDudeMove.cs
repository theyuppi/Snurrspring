using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDudeMove : MonoBehaviour
{
    public PathCreator path;
    private Player player;
    int p = 0;
    float t = 0;
    public float speed = 10;
    public float degreesDelta = 10;

    void Start()
    {
        player = this.GetComponent<Player>();
    }

    void Update()
    {
        t += Time.deltaTime * speed;

        while (t > 1)
        {
            p = (p + 1) % path.pointList.Count;

            if(this.player.isGrounded)
            {
                path.Visit(p);
            }
            t -= 1;
        }

        var pos =  Vector2.Lerp(path.pointList[p].vec2, path.pointList[(p+1) % path.pointList.Count].vec2, t);
        this.gameObject.transform.position = (new Vector3(pos.x, pos.y) + player.Offset);
        if(this.player.isGrounded )
        {
            var normal = Vector2.Lerp(path.pointList[p].normal,
                path.pointList[(p + 1) % path.pointList.Count].normal, t)
                    .normalized;

            this.gameObject.transform.rotation =
                Quaternion.RotateTowards(this.gameObject.transform.rotation,
                    DudeOrientation.CalcOrientation(normal), this.degreesDelta );
        }
    }
}
