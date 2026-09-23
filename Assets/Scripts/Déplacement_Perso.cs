using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UIElements;

public class Déplacement_Perso : MonoBehaviour
{
    public CharacterController PlayerMove; // initialisation des touches et du CharacterController
    public KeyCode forward = KeyCode.UpArrow;
    public KeyCode back = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode Jump = KeyCode.Space;
    public KeyCode Sprint = KeyCode.LeftShift; //print en appuyant sur left shift
    public KeyCode respawn = KeyCode.R; //objectif de cette ligne est de faire respawn le personnage a des coordonnées précis
    public bool verif = true;
    public bool use_Jump = false;
    MeshRenderer Cube; //La meshrender du Cube

    public float speed = 5.5f; //vitesse de déplacement du personnage
    public Vector3 mouvement = Vector3.zero; //différent mouvement que le joueur peut faire
    private int nbr_frame = 0;
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
        {
            mouvement += Vector3.forward;

        }

        if (Input.GetKey(back))
        {
            mouvement += Vector3.back;
        }
        if (Input.GetKey(left))
        {
            mouvement += Vector3.left;
        }    

        if (Input.GetKey(right))
        {
            mouvement += Vector3.right;
        }
        if (Input.GetKey(Sprint)) // A corriger demain le 23 Sept
        {
            mouvement += Vector3.forward;
            speed = speed * 1.1f; 
        }
        if (verif == false)
        {
            mouvement += Vector3.down;
        }

        PlayerMove.Move(mouvement * deltaMove);
        if (Input.GetKey(respawn) && transform.position.y < 0.5f)
        {
            transform.position = new Vector3(1f, 0.5f, 1f);
        }

        if (Input.GetKey(Jump) && use_Jump == false) //process 
        {
            use_Jump = false;    
            print("Je saute");
            bool jump = false;
            if (jump == false)
            {
                nbr_frame += 1;
                Vector3 saut = transform.position;
                saut.y += 1;
                transform.position = saut;
                print("Nombre de frame" + nbr_frame);
                if (nbr_frame == 10)
                {
                    jump = true;
                    transform.position = new Vector3(1f, 0.5f, 1f);
                    use_Jump = false;
                }
            }


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
