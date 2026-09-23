using UnityEngine;

public class Génération_laby : MonoBehaviour
{
    public GameObject mur;

    private void Start()
    {
        for (int i = 0; i < 1000; i+=110)
        {
            for (int j = 0; j < 1000; j+=110)
            {
                Instantiate(mur, new Vector3(i, 0, j), Quaternion.identity);
            }
        }
    }
}
