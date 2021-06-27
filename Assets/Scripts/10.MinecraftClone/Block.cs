using UnityEngine;

public class Block
{
    BlockType blockType;
    GameObject blockObject;
    Vector3 position;
    Chunk parentChunk;
    public bool isSolid;

    Vector2[,] blockUVs = { 
		/*GRASS TOP*/   {new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f), new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/  {new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f), new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
		/*DIRT*/        {new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f), new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*STONE*/       {new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f), new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
        /*BEDROCK*/     {new Vector2( 0.3125f, 0.8125f ), new Vector2( 0.375f, 0.8125f), new Vector2( 0.3125f, 0.875f ),new Vector2( 0.375f, 0.875f )},
		/*REDSTONE*/    {new Vector2( 0.1875f, 0.75f ), new Vector2( 0.25f, 0.75f), new Vector2( 0.1875f, 0.8125f ),new Vector2( 0.25f, 0.8125f )},
        /*DIAMOND*/	    {new Vector2( 0.125f, 0.75f ), new Vector2( 0.1875f, 0.75f), new Vector2( 0.125f, 0.8125f ),new Vector2( 0.1875f, 0.8125f )}
    };

    public Block(BlockType type, Vector3 pos, GameObject parent, Chunk chunk)
    {
        blockType = type;
        position = pos;
        blockObject = parent;
        parentChunk = chunk;
        if (blockType == BlockType.AIR)
            isSolid = false;
        else
            isSolid = true;
    }

    void BuildSquare(CubeSide side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();

        // 4 corners of a square
        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] unitVectors = new Vector2[4];

        // 2 triangles in a square containing 6 points
        int[] triangles = new int[6];

        Vector2 uv00, uv10, uv01, uv11;

        if (blockType == BlockType.GRASS && side == CubeSide.TOP)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
        else if (blockType == BlockType.GRASS && side == CubeSide.BOTTOM)
        {
            uv00 = blockUVs[(int)(BlockType.DIRT + 1), 0];
            uv10 = blockUVs[(int)(BlockType.DIRT + 1), 1];
            uv01 = blockUVs[(int)(BlockType.DIRT + 1), 2];
            uv11 = blockUVs[(int)(BlockType.DIRT + 1), 3];
        }
        else
        {
            uv00 = blockUVs[(int)(blockType + 1), 0];
            uv10 = blockUVs[(int)(blockType + 1), 1];
            uv01 = blockUVs[(int)(blockType + 1), 2];
            uv11 = blockUVs[(int)(blockType + 1), 3];
        }

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
            case CubeSide.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubeSide.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubeSide.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubeSide.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubeSide.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                unitVectors = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubeSide.BACK:
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
        square.transform.position = position;
        square.transform.parent = blockObject.transform;

        MeshFilter meshFilter = (MeshFilter)square.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
    }

    int ConvertBlockIndexToLocal(int i)
    {
        if (i == -1)
            i = World.chunkSize - 1;
        else if (i == World.chunkSize)
            i = 0;
        return i;
    }

    public bool HasSolidNeighbor(int x, int y, int z)
    {
        Block[,,] chunkData;

        // Block in a neighboring chunk
        if (x < 0 || x >= World.chunkSize || y < 0 || y >= World.chunkSize || z < 0 || z >= World.chunkSize)
        {
            Vector3 neighbourChunkPos = this.blockObject.transform.position +
                                        new Vector3((x - (int)position.x) * World.chunkSize,
                                            (y - (int)position.y) * World.chunkSize,
                                            (z - (int)position.z) * World.chunkSize);
            string chunkName = World.BuildChunkName(neighbourChunkPos);

            x = ConvertBlockIndexToLocal(x);
            y = ConvertBlockIndexToLocal(y);
            z = ConvertBlockIndexToLocal(z);

            Chunk chunk;
            if (World.worldChunksDictionary.TryGetValue(chunkName, out chunk))
            {
                chunkData = chunk.chunkData;
            }
            else
                return false;
        }  //block in this chunk
        else
            chunkData = parentChunk.chunkData;

        try
        {
            return chunkData[x, y, z].isSolid;
        }
        catch (System.IndexOutOfRangeException) { }
        return false;
    }

    public void Draw()
    {
        if (blockType == BlockType.AIR)
            return;

        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z + 1))
            BuildSquare(CubeSide.FRONT);
        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z - 1))
            BuildSquare(CubeSide.BACK);
        if (!HasSolidNeighbor((int)position.x, (int)position.y + 1, (int)position.z))
            BuildSquare(CubeSide.TOP);
        if (!HasSolidNeighbor((int)position.x, (int)position.y - 1, (int)position.z))
            BuildSquare(CubeSide.BOTTOM);
        if (!HasSolidNeighbor((int)position.x - 1, (int)position.y, (int)position.z))
            BuildSquare(CubeSide.LEFT);
        if (!HasSolidNeighbor((int)position.x + 1, (int)position.y, (int)position.z))
            BuildSquare(CubeSide.RIGHT);
    }
}
