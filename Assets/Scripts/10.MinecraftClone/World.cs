using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public Slider loadingAmount;
    public Camera cam;
    public GameObject player;
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static int radius = 3;
    public static Dictionary<string, Chunk> worldChunksDictionary;
    public static bool firstBuild = true;
    public Vector3 lastbuildPos;

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

    void BuildExtendedWorld(int x, int y, int z, int rad)
    {
        for (z = -rad; z <= rad; z++)
            for (x = -rad; x <= rad; x++)
                for (y = -rad; y <= rad; y++)
                {
                    Vector3 chunkPosition = new Vector3(lastbuildPos.x + (x * chunkSize),
                        lastbuildPos.y + y * chunkSize,
                        lastbuildPos.z + z * chunkSize);

                    string chunkName = BuildChunkName(chunkPosition);
                    Chunk chunk;

                    if (!worldChunksDictionary.TryGetValue(chunkName, out chunk))
                    {
                        if (Vector3.Distance(chunkPosition / 16, lastbuildPos / 16) <= rad)
                        {
                            BuildChunkAt((int)(player.transform.position.x / chunkSize) + x,
                            (int)(player.transform.position.y / chunkSize) + y,
                            (int)(player.transform.position.z / chunkSize) + z);
                        }
                    }
                }
        Debug.Log("Done Loading");
    }

    public void BuildNearPlayer()
    {
        //StopCoroutine("BuildRecursiveWorld");
        Debug.Log("BuildNearPlayer fired");
        BuildExtendedWorld((int)lastbuildPos.x / 16, (int)lastbuildPos.y / 16, (int)lastbuildPos.z / 16, radius);
    }

    IEnumerator DrawChunks()
    {
        int processCount = 0;
        foreach (KeyValuePair<string, Chunk> chunk in worldChunksDictionary)
        {
            if (chunk.Value.chunkStatus == ChunkStatus.DRAW)
            {
                chunk.Value.DrawChunk();
                chunk.Value.chunkStatus = ChunkStatus.DONE;
                if (firstBuild)
                {
                    processCount++;
                    loadingAmount.value = ((float)processCount / (float)worldChunksDictionary.Count) * 100;
                }
            }
            yield return null;
        }
        if (firstBuild)
        {
            loadingAmount.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
            firstBuild = false;
            player.SetActive(true);
        }
    }

    // Use this for initialization
    void Start()
    {
        player.SetActive(false);
        firstBuild = true;

        Vector3 playerPosition = player.transform.position;
        player.transform.position = new Vector3(playerPosition.x,
            Utils.GenerateHeight(playerPosition.x, playerPosition.z) + 1,
            playerPosition.z);

        lastbuildPos = player.transform.position;

        worldChunksDictionary = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;

        // Build starting chunk
        BuildChunkAt((int)(player.transform.position.x / chunkSize),
            (int)(player.transform.position.y / chunkSize),
            (int)(player.transform.position.z / chunkSize));

        //create a bigger world
        BuildExtendedWorld((int)(player.transform.position.x / chunkSize),
            (int)(player.transform.position.y / chunkSize),
            (int)(player.transform.position.z / chunkSize), radius);

        StartCoroutine(DrawChunks());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = lastbuildPos - player.transform.position;

        if (movement.magnitude > chunkSize)
        {
            lastbuildPos = player.transform.position;
            Debug.Log(lastbuildPos);
            //BuildNearPlayer();
        }

        StartCoroutine(DrawChunks());
    }
}
