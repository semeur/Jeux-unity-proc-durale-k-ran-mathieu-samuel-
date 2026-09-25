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
    public KeyCode sprint = KeyCode.LeftShift; //print en appuyant sur left shift
    public KeyCode respawn = KeyCode.R; //objectif de cette ligne est de faire respawn le personnage a des coordonnées précis
    public float speedshift = 2.8f;
    public float speed = 5.5f; //vitesse de déplacement du personnage
    public Vector3 mouvement = Vector3.zero; //différent mouvement que le joueur peut faire
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        speed = 5.5f;
        float deltaTime = Time.deltaTime; //représente le temps écoulé depuis la frame d'avant.

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
        if (Input.GetKey(sprint) && (Input.GetKey(forward) || Input.GetKey(back) || Input.GetKey(left) || Input.GetKey(right)))
        {
            speed = speed * speedshift;
        }
        float deltaMove = speed * deltaTime; // un calcul avec ma vitesse à la l.11 et avec le temps écoulé depuis la frame d'avant.

        PlayerMove.Move(mouvement * deltaMove);
        if (Input.GetKey(respawn) && transform.position.y < 1f)
        {
            transform.position = new Vector3(1f, 1f, 1f);
        }

    }
}
