using UnityEngine;

public class Test_gravity : MonoBehaviour
{
    private float velocityY = 0;
    private float gravity = -5;
    public KeyCode jump = KeyCode.Space;
    public bool verif = true; // vérfie i le perso touche le sol ou pas
    //public bool sol_toucher = true;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(jump) && verif == true)
        {
            velocityY = 4;
            verif = false;
        }
        velocityY += gravity * Time.deltaTime;
        transform.position += new Vector3(0, velocityY, 0) * Time.deltaTime;
        //if (sol_toucher == true && velocityY < 0)
        if (verif && velocityY < 0)
        {
            velocityY = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        verif = true;
        print("Touche le sol");
    }
    private void OnTriggerExit(Collider other)
    {
        verif = false;
    }
}

