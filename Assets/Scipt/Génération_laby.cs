using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Génération_laby : MonoBehaviour
{
    [SerializeField]
    private GameObject mur_o;

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
        is_visited = true;
        non_visitee.SetActive(false);
    }

    public void erase_mur_o()
    {
        mur_o.SetActive(false);
    }
    public void erase_mur_e()
    {
        mur_e.SetActive(false);
    }
    public void erase_mur_n()
    {
        mur_n.SetActive(false);
    }
    public void erase_mur_s()
    {
        mur_s.SetActive(false);
    }
}
