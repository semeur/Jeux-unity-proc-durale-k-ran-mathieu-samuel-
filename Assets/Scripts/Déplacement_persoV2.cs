using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UIElements;

public class Déplacement_PersoV2 : MonoBehaviour

{

    // Dans ce script, il n'y a pas mécanique de dash.
    public CharacterController PlayerMove; // initialisation des touches et du CharacterController
    public KeyCode forward = KeyCode.UpArrow;
    public KeyCode back = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode Sprint = KeyCode.LeftShift; //print en appuyant sur left shift
    public KeyCode respawn = KeyCode.R; //objectif de cette ligne est de faire respawn le personnage a des coordonnées précis

    public float speed = 1; //vitesse de déplacement du personnage
    public Vector3 mouvement = Vector3.zero; //différent mouvement que le joueur peut faire
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        speed = 5.5f;
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
        if (Input.GetKey(Sprint) && (Input.GetKey(forward) || Input.GetKey(back) || Input.GetKey(left) || Input.GetKey(right)))
        {
            speed = speed + 20.5f;
        }

        PlayerMove.Move(mouvement * deltaMove);
        if (Input.GetKey(respawn) && transform.position.y < -0.95f)
        {
            transform.position = new Vector3(0.99804f, 0.81f, 0.63517f);
        }

    }
}
