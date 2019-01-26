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
    private List<Node> pointList;

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
            pointList[i].normal = Vector2.Perpendicular(pointList[i].vec2 - nextPoint.vec2);
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
}
