using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolyCreator : MonoBehaviour
{
    private void Start()
    {
        // Create Vector2 vertices
        var vertices2D = new Vector2[] {
            new Vector2(0,0),
            new Vector2(0.1f,0),
            new Vector2(0.1f,-0.05f),
            new Vector2(0.15f,-0.1f),
            new Vector2(0.2f,-0.15f),
            new Vector2(0.25f,-0.15f),
            new Vector2(0.3f,-0.1f),
            new Vector2(0.3f,-0.05f),
            new Vector2(0.3f,0f),
        };

        var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

        // Use the triangulator to get indices for creating triangles
        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        // Generate a color for each vertex
        Color col = new Color(100, 100, 100);
        var colors = Enumerable.Range(0, vertices3D.Length)
            .Select(i => col)
            //.Select(i => Random.ColorHSV())
            .ToArray();
        
        // Create the mesh
        var mesh = new Mesh
        {
            vertices = vertices3D,
            triangles = indices,
            colors = colors
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set up game object with mesh;
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
    }
}
