using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldV2 : MonoBehaviour
{
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static int worldSize = 2;
    public static Dictionary<string, ChunkV4> chunks;

    public static string BuildChunkName(Vector3 v)
    {
        return (int)v.x + "_" + (int)v.y + "_" + (int)v.z;
    }

    IEnumerator BuildChunkColumn()
    {
        for (int i = 0; i < columnHeight; i++)
        {
            Vector3 chunkPosition = new Vector3(this.transform.position.x, i * chunkSize, this.transform.position.z);
            ChunkV4 chunkV4 = new ChunkV4(chunkPosition, textureAtlas);
            chunkV4.chunk.transform.parent = this.transform;
            chunks.Add(chunkV4.chunk.name, chunkV4);
        }

        foreach (KeyValuePair<string, ChunkV4> chunk in chunks)
        {
            chunk.Value.DrawChunk();
            yield return null;
        }
    }

    IEnumerator BuildWorld()
    {
        for (int z = 0; z < worldSize; z++)
            for (int x = 0; x < worldSize; x++)
                for (int y = 0; y < columnHeight; y++)
                {
                    Vector3 chunkPosition = new Vector3(x * chunkSize, y * chunkSize, z * chunkSize);
                    ChunkV4 chunkV4 = new ChunkV4(chunkPosition, textureAtlas);
                    chunkV4.chunk.transform.parent = this.transform;
                    chunks.Add(chunkV4.chunk.name, chunkV4);
                }

        foreach (KeyValuePair<string, ChunkV4> c in chunks)
        {
            c.Value.DrawChunk();
            yield return null;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        chunks = new Dictionary<string, ChunkV4>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildWorld());
    }
}
