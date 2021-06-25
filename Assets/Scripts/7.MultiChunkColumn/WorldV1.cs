using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldV1 : MonoBehaviour
{
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static Dictionary<string, ChunkV3> chunks;

    public static string BuildChunkName(Vector3 v)
    {
        return (int)v.x + "_" + (int)v.y + "_" + (int)v.z;
    }

    IEnumerator BuildChunkColumn()
    {
        for (int i = 0; i < columnHeight; i++)
        {
            Vector3 chunkPosition = new Vector3(this.transform.position.x, i * chunkSize, this.transform.position.z);
            ChunkV3 chunkV3 = new ChunkV3(chunkPosition, textureAtlas);
            chunkV3.chunk.transform.parent = this.transform;
            chunks.Add(chunkV3.chunk.name, chunkV3);
        }

        foreach (KeyValuePair<string, ChunkV3> chunk in chunks)
        {
            chunk.Value.DrawChunk();
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        chunks = new Dictionary<string, ChunkV3>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildChunkColumn());
    }
}
