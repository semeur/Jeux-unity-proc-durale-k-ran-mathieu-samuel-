using UnityEngine;

public class Camera_du_perso : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float sourisSensibilité = 100f; // sensibilité de la cam
    public Transform playerBody;
    float xRotation = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sourisSensibilité * Time.deltaTime; // récupération du mouvement de la souris sur l'axe Y et X
        float mouseY = Input.GetAxis("Mouse Y") * sourisSensibilité * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //limitation de la caméra à 90°

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
