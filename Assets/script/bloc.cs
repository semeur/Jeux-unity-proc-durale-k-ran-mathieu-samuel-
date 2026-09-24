using UnityEngine;
using static UnityEngine.ParticleSystem;

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


    void Awake()
    {
         //obtenir les info sur la "couleur " des compossant du bloc quand on les appels 
         cube_haut_renderer = cube_haut.GetComponent<MeshRenderer>();
         cube_bas_renderer = cube_bas.GetComponent<MeshRenderer>();
    }

    /*Ne sert a rien pour le moment
    void Update()
    {
        
    }
    */

    public void changer_en_hebe ()//le biome plaine est choisie et va etre générer
    { 
        //on garde les 2partie du bloc pour avoir la couleur de l'heurbe et la couleur de la terre
        cube_haut.SetActive(true);
        cube_bas.SetActive(true);

        //ici on change les couleur pour bien correpondre au bloc
        cube_haut_renderer.material.color = herbematerial.color;
        cube_bas_renderer.material.color = Terrematerial.color;

    }
    public void changer_en_sable()//le biome desert est choisie et va etre générer
    {
        //on garde que la partie basse car on n'a pas besoin de faire different coulleur
        cube_haut.SetActive(false);
        cube_bas.SetActive(true);

        //ici on change les couleur pour bien correpondre au bloc mais on change quand même les 2 partie au cas ou l'autre s'affiche quand même
        cube_haut_renderer.material.color = sablematerial.color;
        cube_bas_renderer.material.color = sablematerial.color;

    }
    public void changer_en_roche()//le biome montagne est choisie et va etre générer
    {
        //on garde que la partie basse car on n'a pas besoin de faire different coulleur
        cube_haut.SetActive(false);
        cube_bas.SetActive(true);

        //ici on change les couleur pour bien correpondre au bloc mais on change quand même les 2 partie au cas ou l'autre s'affiche quand même
        cube_haut_renderer.material.color = rochematerial.color;
        cube_bas_renderer.material.color = rochematerial.color;

    }
}
