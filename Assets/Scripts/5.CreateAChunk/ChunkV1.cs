using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkV1 : MonoBehaviour
{
    public Material cubeMaterial;

    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
    {
        for (int z = 0; z < sizeZ; z++)
            for (int y = 0; y < sizeY; y++)
                for (int x = 0; x < sizeX; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    BlockV1 block = new BlockV1(BlockV1.BlockType.DIRT, position, this.gameObject, cubeMaterial);
                    block.Draw();
                    yield return null;
                }
        CombineSquaresToCube();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildChunk(5, 5, 5));
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
        renderer.material = cubeMaterial;

        // 5. Delete all uncombined children
        foreach (Transform square in this.transform)
        {
            Destroy(square.gameObject);
        }
    }
}
