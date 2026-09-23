using UnityEngine;
 
public class spawner : MonoBehaviour
{
    //pour ajouter les differente partie des bloc
    public GameObject cube_entier ;
    public GameObject cube_haut ;
    public GameObject cube_bas ;

    //pour generer le choix de biome
    public int radom_biome = Random.Range(0, 10);
    //public int radom_biome = 0;
   
    //pour ajuter les different material crée par moi
    public Material herbematerial;
    public Material Terrematerial ;
    public Material sablematerial ;
    public Material rochematerial ;


    void Start()
    {
        /*
        float x = 0f; 
        if (x < 50) 
            x++ ;
        float z = 0f; if (z < 50)
            z++ ;
        float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;
        */

        //obtenir les info sur la "couleur " des compossant du bloc
        MeshRenderer cube_haut_renderer =cube_haut.GetComponent<MeshRenderer>();
        MeshRenderer cube_bas_renderer =cube_bas.GetComponent<MeshRenderer>();

        //lance le choix du hasard pour le biome
        //radom_biome = Random.Range(0, 3);
        // 0 = plaine || 1 = desert || 2 = montagne
        if (radom_biome == 0)//faire apparaitre la partie haute de l'herbe et la teindre en vert et faire apparaitre la partie basse de l'herbe et la teindre en marron
        {
            cube_haut.SetActive(true);
            cube_bas.SetActive(true);

            cube_haut_renderer.material.color = herbematerial.color;
            cube_bas_renderer.material.color = Terrematerial.color;

            for (float x = 0f; x < 50; x++)
            {
                for (float z = 0f; z < 50; z++)
                {
                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;
                    Instantiate(cube_entier, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                }
            }

        }
        
        if (radom_biome == 1)//faire disparaitre la partie haute et la teindre en jaune au cas ou et faire apparaitre la partie basse de l'herbe et la teindre en jaune
        {
            cube_haut.SetActive(false);
            cube_bas.SetActive(true);

            cube_haut_renderer.material.color = sablematerial.color;
            cube_bas_renderer.material.color = sablematerial.color;

            for (float x = 0f; x < 50; x++)
            {
                for (float z = 0f; z < 50; z++)
                {
                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) *-10 ;
                    Instantiate(cube_entier, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                }
            }
            // PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;  


        }

        if (radom_biome == 2)//faire disparaitre la partie haute et la teindre en gris au cas ou et faire apparaitre la partie basse de l'herbe et la teindre en gris
        {
            cube_haut.SetActive(false);
            cube_bas.SetActive(true);

            cube_haut_renderer.material.color = rochematerial.color;
            cube_bas_renderer.material.color = rochematerial.color;

            for (float x = 0f; x < 50; x++)
            {
                for (float z = 0f; z < 50; z++)
                {
                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 50;
                    Instantiate(cube_entier, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                }
            }

            //PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;  


        }

        /*
        for ( x = 0f; x < 50; x++)
        {
            for ( z = 0f; z < 50; z++)
            {
                //float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;
                Instantiate(cube_entier, new Vector3(z *2 , PerlinY, x* 2 ), Quaternion.identity);
            }
        }
        */
    }

    // Update is called once per frame || ne sert a rien pour le moment
    /*
    void biome_chox()
    {
        haut player = other.GetComponent<haut>();

        // 1 = plaine || 2 = desert || 3 = montagne
        if (radom_biome == 0)
        {
            SetActive

        }
    }
    */
        }
