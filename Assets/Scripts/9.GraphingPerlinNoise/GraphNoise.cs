using UnityEngine;


public class GraphNoise : MonoBehaviour
{
    float t = 0;
    float inc = 0.01f;

    float t2 = 0;
    float inc2 = 0.01f;

    float Map(float min, float max, float omin, float omax, float value)
    {
        return Mathf.Lerp(min, max, Mathf.InverseLerp(omin, omax, value));
    }

    void Update()
    {
        t += inc;
        float n = fBM(t, 6, 0.8f);
        Grapher.Log(n, "Brownian Motion", Color.red);

        t += inc;
        float n2 = Mathf.PerlinNoise(t, 1);
        Grapher.Log(n2, "Perlin1", Color.yellow);

        t2 += inc2;
        float n3 = Mathf.PerlinNoise(t2, 1);
        Grapher.Log(n3, "Perlin2", Color.green);

        float n4 = (n2 + n3) / 2.0f;
        Grapher.Log(n4, "Total", Color.blue);
    }

    float fBM(float t, int octaves, float persistence)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(t * frequency, 1) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }

        return total / maxValue;
    }
}
