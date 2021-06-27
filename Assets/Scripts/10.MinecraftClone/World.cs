using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public GameObject player;
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static int worldSize = 16;
    public static int radius = 2;
    public static Dictionary<string, Chunk> worldChunksDictionary;
    public Slider loadingAmount;
    public Camera cam;
    public Button playButton;
    bool firstBuild = true;
    bool building = false;

    public static string BuildChunkName(Vector3 vector3)
    {
        return (int)vector3.x + "_" + (int)vector3.y + "_" + (int)vector3.z;
    }

    IEnumerator BuildWorld()
    {
        building = true;
        int posx = (int)Mathf.Floor(player.transform.position.x / chunkSize);
        int posz = (int)Mathf.Floor(player.transform.position.z / chunkSize);

        float totalChunks = (Mathf.Pow(radius * 2 + 1, 2) * columnHeight) * 2;
        int processCount = 0;

        for (int z = -radius; z <= radius; z++)
            for (int x = -radius; x <= radius; x++)
                for (int y = 0; y < columnHeight; y++)
                {
                    Vector3 chunkPosition = new Vector3((x + posx) * chunkSize, y * chunkSize, (posz + z) * chunkSize);
                    Chunk chunk;
                    string chunkName = BuildChunkName(chunkPosition);

                    if (worldChunksDictionary.TryGetValue(chunkName, out chunk))
                    {
                        chunk.chunkStatus = ChunkStatus.KEEP;
                        break;
                    }


                    chunk = new Chunk(chunkPosition, textureAtlas);
                    chunk.chunkObject.transform.parent = this.transform;
                    worldChunksDictionary.Add(chunk.chunkObject.name, chunk);

                    if (firstBuild)
                    {
                        processCount++;
                        loadingAmount.value = processCount / totalChunks * 100;
                    }
                    yield return null;
                }

        foreach (KeyValuePair<string, Chunk> chunk in worldChunksDictionary)
        {
            if (chunk.Value.chunkStatus == ChunkStatus.DRAW)
            {
                chunk.Value.DrawChunk();
                chunk.Value.chunkStatus = ChunkStatus.KEEP;
            }

            // Delete old chunks here
            chunk.Value.chunkStatus = ChunkStatus.DONE;
            if (firstBuild)
            {
                processCount++;
                loadingAmount.value = processCount / totalChunks * 100;
            }
            yield return null;
        }

        if (firstBuild)
        {
            player.SetActive(true);
            loadingAmount.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            firstBuild = false;
        }
        building = false;
    }

    public void StartBuild()
    {
        StartCoroutine(BuildWorld());
    }

    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
        worldChunksDictionary = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!building && !firstBuild)
            StartCoroutine(BuildWorld());
    }
}
