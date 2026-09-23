using UnityEngine;
using UnityEngine.UIElements;

public class Déplacement_Perso : MonoBehaviour
{
    public CharacterController PlayerMove; // initialisation des touches et du CharacterController
    public KeyCode forward = KeyCode.UpArrow;
    public KeyCode back = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode respawn = KeyCode.R; //objectif de cette ligne est de faire respawn le personnage a des coordonnées précis
    public bool verif = true;
    MeshRenderer Cube; //La meshrender du Cube

    public float speed = 5.5f; //vitesse de déplacement du personnage
    public Vector3 mouvement = Vector3.zero; //différent mouvement que le joueur peut faire

    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime; //représente le temps écoulé depuis la frame d'avant.
        float deltaMove = speed * deltaTime; // un calcul avec ma vitesse à la l.11 et avec le temps écoulé depuis la frame d'avant.


        mouvement = Vector3.zero;

        if (Input.GetKey(forward))
            mouvement += Vector3.forward;

        if (Input.GetKey(back))
            mouvement += Vector3.back;

        if (Input.GetKey(left))
            mouvement += Vector3.left;

        if (Input.GetKey(right))
        {
            mouvement += Vector3.right;
        }
        if (verif == false)
        {
            mouvement += Vector3.down;
        }








        PlayerMove.Move(mouvement * deltaMove);
        if (Input.GetKey(respawn) && transform.position.y < 0.5f)
        {
            print("Je suis revenu"); //test du if 
            transform.position = new Vector3(1f, 0.5f, 1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        verif = true;
        Cube = other.GetComponent<MeshRenderer>(); // rend le cube "invisible" lors du contact
        Cube.enabled = false;
    }
    private void OnTriggerExit(Collider other)
    {
        verif = false;
    }
}
