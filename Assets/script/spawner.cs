using UnityEngine;
 
public class spawner : MonoBehaviour
{
    public GameObject cube_haut ;
    public GameObject cube_bas ;
    public int radom_biome = Random.Range(0, 10);
    public int biome;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        radom_biome = Random.Range(0, 3);
        // 1 = plaine || 2 = desert || 3 = montagne
        if (radom_biome == 0)
        {


        }


        for (float x = 0f; x < 10000f; x++)
        {
            for (float z = 0f; z < 10000f; z++)
            {
                float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;
                Instantiate(cube_haut, new Vector3(z * 0.5f, PerlinY, x * 0.5f), Quaternion.identity);
                Instantiate(cube_bas, new Vector3((z * 0.5f), (PerlinY - 5f)  , (x * 0.5f) ), Quaternion.identity);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
