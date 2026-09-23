using UnityEngine;

public class Génération_laby : MonoBehaviour
{
    public GameObject mur;

    [SerializeField]
    private GameObject mur_e;

    [SerializeField]
    private GameObject mur_n;
    
    [SerializeField]
    private GameObject mur_s;

    [SerializeField]
    private GameObject non_visitee;

    public bool is_visited { get; private set; }

    public void Visit()
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
