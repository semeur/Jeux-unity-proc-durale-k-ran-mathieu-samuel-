using System.Collections.Generic;
using UnityEngine;

public class code_final_spawner : MonoBehaviour
{
    // Les 3 prefabs disponibles
    public GameObject cube_plaine;
    public GameObject cube_desert;
    public GameObject cube_montagne;



    //normalement crée une régle des 
    List<int>[] regles = new List<int>[3];

    // Taille de la grille
    [SerializeField] int taille = 50;

    // Une case de la grille
    private List<int>[,] grille;

    //ce système pas terminer devait permetre de definir un nombre minimum et maximum de cube par chunk
    [SerializeField] int nb_min_plaine = 10;
    [SerializeField] int nb_min_desert = 10;
    [SerializeField] int nb_min_monragne = 5;

    [SerializeField] int nb_max_plaine = 10;
    [SerializeField] int nb_max_desert = 10;
    [SerializeField] int nb_max_monragne = 5;

    //ce système pas terminer devait permetre de d'augmenter le taux de chance ,
    //pour avoir le même bloc cote a cote et donc il permettrais d'agrandir les biome jusqu'au nombre max de bloc
    //et enlèverais en partie les block de biome different qui spawn dans une zonne uniquement fait d'un seul bloc
    [SerializeField] float augement_meme_biome = 1.5f;
    void Start()
    {
        //definir la taille de la grille a la taille definit dans l'édteur
        grille = new List<int>[taille, taille];

        //definit ce qui peut spawn a coter de quoi mais bug 
        regles[0] = new List<int> { 0, 1, 2 }; // Plaine
        regles[1] = new List<int> { 0, 1 };    // Désert
        regles[2] = new List<int> { 0, 2 };    // Montagne


        // Chaque case peut commencer avec les 3 biomes
        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                grille[x, z] = new List<int> { 0, 1, 2 };
            }
        }

        // crée une selection de posibiliter de bloc
        while (true)
        {
            Vector2Int caseChoisie = trouver_case_moins_possibilites();

            // pour arrter de crée
            if (caseChoisie.x == -1)
                break;

            // choisir les biome parmis les possibilité
            List<int> possibilites = grille[caseChoisie.x, caseChoisie.y];

            int biome = possibilites[Random.Range(0, possibilites.Count)];

            // la case est choisie
            grille[caseChoisie.x, caseChoisie.y] = new List<int> { biome };

            // dit le type de classe au autre bloc
            propager(caseChoisie.x, caseChoisie.y);
        }

        // appelle la fonct pour crée les bloc
        creer_monde();
    }


    // Cherche la case qui a le moin de possibilité
    Vector2Int trouver_case_moins_possibilites()
    {
        int minimum = 999;
        Vector2Int meilleure_case = new Vector2Int(-1, -1);

        //je ne sais plus a vrérifier
        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                int nombre = grille[x, z].Count;

                // si une case a 1 possibilité il est le meilleur
                if (nombre > 1 && nombre < minimum)
                {
                    minimum = nombre;
                    meilleure_case = new Vector2Int(x, z);
                }
            }
        }

        return meilleure_case;
    }


    // Vérifie les autre possibilité des joueur et suprime ceux qui ne peuvent pas se lier
    void propager(int x, int z)
    {
        int biome = grille[x, z][0];

        // Gauche
        if (x > 0)
        {
            appliquer_regle(x - 1, z, biome);
        }

        // Droite
        if (x < taille - 1)
        {
            appliquer_regle(x + 1, z, biome);
        }

        // derrière
        if (z > 0)
        {
            appliquer_regle(x, z - 1, biome);
        }

        // devant
        if (z < taille - 1)
        {
            appliquer_regle(x, z + 1, biome);
        }
    }


    // lier des biome entre eux
    void appliquer_regle(int x, int z, int biomeVoisin)
    {
        List<int> possibilites = grille[x, z];
        // Récupère les biomes autorisés autour du biome actuel
        List<int> biomesOkspawn = regles[biomeVoisin];

        // regarde les autre possibiliter
        for (int p = possibilites.Count - 1; p >= 0; p--)
        {
            // Si le biome il est pas compatible
            if (!biomesOkspawn.Contains(possibilites[p]))
            {
                // On le retire
                possibilites.RemoveAt(p);
            }
        }
    }



    // crée les bloc
    void creer_monde()
    {
        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                int biome = grille[x, z][0];

                float PerlinY = Mathf.PerlinNoise(
                    x * 0.1f,
                    z * 0.1f
                );

                GameObject prefab = null;

                if (biome == 0 )
                {
                    // Plaine
                    PerlinY *= 2.5f;
                    prefab = cube_plaine;
                }
                else if (biome == 1)
                {
                    // Désert
                    PerlinY *= -1.2f;
                    prefab = cube_desert;
                }
                else if (biome == 2)
                {
                    // Montagne
                    PerlinY *= 6.8f;
                    prefab = cube_montagne;
                }
                
                //crée les bloc
                Instantiate(

                    prefab,
                    new Vector3(z * 2, PerlinY, x * 2),
                    Quaternion.identity
                );
            }
        }
    }
}