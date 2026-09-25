using System.Collections.Generic;
using UnityEngine;

public class code_test : MonoBehaviour
{
    // ============================================================
    // PREFABS
    // ============================================================

    public GameObject cube_plaine;
    public GameObject cube_desert;
    public GameObject cube_montagne;


    // ============================================================
    // REGLES DES BIOMES
    // ============================================================

    // 0 = Plaine
    // 1 = Désert
    // 2 = Montagne

    List<int>[] regles = new List<int>[3];


    // ============================================================
    // TAILLE DE LA GRILLE
    // ============================================================

    [SerializeField] int taille = 50;

    private List<int>[,] grille;


    // ============================================================
    // MINIMUM DE CHAQUE BIOME
    // ============================================================

    [SerializeField] int nb_min_plaine = 10;
    [SerializeField] int nb_min_desert = 10;
    [SerializeField] int nb_min_montagne = 5;


    // ============================================================
    // MAXIMUM DE CHAQUE BIOME
    // ============================================================

    [SerializeField] int nb_max_plaine = 1000;
    [SerializeField] int nb_max_desert = 1000;
    [SerializeField] int nb_max_montagne = 500;


    // ============================================================
    // POIDS DU MEME BIOME
    // ============================================================

    // Plus cette valeur est grande,
    // plus un biome a tendance à continuer
    // lorsqu'il est entouré du même biome.

    [SerializeField] float poidsMemeBiome = 1.5f;


    // ============================================================
    // EVITE LES PETITS ILOTS
    // ============================================================

    // Nombre minimum de voisins nécessaires
    // pour forcer le biome majoritaire.

    [SerializeField] int voisinsMinimumPourForcer = 2;


    // ============================================================
    // TRANSITION DE HAUTEUR ENTRE LES BIOMES
    // ============================================================

    // 0 = changement brutal
    // 1 = transition très forte

    [SerializeField]
    [Range(0f, 1f)]
    float douceurTransition = 0.3f;


    // ============================================================
    // DISTANCE DE TRANSITION
    // ============================================================

    // Nombre de cases utilisées pour adoucir
    // la différence de hauteur entre deux biomes.

    [SerializeField] int distanceTransition = 3;


    // ============================================================
    // HAUTEUR DES BIOMES
    // ============================================================

    [SerializeField] float hauteurPlaine = 2.5f;
    [SerializeField] float hauteurDesert = 1.5f;
    [SerializeField] float hauteurMontagne = 6.8f;


    // ============================================================
    // VARIATION DU TERRAIN
    // ============================================================

    [SerializeField] float taillePerlin = 0.1f;


    // ============================================================
    // NOMBRE DE BLOCS ACTUELLEMENT CREES
    // ============================================================

    int[] nombreBiomes = new int[3];


    // ============================================================
    // START
    // ============================================================

    void Start()
    {
        // Définir la taille de la grille
        grille = new List<int>[taille, taille];


        // --------------------------------------------------------
        // REGLES DE VOISINAGE
        // --------------------------------------------------------

        regles[0] = new List<int> { 0, 1, 2 }; // Plaine
        regles[1] = new List<int> { 0, 1 };    // Désert
        regles[2] = new List<int> { 0, 2 };    // Montagne


        // --------------------------------------------------------
        // INITIALISATION DE LA GRILLE
        // --------------------------------------------------------

        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                grille[x, z] = new List<int> { 0, 1, 2 };
            }
        }


        // --------------------------------------------------------
        // GENERATION
        // --------------------------------------------------------

        while (true)
        {
            Vector2Int caseChoisie = trouver_case_moins_possibilites();


            // Plus aucune case à choisir
            if (caseChoisie.x == -1)
                break;


            // Récupère les possibilités
            List<int> possibilites =
                grille[caseChoisie.x, caseChoisie.y];


            // Choisit le biome
            int biome = choisir_biome(
                caseChoisie.x,
                caseChoisie.y,
                possibilites
            );


            // La case devient ce biome
            grille[caseChoisie.x, caseChoisie.y] =
                new List<int> { biome };


            // Augmente le compteur
            nombreBiomes[biome]++;


            // Propagation des règles
            propager(
                caseChoisie.x,
                caseChoisie.y
            );
        }


        // --------------------------------------------------------
        // CREATION DU MONDE
        // --------------------------------------------------------

        creer_monde();
    }


    // ============================================================
    // CHERCHE LA CASE AVEC LE MOINS DE POSSIBILITES
    // ============================================================

    Vector2Int trouver_case_moins_possibilites()
    {
        int minimum = 999;

        Vector2Int meilleure_case =
            new Vector2Int(-1, -1);


        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                int nombre =
                    grille[x, z].Count;


                // Une case avec 1 possibilité
                // est déjà terminée.

                if (nombre > 1 && nombre < minimum)
                {
                    minimum = nombre;

                    meilleure_case =
                        new Vector2Int(x, z);
                }
            }
        }


        return meilleure_case;
    }


    // ============================================================
    // CHOISIT LE BIOME
    // ============================================================

    int choisir_biome(
        int x,
        int z,
        List<int> possibilites)
    {
        // --------------------------------------------------------
        // 1. RETIRE LES BIOMES AYANT ATTEINT LEUR MAXIMUM
        // --------------------------------------------------------

        List<int> choix =
            new List<int>();


        foreach (int biome in possibilites)
        {
            if (nombreBiomes[biome] <
                obtenir_maximum(biome))
            {
                choix.Add(biome);
            }
        }


        // Sécurité :
        // si tous les biomes ont atteint leur maximum,
        // on garde les possibilités originales.

        if (choix.Count == 0)
        {
            choix =
                new List<int>(possibilites);
        }


        // --------------------------------------------------------
        // 2. CHERCHE LES BIOMES SOUS LEUR MINIMUM
        // --------------------------------------------------------

        List<int> biomesSousMinimum =
            new List<int>();


        foreach (int biome in choix)
        {
            if (nombreBiomes[biome] <
                obtenir_minimum(biome))
            {
                biomesSousMinimum.Add(biome);
            }
        }


        // Si un biome est encore sous son minimum,
        // on lui donne la priorité.

        if (biomesSousMinimum.Count > 0)
        {
            choix =
                biomesSousMinimum;
        }


        // --------------------------------------------------------
        // 3. CHERCHE LE BIOME DOMINANT AUTOUR
        // --------------------------------------------------------

        int biomeDominant =
            trouver_biome_dominant_autour(
                x,
                z,
                choix
            );


        // --------------------------------------------------------
        // 4. EVITE LES PETITS ILOTS
        // --------------------------------------------------------

        if (biomeDominant != -1)
        {
            int nombreVoisins =
                compter_voisins_biome(
                    x,
                    z,
                    biomeDominant
                );


            // Si suffisamment de voisins sont du même biome,
            // on force ce biome.

            if (nombreVoisins >=
                voisinsMinimumPourForcer)
            {
                if (nombreBiomes[biomeDominant] <
                    obtenir_maximum(biomeDominant))
                {
                    return biomeDominant;
                }
            }
        }


        // --------------------------------------------------------
        // 5. CALCUL DES POIDS
        // --------------------------------------------------------

        List<float> poids =
            new List<float>();


        foreach (int biome in choix)
        {
            float poidsBiome = 1f;


            // ----------------------------------------------------
            // Gauche
            // ----------------------------------------------------

            if (x > 0)
            {
                if (grille[x - 1, z].Count == 1 &&
                    grille[x - 1, z][0] == biome)
                {
                    poidsBiome *=
                        poidsMemeBiome;
                }
            }


            // ----------------------------------------------------
            // Droite
            // ----------------------------------------------------

            if (x < taille - 1)
            {
                if (grille[x + 1, z].Count == 1 &&
                    grille[x + 1, z][0] == biome)
                {
                    poidsBiome *=
                        poidsMemeBiome;
                }
            }


            // ----------------------------------------------------
            // Derrière
            // ----------------------------------------------------

            if (z > 0)
            {
                if (grille[x, z - 1].Count == 1 &&
                    grille[x, z - 1][0] == biome)
                {
                    poidsBiome *=
                        poidsMemeBiome;
                }
            }


            // ----------------------------------------------------
            // Devant
            // ----------------------------------------------------

            if (z < taille - 1)
            {
                if (grille[x, z + 1].Count == 1 &&
                    grille[x, z + 1][0] == biome)
                {
                    poidsBiome *=
                        poidsMemeBiome;
                }
            }


            poids.Add(poidsBiome);
        }


        // --------------------------------------------------------
        // 6. CHOIX ALEATOIRE PONDERE
        // --------------------------------------------------------

        float totalPoids = 0f;


        foreach (float poidsBiome in poids)
        {
            totalPoids += poidsBiome;
        }


        float hasard =
            Random.Range(
                0f,
                totalPoids
            );


        for (int i = 0;
             i < choix.Count;
             i++)
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


    // ============================================================
    // TROUVE LE BIOME MAJORITAIRE AUTOUR
    // ============================================================

    int trouver_biome_dominant_autour(
        int x,
        int z,
        List<int> possibilites)
    {
        int[] compteur =
            new int[3];


        // Gauche
        if (x > 0 &&
            grille[x - 1, z].Count == 1)
        {
            int biome =
                grille[x - 1, z][0];

            if (possibilites.Contains(biome))
                compteur[biome]++;
        }


        // Droite
        if (x < taille - 1 &&
            grille[x + 1, z].Count == 1)
        {
            int biome =
                grille[x + 1, z][0];

            if (possibilites.Contains(biome))
                compteur[biome]++;
        }


        // Derrière
        if (z > 0 &&
            grille[x, z - 1].Count == 1)
        {
            int biome =
                grille[x, z - 1][0];

            if (possibilites.Contains(biome))
                compteur[biome]++;
        }


        // Devant
        if (z < taille - 1 &&
            grille[x, z + 1].Count == 1)
        {
            int biome =
                grille[x, z + 1][0];

            if (possibilites.Contains(biome))
                compteur[biome]++;
        }


        // Recherche du plus présent

        int biomeDominant = -1;
        int maximum = 0;


        for (int i = 0; i < 3; i++)
        {
            if (compteur[i] > maximum)
            {
                maximum =
                    compteur[i];

                biomeDominant =
                    i;
            }
        }


        return biomeDominant;
    }


    // ============================================================
    // COMPTE LES VOISINS D'UN BIOME
    // ============================================================

    int compter_voisins_biome(
        int x,
        int z,
        int biomeRecherche)
    {
        int nombre = 0;


        // Gauche
        if (x > 0 &&
            grille[x - 1, z].Count == 1 &&
            grille[x - 1, z][0] == biomeRecherche)
        {
            nombre++;
        }


        // Droite
        if (x < taille - 1 &&
            grille[x + 1, z].Count == 1 &&
            grille[x + 1, z][0] == biomeRecherche)
        {
            nombre++;
        }


        // Derrière
        if (z > 0 &&
            grille[x, z - 1].Count == 1 &&
            grille[x, z - 1][0] == biomeRecherche)
        {
            nombre++;
        }


        // Devant
        if (z < taille - 1 &&
            grille[x, z + 1].Count == 1 &&
            grille[x, z + 1][0] == biomeRecherche)
        {
            nombre++;
        }


        return nombre;
    }


    // ============================================================
    // MINIMUM DU BIOME
    // ============================================================

    int obtenir_minimum(int biome)
    {
        if (biome == 0)
            return nb_min_plaine;

        if (biome == 1)
            return nb_min_desert;

        return nb_min_montagne;
    }


    // ============================================================
    // MAXIMUM DU BIOME
    // ============================================================

    int obtenir_maximum(int biome)
    {
        if (biome == 0)
            return nb_max_plaine;

        if (biome == 1)
            return nb_max_desert;

        return nb_max_montagne;
    }


    // ============================================================
    // PROPAGATION
    // ============================================================

    void propager(int x, int z)
    {
        int biome =
            grille[x, z][0];


        // Gauche
        if (x > 0)
        {
            appliquer_regle(
                x - 1,
                z,
                biome
            );
        }


        // Droite
        if (x < taille - 1)
        {
            appliquer_regle(
                x + 1,
                z,
                biome
            );
        }


        // Derrière
        if (z > 0)
        {
            appliquer_regle(
                x,
                z - 1,
                biome
            );
        }


        // Devant
        if (z < taille - 1)
        {
            appliquer_regle(
                x,
                z + 1,
                biome
            );
        }
    }


    // ============================================================
    // APPLIQUE UNE REGLE DE VOISINAGE
    // ============================================================

    void appliquer_regle(
        int x,
        int z,
        int biomeVoisin)
    {
        List<int> possibilites =
            grille[x, z];


        List<int> biomesOkspawn =
            regles[biomeVoisin];


        for (int p =
             possibilites.Count - 1;
             p >= 0;
             p--)
        {
            if (!biomesOkspawn.Contains(
                possibilites[p]))
            {
                possibilites.RemoveAt(p);
            }
        }
    }


    // ============================================================
    // HAUTEUR NORMALE D'UN BIOME
    // ============================================================

    float obtenir_hauteur_biome(
        int x,
        int z,
        int biome)
    {
        float bruit =
            Mathf.PerlinNoise(
                x * taillePerlin,
                z * taillePerlin
            );


        if (biome == 0)
        {
            // Plaine
            return bruit * hauteurPlaine;
        }


        if (biome == 1)
        {
            // Désert
            return bruit * hauteurDesert;
        }


        // Montagne
        return bruit * hauteurMontagne;
    }


    // ============================================================
    // DISTANCE PAR RAPPORT A UN AUTRE BIOME
    // ============================================================

    int trouver_distance_biome_different(
        int x,
        int z,
        int biome)
    {
        int meilleureDistance =
            distanceTransition + 1;


        // On cherche autour de la case
        // dans la zone de transition.

        for (int dx =
             -distanceTransition;
             dx <= distanceTransition;
             dx++)
        {
            for (int dz =
                 -distanceTransition;
                 dz <= distanceTransition;
                 dz++)
            {
                if (dx == 0 && dz == 0)
                    continue;


                int nouveauX =
                    x + dx;

                int nouveauZ =
                    z + dz;


                // Hors de la grille
                if (nouveauX < 0 ||
                    nouveauX >= taille ||
                    nouveauZ < 0 ||
                    nouveauZ >= taille)
                {
                    continue;
                }


                // La case doit être terminée
                if (grille[nouveauX, nouveauZ].Count != 1)
                    continue;


                int biomeVoisin =
                    grille[nouveauX, nouveauZ][0];


                // On cherche uniquement un autre biome
                if (biomeVoisin != biome)
                {
                    int distance =
                        Mathf.Abs(dx) +
                        Mathf.Abs(dz);


                    if (distance < meilleureDistance)
                    {
                        meilleureDistance =
                            distance;
                    }
                }
            }
        }


        return meilleureDistance;
    }


    // ============================================================
    // HAUTEUR FINALE
    // ============================================================

    float obtenir_hauteur(
        int x,
        int z)
    {
        int biome =
            grille[x, z][0];


        // Hauteur normale
        float hauteur =
            obtenir_hauteur_biome(
                x,
                z,
                biome
            );


        // Cherche à quelle distance se trouve
        // un biome différent.

        int distance =
            trouver_distance_biome_different(
                x,
                z,
                biome
            );


        // Aucun autre biome à proximité
        if (distance >
            distanceTransition)
        {
            return hauteur;
        }


        // --------------------------------------------------------
        // FORCE DE LA TRANSITION
        // --------------------------------------------------------

        float transition =
            1f -
            ((float)distance /
            (distanceTransition + 1));


        // La transition est limitée
        // par la douceur choisie.

        transition *=
            douceurTransition;


        // --------------------------------------------------------
        // CHERCHE UNE HAUTEUR MOYENNE DES BIOMES VOISINS
        // --------------------------------------------------------

        float somme =
            0f;

        int nombre =
            0;


        for (int dx =
             -distanceTransition;
             dx <= distanceTransition;
             dx++)
        {
            for (int dz =
                 -distanceTransition;
                 dz <= distanceTransition;
                 dz++)
            {
                if (dx == 0 && dz == 0)
                    continue;


                int nouveauX =
                    x + dx;

                int nouveauZ =
                    z + dz;


                if (nouveauX < 0 ||
                    nouveauX >= taille ||
                    nouveauZ < 0 ||
                    nouveauZ >= taille)
                {
                    continue;
                }


                if (grille[nouveauX, nouveauZ].Count != 1)
                    continue;


                int biomeVoisin =
                    grille[nouveauX, nouveauZ][0];


                if (biomeVoisin != biome)
                {
                    somme +=
                        obtenir_hauteur_biome(
                            nouveauX,
                            nouveauZ,
                            biomeVoisin
                        );

                    nombre++;
                }
            }
        }


        if (nombre > 0)
        {
            float moyenne =
                somme / nombre;


            // Mélange entre la hauteur normale
            // et celle du biome voisin.

            hauteur =
                Mathf.Lerp(
                    hauteur,
                    moyenne,
                    transition
                );
        }


        return hauteur;
    }


    // ============================================================
    // CREATION DU MONDE
    // ============================================================

    void creer_monde()
    {
        for (int x = 0; x < taille; x++)
        {
            for (int z = 0; z < taille; z++)
            {
                int biome =
                    grille[x, z][0];


                // Calcule la hauteur
                // avec les transitions.

                float PerlinY =
                    obtenir_hauteur(
                        x,
                        z
                    );


                GameObject prefab =
                    null;


                // ------------------------------------------------
                // PLAINE
                // ------------------------------------------------

                if (biome == 0)
                {
                    prefab =
                        cube_plaine;
                }


                // ------------------------------------------------
                // DESERT
                // ------------------------------------------------

                else if (biome == 1)
                {
                    prefab =
                        cube_desert;
                }


                // ------------------------------------------------
                // MONTAGNE
                // ------------------------------------------------

                else if (biome == 2)
                {
                    prefab =
                        cube_montagne;
                }


                // ------------------------------------------------
                // CREATION DU BLOC
                // ------------------------------------------------

                Instantiate(
                    prefab,
                    new Vector3(
                        z * 2,
                        PerlinY,
                        x * 2
                    ),
                    Quaternion.identity
                );
            }
        }
    }
}