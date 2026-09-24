using UnityEngine;

public class Test_gravity : MonoBehaviour
{
    private float velocityY = 0;
    private float gravity = -5;
    public KeyCode Jump = KeyCode.Space;
    public bool verif = true;

    MeshRenderer trigger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Jump) && verif == true)
        {
            velocityY = 4;
            gravity = -1;
        }
        velocityY += gravity * Time.deltaTime;
        transform.position += new Vector3(0, velocityY, 0) * Time.deltaTime;
        if (verif == true && velocityY < 0)
        {
            velocityY = 0;
            
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        print("je détecte le sol");
        verif = true;
        trigger = other.GetComponent<MeshRenderer>(); // rend le cube "invisible" lors du contact
        trigger.enabled = false;


    }
    private void OnTriggerExit(Collider other)
    {
        verif = false;
    }
}

