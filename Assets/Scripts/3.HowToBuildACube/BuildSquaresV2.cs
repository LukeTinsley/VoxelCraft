using UnityEngine;

public class BuildSquaresV2 : MonoBehaviour
{
    public Material squareMaterial;
    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

    void BuildSquare(Cubeside side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();

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

        switch (side)
        {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

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

    void CombineSquaresToCube()
    {
        int i = 0;

        // 1. Combine all children meshes
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // 2. Create a new mesh on the parent object
        MeshFilter meshFilter = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = new Mesh();

        // 3. Add combined meshes on children as the parent's mesh
        meshFilter.mesh.CombineMeshes(combine);

        // 4. Create a renderer for the parent
        MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = squareMaterial;

        // 5. Delete all uncombined children
        foreach (Transform square in this.transform)
        {
            Destroy(square.gameObject);
        }
    }

    void CreateCube()
    {
        BuildSquare(Cubeside.FRONT);
        BuildSquare(Cubeside.BACK);
        BuildSquare(Cubeside.TOP);
        BuildSquare(Cubeside.BOTTOM);
        BuildSquare(Cubeside.LEFT);
        BuildSquare(Cubeside.RIGHT);

        // Uncomment this method to combine the squares into a single cube mesh.
        // CombineSquaresToCube();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateCube();
    }
}
