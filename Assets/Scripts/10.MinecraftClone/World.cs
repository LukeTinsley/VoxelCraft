using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public Slider loadingAmount;
    public Camera cam;
    public Button playButton;
    public GameObject player;
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static int worldSize = 1;
    public static int radius = 4;
    public static Dictionary<string, Chunk> worldChunksDictionary;
    public static bool firstBuild = true;

    public static string BuildChunkName(Vector3 vector3)
    {
        return (int)vector3.x + "_" + (int)vector3.y + "_" + (int)vector3.z;
    }

    void BuildChunkAt(int x, int y, int z)
    {
        Vector3 chunkPosition = new Vector3(x * chunkSize, y * chunkSize, z * chunkSize);

        string chunkName = BuildChunkName(chunkPosition);
        Chunk chunk;

        if (!worldChunksDictionary.TryGetValue(chunkName, out chunk))
        {
            chunk = new Chunk(chunkPosition, textureAtlas);
            chunk.chunkObject.transform.parent = this.transform;
            worldChunksDictionary.Add(chunk.chunkObject.name, chunk);
        }

    }

    IEnumerator BuildRecursiveWorld(int x, int y, int z, int rad)
    {
        if (rad <= 0) yield break;
        BuildChunkAt(x, y, z - 1);
        StartCoroutine(BuildRecursiveWorld(x, y, z - 1, rad - 1));

        yield return null;
    }

    IEnumerator DrawChunks()
    {
        foreach (KeyValuePair<string, Chunk> chunk in worldChunksDictionary)
        {
            if (chunk.Value.chunkStatus == ChunkStatus.DRAW)
            {
                chunk.Value.DrawChunk();
            }
            yield return null;
        }
    }

    // Use this for initialization
    void Start()
    {
        Vector3 playerPosition = player.transform.position;
        player.transform.position = new Vector3(playerPosition.x,
            Utils.GenerateHeight(playerPosition.x, playerPosition.z) + 1,
            playerPosition.z);

        player.SetActive(false);

        firstBuild = true;
        worldChunksDictionary = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;

        // Build starting chunk
        BuildChunkAt((int)(player.transform.position.x / chunkSize),
            (int)(player.transform.position.y / chunkSize),
            (int)(player.transform.position.z / chunkSize));

        for (int playerRadius = -radius; playerRadius <= radius; playerRadius++)
        {
            BuildChunkAt((int)(player.transform.position.x / chunkSize),
            (int)(player.transform.position.y / chunkSize) + playerRadius,
            (int)(player.transform.position.z / chunkSize) + playerRadius);
        }

        // Draw starting chunk
        StartCoroutine(DrawChunks());

        //create a bigger world
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.activeSelf)
        {
            player.SetActive(true);
            firstBuild = false;
        }

        //StartCoroutine(DrawChunks());
    }
}
