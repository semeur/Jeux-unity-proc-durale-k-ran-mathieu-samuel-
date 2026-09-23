using UnityEngine;

public class bloc : MonoBehaviour
{
    public GameObject cube_haut;
    public GameObject cube_bas;

    //pour ajuter les different material crée par moi
    public Material herbematerial;
    public Material Terrematerial;
    public Material sablematerial;
    public Material rochematerial;

     MeshRenderer cube_haut_renderer;
     MeshRenderer cube_bas_renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         cube_haut_renderer = cube_haut.GetComponent<MeshRenderer>();
         cube_bas_renderer = cube_bas.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changer_en_hebe ()
    { 
        cube_haut.SetActive(true);
        cube_bas.SetActive(true);

        cube_haut_renderer.material.color = herbematerial.color;
        cube_bas_renderer.material.color = Terrematerial.color;

    }
    public void changer_en_sable()
    {
        cube_haut.SetActive(false);
        cube_bas.SetActive(true);

        cube_haut_renderer.material.color = sablematerial.color;
        cube_bas_renderer.material.color = sablematerial.color;

    }
    public void changer_en_roche()
    {
        cube_haut.SetActive(false);
        cube_bas.SetActive(true);

        cube_haut_renderer.material.color = rochematerial.color;
        cube_bas_renderer.material.color = rochematerial.color;

    }
}
