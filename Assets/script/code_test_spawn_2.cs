using System.Collections.Generic;
using UnityEngine;

public class code_chatgpt : MonoBehaviour
{
    // Les 3 prefabs disponibles
    public GameObject cube_plaine;
    public GameObject cube_desert;
    public GameObject cube_montagne;

    // Règles des biomes
    List<int>[] regles = new List<int>[3];

    // Taille de la grille
    [SerializeField] int taille = 100;

    // Une case de la grille
    private List<int>[,] grille;

    // Nombre minimum de chaque biome
    [SerializeField] int nb_min_plaine = 10;
    [SerializeField] int nb_min_desert = 10;
    [SerializeField] int nb_min_montagne = 5;

    // Nombre maximum de chaque biome
    [SerializeField] int nb_max_plaine = 100;
    [SerializeField] int nb_max_desert = 20;
    [SerializeField] int nb_max_montagne = 20;

    // Poids donné au même biome qu'une case voisine
    [SerializeField] float poidsMemeBiome = 1.5f; 

    // Nombre de blocs actuellement créés pour chaque biome
    int[] nombreBiomes = new int[3];


    void Start()
    {
        // Définir la taille de la grille
        grille = new List<int>[taille, taille];

        // Définit ce qui peut spawn à côté de quoi
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


        // Crée une sélection de possibilités de bloc
        while (true)
        {
            Vector2Int caseChoisie = trouver_case_moins_possibilites();

            // Pour arrêter de créer
            if (caseChoisie.x == -1)
                break;


            // Choisir les biomes parmi les possibilités
            List<int> possibilites = grille[caseChoisie.x, caseChoisie.y];

            int biome = choisir_biome(
                caseChoisie.x,
                caseChoisie.y,
                possibilites
            );


            // La case est choisie
            grille[caseChoisie.x, caseChoisie.y] = new List<int> { biome };


            // Ajouter le biome au compteur
            nombreBiomes[biome]++;


            // Dit le type de classe aux autres blocs
            propager(caseChoisie.x, caseChoisie.y);
        }


        // Appelle la fonction pour créer les blocs
        creer_monde();
    }


    // Cherche la case qui a le moins de possibilités
    Vector2Int trouver_case_moins_possibilites()
    {
        int minimum = 999;
        Vector2Int meilleure_case = new Vector2Int(-1, -1);

        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                int nombre = grille[x, z].Count;

                // Si une case a 1 possibilité, elle est déjà terminée
                if (nombre > 1 && nombre < minimum)
                {
                    minimum = nombre;
                    meilleure_case = new Vector2Int(x, z);
                }
            }
        }

        return meilleure_case;
    }


    // Choisit le biome en utilisant les minimums, maximums et poids
    int choisir_biome(int x, int z, List<int> possibilites)
    {
        List<int> choix = new List<int>();


        // ------------------------------------------------
        // 1. Retirer les biomes qui ont atteint leur maximum
        // ------------------------------------------------

        foreach (int biome in possibilites)
        {
            if (nombreBiomes[biome] < obtenir_maximum(biome))
            {
                choix.Add(biome);
            }
        }


        // Si aucun biome n'est disponible à cause des maximums
        // on utilise les possibilités originales
        if (choix.Count == 0)
        {
            choix = new List<int>(possibilites);
        }


        // ------------------------------------------------
        // 2. Vérifier les biomes qui n'ont pas atteint
        //    leur minimum
        // ------------------------------------------------

        List<int> biomesSousMinimum = new List<int>();

        foreach (int biome in choix)
        {
            if (nombreBiomes[biome] < obtenir_minimum(biome))
            {
                biomesSousMinimum.Add(biome);
            }
        }


        // S'il y a des biomes sous leur minimum,
        // on donne la priorité à ceux-ci
        if (biomesSousMinimum.Count > 0)
        {
            choix = biomesSousMinimum;
        }


        // ------------------------------------------------
        // 3. Calculer les poids
        // ------------------------------------------------

        List<float> poids = new List<float>();

        foreach (int biome in choix)
        {
            float poidsBiome = 1f;


            // Gauche
            if (x > 0)
            {
                if (grille[x - 1, z].Count == 1 &&
                    grille[x - 1, z][0] == biome)
                {
                    poidsBiome *= poidsMemeBiome;
                }
            }


            // Droite
            if (x < taille - 1)
            {
                if (grille[x + 1, z].Count == 1 &&
                    grille[x + 1, z][0] == biome)
                {
                    poidsBiome *= poidsMemeBiome;
                }
            }


            // Derrière
            if (z > 0)
            {
                if (grille[x, z - 1].Count == 1 &&
                    grille[x, z - 1][0] == biome)
                {
                    poidsBiome *= poidsMemeBiome;
                }
            }


            // Devant
            if (z < taille - 1)
            {
                if (grille[x, z + 1].Count == 1 &&
                    grille[x, z + 1][0] == biome)
                {
                    poidsBiome *= poidsMemeBiome;
                }
            }


            poids.Add(poidsBiome);
        }


        // ------------------------------------------------
        // 4. Choix aléatoire pondéré
        // ------------------------------------------------

        float totalPoids = 0f;

        foreach (float poidsBiome in poids)
        {
            totalPoids += poidsBiome;
        }


        float hasard = Random.Range(0f, totalPoids);


        for (int i = 0; i < choix.Count; i++)
        {
            hasard -= poids[i];

            if (hasard <= 0)
            {
                return choix[i];
            }
        }


        // Sécurité
        return choix[choix.Count - 1];
    }


    // Retourne le minimum du biome
    int obtenir_minimum(int biome)
    {
        if (biome == 0)
            return nb_min_plaine;

        if (biome == 1)
            return nb_min_desert;

        return nb_min_montagne;
    }


    // Retourne le maximum du biome
    int obtenir_maximum(int biome)
    {
        if (biome == 0)
            return nb_max_plaine;

        if (biome == 1)
            return nb_max_desert;

        return nb_max_montagne;
    }


    // Vérifie les autres possibilités et supprime
    // celles qui ne peuvent pas se lier
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


        // Derrière
        if (z > 0)
        {
            appliquer_regle(x, z - 1, biome);
        }


        // Devant
        if (z < taille - 1)
        {
            appliquer_regle(x, z + 1, biome);
        }
    }


    // Lie les biomes entre eux
    void appliquer_regle(int x, int z, int biomeVoisin)
    {
        List<int> possibilites = grille[x, z];

        // Récupère les biomes autorisés autour du biome actuel
        List<int> biomesOkspawn = regles[biomeVoisin];


        // Regarde les autres possibilités
        for (int p = possibilites.Count - 1; p >= 0; p--)
        {
            // Si le biome n'est pas compatible
            if (!biomesOkspawn.Contains(possibilites[p]))
            {
                // On le retire
                possibilites.RemoveAt(p);
            }
        }
    }


    // Crée les blocs
    void creer_monde()
    {
        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                int biome = grille[x, z][0];


                float PerlinY = Mathf.PerlinNoise(
                    x * 1f,
                    z * 1f
                );


                GameObject prefab = null;


                if (biome == 0)
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


                // Crée le bloc
                Instantiate(
                    prefab,
                    new Vector3(z * 2, PerlinY, x * 2),
                    Quaternion.identity
                );
            }
        }
    }
}