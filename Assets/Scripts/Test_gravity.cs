using UnityEngine;

public class Test_gravity : MonoBehaviour
{
    private float velocityY = 0;
    private float gravity = -5;
    public KeyCode jump = KeyCode.Space;
    public bool verif ; // vérfie i le perso touche le sol ou pas
    //public bool sol_toucher = true;
    public bool isJumping = false;

    void Start()
    {
        verif = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(jump) && verif == true )
        {
            velocityY = 10;
            verif = false;
                isJumping = true;
            Debug.Log("jump");
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Sol"))
        {
            verif = true;
            isJumping = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sol")&&isJumping==false)
        {
        
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sol"))
        {
            verif = false;
            Debug.Log("TriggerExit");
        }
    }
    
}

