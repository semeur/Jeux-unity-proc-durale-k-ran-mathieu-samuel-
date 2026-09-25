using UnityEngine;

public class Meca_de_saut : MonoBehaviour
{
    private float velocityY = 0;
    private float gravity = -35;
    public KeyCode jump = KeyCode.Space;
    public bool verif ; // vérfie i le perso touche le sol ou pas
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
            velocityY = 35;
            verif = false;
                isJumping = true;
        }
        velocityY += gravity * Time.deltaTime;
        transform.position += new Vector3(0, velocityY, 0) * Time.deltaTime;
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
        }
    }
    
}

