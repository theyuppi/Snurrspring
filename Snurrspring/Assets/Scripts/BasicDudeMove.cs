using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDudeMove : MonoBehaviour
{
    public PathCreator path;
    public PathCreator[] allPaths;

    private Player player;
    int positionIndex = 0;
    float timeUntilIndexChange = 0;
    public float speed = 10;
    public float degreesDelta = 10;

    bool runPositive = true;

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
        timeUntilIndexChange += Time.deltaTime * speed;

        while(player.switchDirectionPlease > 0)
        {
            runPositive = !runPositive;
            timeUntilIndexChange = 1 - timeUntilIndexChange;
            player.switchDirectionPlease -= 1;
        }

        while (timeUntilIndexChange > 1)
        {
            positionIndex = (positionIndex + (runPositive ? 1 : -1) + path.pointList.Count) % path.pointList.Count;

            if(this.player.isGrounded)
            {
                path.Visit(positionIndex);
            }
            timeUntilIndexChange -= 1;

            // Debug.Log(string.Format("Percentage complete: {0}/{1}", PercentageComplete * 100, TotalPercentageComplete * 100));
        }

        var nextIndex = (positionIndex + (runPositive ? 1 : -1) + path.pointList.Count) % path.pointList.Count;

        var pos =  Vector2.Lerp(path.pointList[positionIndex].vec2, path.pointList[nextIndex].vec2, timeUntilIndexChange);
        var playerpos = new Vector3(pos.x, pos.y) + player.Offset;
        this.gameObject.transform.position = playerpos;

        // if on the ground, rotate to player to stand up straight
        if(this.player.isGrounded )
        {
            var normal = Vector2.Lerp(path.pointList[positionIndex].normal,
                path.pointList[nextIndex].normal, timeUntilIndexChange)
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
                    positionIndex = c.id;
                    this.path = c.pc;
                    timeUntilIndexChange = 0;
                    player.isGrounded = true;
                }
            }
        }
    }


    public float timeInJumpBeforeCollisionCheck = 0.2f;
    public float squaredDistanceCollision = 0.2f;

}
