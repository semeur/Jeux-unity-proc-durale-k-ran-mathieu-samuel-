using UnityEngine;

public class spawner : MonoBehaviour
{
    //pour ajouter les differente partie des bloc
    public GameObject cube_plaine;
    public GameObject cube_desert;
    public GameObject cube_montagne;
    [SerializeField] int radom_biome;

    //pour generer le choix de biome
    //public int radom_biome = 0;


    void Start()
    {
        int[,] Grille ;
        //va réaliser l'action a l'intérieur en boucle temps que x et z ne sont pas = a 100 , donc il va crée 400 bloc au total
        for (float x = 0f; x < 400; x++)
        {
            for (float z = 0f; z < 400; z++)
            {

                //lance choix du biome au hasard parmis 3 option , 0 = plaine || 1 = desert || 2 = montagne
                //radom_biome = Random.Range(0, 3);
                
                if (radom_biome == 0)//faire apparaitre la partie haute de l'herbe et la teindre en vert et faire apparaitre la partie basse de l'herbe et la teindre en marron
                { 
                  
                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;
                    Instantiate(cube_plaine, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                    //cube.GetComponent<bloc>().changer_en_hebe();
                
                }

                else if (radom_biome == 1)//faire disparaitre la partie haute et la teindre en jaune au cas ou et faire apparaitre la partie basse de l'herbe et la teindre en jaune
                {

                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * -10;
                    Instantiate(cube_desert, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                    //bloc.GetComponent<bloc>().changer_en_sable();

                }

                else if (radom_biome == 2)//faire disparaitre la partie haute et la teindre en gris au cas ou et faire apparaitre la partie basse de l'herbe et la teindre en gris
                {
                    
                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 50;
                    Instantiate(cube_montagne, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                    //bloc.GetComponent<bloc>().changer_en_roche();
                
                }
                
            }
        }
    }
}
