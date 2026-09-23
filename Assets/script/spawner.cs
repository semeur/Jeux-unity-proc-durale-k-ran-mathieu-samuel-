using UnityEngine;

public class spawner : MonoBehaviour
{
    //pour ajouter les differente partie des bloc
    public GameObject cube_entier;


    //pour generer le choix de biome
    //public int radom_biome = 0;




    void Start()
    {
        //va réaliser l'action a l'intérieur en boucle temps que x et z ne sont pas = a 100 , donc il va crée 400 bloc au total
        for (float x = 0f; x < 400; x++)
        {
            for (float z = 0f; z < 400; z++)
            {
                //obtenir les info sur la "couleur " des compossant du bloc


                //lance choix du biome au hasard parmis 3 option , 0 = plaine || 1 = desert || 2 = montagne
                int radom_biome = Random.Range(0, 3);
                
                if (radom_biome == 0)//faire apparaitre la partie haute de l'herbe et la teindre en vert et faire apparaitre la partie basse de l'herbe et la teindre en marron
                { 
                    
                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 3;
                    GameObject bloc = Instantiate(cube_entier, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                    bloc.GetComponent<bloc>().changer_en_hebe();

                }

                if (radom_biome == 1)//faire disparaitre la partie haute et la teindre en jaune au cas ou et faire apparaitre la partie basse de l'herbe et la teindre en jaune
                {

                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * -10;
                    Instantiate(cube_entier, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                    bloc.GetComponent<bloc>().changer_en_sable();

                }

                if (radom_biome == 2)//faire disparaitre la partie haute et la teindre en gris au cas ou et faire apparaitre la partie basse de l'herbe et la teindre en gris
                {

                    float PerlinY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 50;
                    Instantiate(cube_entier, new Vector3(z * 2, PerlinY, x * 2), Quaternion.identity);
                    bloc.GetComponent<bloc>().changer_en_roche();

                }

            }
        }
    }
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
  
