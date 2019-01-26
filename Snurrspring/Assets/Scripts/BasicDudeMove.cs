using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDudeMove : MonoBehaviour
{
    public PathCreator path;
    public PathCreator[] allPaths;

    private Player player;
    int p = 0;
    float t = 0;
    public float speed = 10;
    public float degreesDelta = 10;

    void Start()
    {
        player = this.GetComponent<Player>();

        if(allPaths == null || allPaths.Length == 0)
        {
            allPaths = new PathCreator[] { path };
        }
    }

    public float PercentageComplete
    {
        get
        {
            return path.percentComplete;
        }
    }

    public float TotalPercentageComplete
    {
        get
        {
            float sum = 0;
            foreach(var pa in allPaths)
            {
                sum += pa.percentComplete;
            }
            return sum / allPaths.Length;
        }
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

            // Debug.Log(string.Format("Percentage complete: {0}/{1}", PercentageComplete * 100, TotalPercentageComplete * 100));
        }

        var pos =  Vector2.Lerp(path.pointList[p].vec2, path.pointList[(p+1) % path.pointList.Count].vec2, t);
        var playerpos = new Vector3(pos.x, pos.y) + player.Offset;
        this.gameObject.transform.position = playerpos;

        // if on the ground, rotate to player to stand up straight
        if(this.player.isGrounded )
        {
            var normal = Vector2.Lerp(path.pointList[p].normal,
                path.pointList[(p + 1) % path.pointList.Count].normal, t)
                    .normalized;

            this.gameObject.transform.rotation =
                Quaternion.RotateTowards(this.gameObject.transform.rotation,
                    DudeOrientation.CalcOrientation(normal), this.degreesDelta );
        }

        // some time after jumping, start checking for new positions to jump to
        if(player.timeInJump > timeInJumpBeforeCollisionCheck)
        {
            // brute force the best fit
            ClosestPoint c = null;
            foreach(var pp in allPaths)
            {
                c = ClosestPoint.GetClosest(pp.GetClosestPoint(playerpos), c);
            }

            if(c != null)
            {
                // Debug.Log(string.Format("Closest sq distance is {0}", c.distances));
                if(c.distances < squaredDistanceCollision)
                {
                    // there was a collision
                    p = c.id;
                    this.path = c.pc;
                    t = 0;
                    player.isGrounded = true;
                }
            }
        }
    }


    public float timeInJumpBeforeCollisionCheck = 0.2f;
    public float squaredDistanceCollision = 0.2f;

}
