using UnityEngine;

public class Chunk
{
    public Material cubeMaterial;
    public Block[,,] chunkData;
    public GameObject chunkObject;
    public ChunkStatus chunkStatus;

    void BuildChunk()
    {
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];

        // Create blocks in chunk
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    int worldX = (int)(x + chunkObject.transform.position.x);
                    int worldY = (int)(y + chunkObject.transform.position.y);
                    int worldZ = (int)(z + chunkObject.transform.position.z);

                    if (Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 3) < 0.42f)
                        chunkData[x, y, z] = new Block(BlockType.AIR, position, chunkObject.gameObject, this);
                    else if (worldY == 0)
                        chunkData[x, y, z] = new Block(BlockType.BEDROCK, position, chunkObject.gameObject, this);
                    else if (worldY <= Utils.GenerateStoneHeight(worldX, worldZ))
                    {
                        if (Utils.fBM3D(worldX, worldY, worldZ, 0.01f, 2) < 0.38f && worldY < 30)
                            chunkData[x, y, z] = new Block(BlockType.DIAMOND, position, chunkObject.gameObject, this);
                        else if (Utils.fBM3D(worldX, worldY, worldZ, 0.03f, 3) < 0.41f && worldY < 15)
                            chunkData[x, y, z] = new Block(BlockType.REDSTONE, position, chunkObject.gameObject, this);
                        else
                            chunkData[x, y, z] = new Block(BlockType.STONE, position, chunkObject.gameObject, this);
                    }
                    else if (worldY == Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(BlockType.GRASS, position, chunkObject.gameObject, this);
                    else if (worldY < Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(BlockType.DIRT, position, chunkObject.gameObject, this);
                    else
                        chunkData[x, y, z] = new Block(BlockType.AIR, position, chunkObject.gameObject, this);

                    chunkStatus = ChunkStatus.DRAW;
                }
    }

    public void DrawChunk()
    {
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
        CombineSquaresToCube();
        MeshCollider collider = chunkObject.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = chunkObject.transform.GetComponent<MeshFilter>().mesh;
    }

    public Chunk(Vector3 position, Material material)
    {
        chunkObject = new GameObject(World.BuildChunkName(position));
        chunkObject.transform.position = position;
        cubeMaterial = material;
        BuildChunk();
    }

    void CombineSquaresToCube()
    {
        int i = 0;

        // 1. Combine all children meshes
        MeshFilter[] meshFilters = chunkObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // 2. Create a new mesh on the parent object
        MeshFilter meshFilter = (MeshFilter)chunkObject.gameObject.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = new Mesh();

        // 3. Add combined meshes on children as the parent's mesh
        meshFilter.mesh.CombineMeshes(combine);

        // 4. Create a renderer for the parent
        MeshRenderer renderer = chunkObject.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = cubeMaterial;

        // 5. Delete all uncombined children
        foreach (Transform square in chunkObject.transform)
            GameObject.Destroy(square.gameObject);
    }
}
