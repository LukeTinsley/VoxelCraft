using System.Collections;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    private Transform world;
    public GameObject block;
    private int width = 20;
    private int height = 20;
    private int depth = 20;

    public IEnumerator BuildWorld()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    GameObject cube = GameObject.Instantiate(block, position, Quaternion.identity);
                    cube.name = x + "_" + y + "_" + z;
                    cube.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
                    cube.transform.parent = world.transform;
                }
                yield return null;
            }
        }
    }

    void Awake()
    {
        world = GameObject.FindGameObjectWithTag("cubeParentTransform").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildWorld());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
