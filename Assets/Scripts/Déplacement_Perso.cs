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
    public KeyCode sprint = KeyCode.RightShift; //print en appuyant sur Right shift
    public KeyCode respawn = KeyCode.R; //objectif de cette ligne est de faire respawn le personnage a des coordonnées précis
    public float speedshift = 2.8f;
    public float speed = 5.5f; //vitesse de déplacement du personnage
    public Vector3 mouvement = Vector3.zero; //différent mouvement que le joueur peut faire
    public Meca_de_saut other_verif;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        speed = 5.5f;
        float deltaTime = Time.deltaTime; //représente le temps écoulé depuis la frame d'avant
        mouvement = Vector3.zero;

        if (Input.GetKey(forward))
        {
            mouvement += transform.forward; //transform.forward me permet d'aller dans la direction ou regarde mon personnage
        }
        if (Input.GetKey(back))
        {
            mouvement += -transform.forward; // inverse de l'avant donc l'arrière // j'ai mis sa car maintenant sa me permet d'avancer selon mon curseur
        }
        if (Input.GetKey(left))
        {
            mouvement += -transform.right; //inverse de droite donc gauche
        }

        if (Input.GetKey(right))
        {
            mouvement += transform.right;
        }
        mouvement = mouvement.normalized; // j'ai mis sa car sa me permet d'éviter d'avancer plus vite en diagonale
        if ((Input.GetKey(sprint) && other_verif.isJumping == false) && (Input.GetKey(forward) || Input.GetKey(back) || Input.GetKey(left) || Input.GetKey(right)))
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
