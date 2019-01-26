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

    private void Replace(DrawLine older, DrawLine newer)
    {
        Destroy(older.line.gameObject);

        // todo: optimize this!
        foreach(var node in this.pointList)
        {
            if(node.line == older)
            {
                node.line = newer;
            }
        }
    }

    public void Visit(int p)
    {
        var me = this.pointList[p];
        if (!me.visited)
        {
            me.visited = true;

            if(this.CompletionStyle == null)
            {
                return;
            }

            var next = this.pointList[(p + 1) % this.pointList.Count];
            var prev = this.pointList[(p + this.pointList.Count - 1) % this.pointList.Count];

            if(next.line == null && prev.line == null)
            {
                me.line = new DrawLine(me.vec2)
                {
                    line = New()
                };

                me.line.UpdateLine();
            }
            else if(next.line != null && prev.line != null)
            {
                if(next.line != prev.line)
                {
                    prev.line.points.Add(me.vec2);
                    prev.line.points.AddRange(next.line.points);

                    prev.line.UpdateLine();

                    // todo: joining lines is crap because we only change the next node, we need to change all the nodes containing the next
                    me.line = prev.line;
                    Replace(next.line, prev.line);
                }
                else
                {
                    next.line.line.loop = true;
                    me.line = next.line;
                }
            }
            else if(prev.line != null)
            {
                prev.line.points.Add(me.vec2);
                me.line = prev.line;

                prev.line.UpdateLine();
            }
            else
            {
                me.line = new DrawLine(me.vec2);
                me.line.points.AddRange(next.line.points);
                me.line = prev.line;
                me.line.UpdateLine();
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

    public DrawLine line;
}

public class DrawLine
{
    public LineRenderer line;
    public List<Vector3> points;

    public DrawLine(Vector3 p)
    {
        points = new List<Vector3>() { p };
    }

    public void UpdateLine()
    {
        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}
