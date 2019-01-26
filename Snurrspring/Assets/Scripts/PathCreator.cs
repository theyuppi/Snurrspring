using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class PathCreator : MonoBehaviour
{

    [HideInInspector]
    public Path path;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;

    [SerializeField]
    private EdgeCollider2D edge;
    public List<Node> pointList;

    public bool flipNormals = false;

    public void Start()
    {
        pointList = new List<Node>();
        this.edge = this.GetComponent<EdgeCollider2D>();

        foreach (var point in this.path.CalculateEvenlySpacedPoints(0.1f))
        {
            pointList.Add(new Node(point));
        }
        for (int i = 0; i < pointList.Count; i++)
        {
            var nextPoint = (i + 1 < pointList.Count ? pointList[i + 1] : pointList[0]);
            pointList[i].normal = (flipNormals?-1:1) * Vector2.Perpendicular(pointList[i].vec2 - nextPoint.vec2);
        }
    }


    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    void Reset()
    {
        CreatePath();
    }

    private void OnDrawGizmos()
    {
        if (pointList != null)
            foreach (var point in pointList)
            {
                Debug.DrawRay(new Vector3(point.vec2.x, point.vec2.y, 0), point.normal, Color.red);
            }
    }

    public LineRenderer CompletionStyle;

    public float completionSize = 0.1f;

    LineRenderer New()
    {
        var r = Instantiate(this.CompletionStyle);
        var w = completionSize;
        r.startWidth = w;
        r.endWidth = w;
        return r;
    }

    private void Remove(LineRenderer r)
    {
        Destroy(r.gameObject);
    }

    public void Visit(int p)
    {
        var me = this.pointList[p];
        if (!me.visited)
        {
            me.visited = true;

            var next = this.pointList[(p + 1) % this.pointList.Count];
            var prev = this.pointList[(p + this.pointList.Count - 1) % this.pointList.Count];
            if(next.line == null && prev.line == null)
            {
                Debug.Log("Marked first");
                me.line = New();
                me.points = new List<Vector3>() { me.vec2 };

                me.line.positionCount = me.points.Count;
                me.line.SetPositions(me.points.ToArray());
            }
            else if(next.line != null && prev.line != null)
            {
                if(next.line != prev.line)
                {
                    Debug.Log("joing prev & next");
                    Remove(next.line);
                    prev.points.Add(me.vec2);
                    prev.points.AddRange(next.points);

                    prev.line.positionCount = prev.points.Count;
                    prev.line.SetPositions(prev.points.ToArray());

                    // todo: joining lines is crap because we only change the next node, we need to change all the nodes containing the next
                    me.line = prev.line;
                    me.points = prev.points;
                    next.line = prev.line;
                    next.points = prev.points;
                }
                else
                {
                    Debug.Log("Whole world filled");
                }
            }
            else if(prev.line != null)
            {
                prev.points.Add(me.vec2);
                me.points = prev.points;
                me.line = prev.line;

                prev.line.positionCount = prev.points.Count;
                prev.line.SetPositions(prev.points.ToArray());

                Debug.Log(string.Format("Adding to prev {0}", prev.points.Count));
            }
            else
            {
                Debug.Log("Adding to next");
                me.points = new List<Vector3>() { me.vec2 };
                me.points.AddRange(next.points);
                me.line = prev.line;
                prev.points = me.points;
                me.line.positionCount = me.points.Count;
                me.line.SetPositions(me.points.ToArray());
            }
        }
    }
}


public class Node
{
    public Node(Vector2 point)
    {
        this.vec2 = point;
        visited = false;
    }

    public Vector2 vec2;
    public Vector2 normal;
    public bool visited;

    public LineRenderer line;
    public List<Vector3> points;
}
