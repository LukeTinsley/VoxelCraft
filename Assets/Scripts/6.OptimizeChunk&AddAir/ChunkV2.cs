using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkV2 : MonoBehaviour
{
    public Material cubeMaterial;
    public BlockV2[,,] chunkData;

    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
    {
        chunkData = new BlockV2[sizeX, sizeY, sizeZ];

        // Create blocks
        for (int z = 0; z < sizeZ; z++)
            for (int y = 0; y < sizeY; y++)
                for (int x = 0; x < sizeX; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    if (Random.Range(0, 100) < 95)
                        chunkData[x, y, z] = new BlockV2(BlockV2.BlockType.DIRT, position, this.gameObject, cubeMaterial);
                    else
                        chunkData[x, y, z] = new BlockV2(BlockV2.BlockType.AIR, position, this.gameObject, cubeMaterial);
                }

        //draw blocks
        for (int z = 0; z < sizeZ; z++)
            for (int y = 0; y < sizeY; y++)
                for (int x = 0; x < sizeX; x++)
                {
                    chunkData[x, y, z].Draw();
                }

        CombineSquaresToCube();
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildChunk(20, 20, 20));
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
