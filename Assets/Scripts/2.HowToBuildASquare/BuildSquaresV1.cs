using UnityEngine;

public class BuildSquaresV1 : MonoBehaviour
{
    public Material squareMaterial;

    void BuildSquare()
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh";

        // 4 corners of a square
        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] unitVectors = new Vector2[4];

        // 2 triangles in a square containing 6 points
        int[] triangles = new int[6];

        // All possible unitVectors of square
        Vector2 uv00 = new Vector2(0f, 0f); // Bottom left
        Vector2 uv10 = new Vector2(1f, 0f); // Bottom right
        Vector2 uv01 = new Vector2(0f, 1f); // Top left
        Vector2 uv11 = new Vector2(1f, 1f); // Top right

        // All possible vertices of a cube/block
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        // The specific points of a front facing square
        vertices = new Vector3[] { p4, p5, p1, p0 };
        normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };

        unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
        triangles = new int[] { 3, 1, 0, 3, 2, 1 };

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = unitVectors;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        // Create the square
        GameObject square = new GameObject("Square");
        square.transform.parent = this.gameObject.transform;

        MeshFilter meshFilter = (MeshFilter)square.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

        MeshRenderer renderer = square.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = squareMaterial;
    }

    // Start is called before the first frame update
    void Start()
    {
        BuildSquare();
    }
}
