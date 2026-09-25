using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Génération_laby : MonoBehaviour
{
    [SerializeField] Controlbase prefab;
    public GameObject monstre;

    private void Start()
    {
        StartCoroutine(Gene_laby(new Vector2Int(15, 15)));
    }


   
    IEnumerator Gene_laby(Vector2Int taille)
    {
        // liste des nodes
        List<Controlbase> nodes = new List<Controlbase>();
        int araignee = 3;

        //remplit la liste
        for (int x = 0; x < taille.x; x++)
        {
            for (int y = 0; y < taille.y; y++)
            {
                Vector3 nod_pos = new Vector3(x * 50 - 350, 0, y * 50 - 350);
                Controlbase new_node = Instantiate(prefab, nod_pos, Quaternion.identity);
                if (Random.Range(0, 10)==0 && araignee > 0 && nod_pos.x != 0 && nod_pos.z != 0)
                {
                    Instantiate(monstre, new Vector3(nod_pos.x, 3, nod_pos.z), Quaternion.identity);
                    araignee -= 1;
                }
                nodes.Add(new_node);

                yield return null;
            }
        }

        // liste du chemin actuel et des chemins déjà visitée
        List<Controlbase> chemin_actuel = new List<Controlbase>();
        List<Controlbase> complete_nodes = new List<Controlbase>();

        chemin_actuel.Add(nodes[Random.Range(0, nodes.Count)]);
        chemin_actuel[0].set_state(state.Actuel);

        //boucle de la création du laby
        while (complete_nodes.Count < nodes.Count)
        {
            List<int> pro_node_poss = new List<int>();
            List<int> direc_poss = new List<int>();

            int actuel_index = nodes.IndexOf(chemin_actuel[chemin_actuel.Count - 1]);
            int actuel_node_x = actuel_index / taille.y;
            int actuel_node_y = actuel_index % taille.y;


            //Est
            if (actuel_node_x < taille.x - 1)
            {
                if (!complete_nodes.Contains(nodes[actuel_index + taille.y]) && !chemin_actuel.Contains(nodes[actuel_index + taille.y]))
                {
                    direc_poss.Add(3);
                    pro_node_poss.Add(actuel_index + taille.y);
                }

            }

            //Ouest
            if (actuel_node_x > 0)
            {
                if (!complete_nodes.Contains(nodes[actuel_index - taille.y]) && !chemin_actuel.Contains(nodes[actuel_index - taille.y]))
                {
                    direc_poss.Add(4);
                    pro_node_poss.Add(actuel_index - taille.y);
                }
            }


            //Nord
            if (actuel_node_y < taille.y - 1)
            {
                if (!complete_nodes.Contains(nodes[actuel_index + 1]) && !chemin_actuel.Contains(nodes[actuel_index + 1]))
                {
                    direc_poss.Add(1);
                    pro_node_poss.Add(actuel_index + 1);
                }
            }

            //Sud
            if (actuel_node_y > 0)
            {
                if (!complete_nodes.Contains(nodes[actuel_index - 1]) && !chemin_actuel.Contains(nodes[actuel_index - 1]))
                {
                    direc_poss.Add(2);
                    pro_node_poss.Add(actuel_index - 1);
                }
            }

            //si il y a un chemin disponible  on en choisit un
            if (direc_poss.Count > 0)
            {
                int direct_choisi = Random.Range(0, direc_poss.Count);
                Controlbase node_choisi = nodes[pro_node_poss[direct_choisi]];

                //on avance vers le node choisit en effçant les murs sur le chemin
                switch (direc_poss[direct_choisi])
                {
                    case 1:
                        node_choisi.Remove_wall(1);
                        chemin_actuel[chemin_actuel.Count - 1].Remove_wall(0);
                        break;
                    case 2:
                        node_choisi.Remove_wall(0);
                        chemin_actuel[chemin_actuel.Count - 1].Remove_wall(1);
                        break;
                    case 3:
                        node_choisi.Remove_wall(3);
                        chemin_actuel[chemin_actuel.Count - 1].Remove_wall(2);
                        break;
                    case 4:
                        node_choisi.Remove_wall(2);
                        chemin_actuel[chemin_actuel.Count - 1].Remove_wall(3);
                        break;
                }

                chemin_actuel.Add(node_choisi);
                node_choisi.set_state(state.Actuel);
            }

            else
            {
                //remplit les chemins complets
                complete_nodes.Add(chemin_actuel[chemin_actuel.Count - 1]);
                chemin_actuel[chemin_actuel.Count - 1].set_state(state.Complete);
                chemin_actuel.RemoveAt(chemin_actuel.Count - 1);
            }
            yield return null;
        }
        

        //Ouvertur de la sortie

        int h_b_g_d = Random.Range(0, 2);

        //nord ou sud aléatoire
        int h_b = 0;

        //ouest ou est
        int g_d = 0;

        //haut bas ou gauche droite
        if (h_b_g_d == 0)
        {
            h_b = Random.Range(0, 2);
            g_d = Random.Range(0, taille.x);

            switch (h_b)
            {
                case 0:
                    nodes[g_d * 10].Remove_wall(1);
                    break;

                case 1:
                    nodes[g_d * 10 + 9].Remove_wall(0);
                    break;

            }

        }
        if (h_b_g_d == 1)
        {
            h_b = Random.Range(0, taille.y);
            g_d = Random.Range(0, 2);

            switch (g_d)
            {
                case 0:
                    nodes[h_b].Remove_wall(3);
                    break;

                case 1:
                    nodes[90 + h_b].Remove_wall(2);
                    break;
            }  
        }
    }
}

