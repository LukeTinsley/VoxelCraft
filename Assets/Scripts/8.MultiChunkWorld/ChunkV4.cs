using UnityEngine;

public class ChunkV4
{
    public Material cubeMaterial;
    public BlockV4[,,] chunkData;
    public GameObject chunk;

    void BuildChunk()
    {
        chunkData = new BlockV4[WorldV2.chunkSize, WorldV2.chunkSize, WorldV2.chunkSize];

        // Create blocks
        for (int z = 0; z < WorldV2.chunkSize; z++)
            for (int y = 0; y < WorldV2.chunkSize; y++)
                for (int x = 0; x < WorldV2.chunkSize; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    if (Random.Range(0, 100) < 95)
                        chunkData[x, y, z] = new BlockV4(BlockV4.BlockType.DIRT, position, chunk.gameObject, this);
                    else
                        chunkData[x, y, z] = new BlockV4(BlockV4.BlockType.AIR, position, chunk.gameObject, this);
                }
    }

    public void DrawChunk()
    {
        for (int z = 0; z < WorldV2.chunkSize; z++)
            for (int y = 0; y < WorldV2.chunkSize; y++)
                for (int x = 0; x < WorldV2.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
        CombineSquaresToCube();
    }

    public ChunkV4(Vector3 position, Material c)
    {
        chunk = new GameObject(WorldV2.BuildChunkName(position));
        chunk.transform.position = position;
        cubeMaterial = c;
        BuildChunk();
    }

    void CombineSquaresToCube()
    {
        int i = 0;

        // 1. Combine all children meshes
        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // 2. Create a new mesh on the parent object
        MeshFilter meshFilter = (MeshFilter)chunk.gameObject.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = new Mesh();

        // 3. Add combined meshes on children as the parent's mesh
        meshFilter.mesh.CombineMeshes(combine);

        // 4. Create a renderer for the parent
        MeshRenderer renderer = chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = cubeMaterial;

        // 5. Delete all uncombined children
        foreach (Transform square in chunk.transform)
        {
            GameObject.Destroy(square.gameObject);
        }
    }
}
