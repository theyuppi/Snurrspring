using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class PathCreator : MonoBehaviour {

    [HideInInspector]
    public Path path;

	public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;

    public void Start()
    {
        this.edge = this.GetComponent<EdgeCollider2D>();
        edge.points = this.path.CalculateEvenlySpacedPoints(0.1f);
    }

    [SerializeField]
    private EdgeCollider2D edge;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    void Reset()
    {
        CreatePath();
    }
}
