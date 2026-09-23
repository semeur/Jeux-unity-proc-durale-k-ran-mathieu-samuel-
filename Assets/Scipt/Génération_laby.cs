using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Génération_laby : MonoBehaviour
{
    public GameObject mur;
     

    private void Start()
    {
        List<GameObject> wall = new List<GameObject>();
        for (float x = 0; x < 1000; x+=94.5f)
        {
            for (float y = 0; y < 1000; y+=94.5f)
            {
                GameObject new_wall = Instantiate(mur, new Vector3(x, 0, y), Quaternion.identity);
                wall.Add(new_wall);
            }
        }

        List<GameObject> chemin_actuel = new List<GameObject>();
        List<GameObject> chemin_complete = new List<GameObject>();

        chemin_actuel.Add(wall[Random.Range(0, wall.Count)]);
        chemin_actuel[0].transform.Translate(Vector3.up * 100);

    }
}
