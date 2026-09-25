using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody rb;
    public float move_speed;
    private Transform target;
    private float detection_range = 1000f;
    private Animator animator;

    [System.Obsolete]
    private void Start()
    {
        target = FindFirstObjectByType<CharacterController>().transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        rb.linearVelocity = Vector3.down;
        float distance_to_player = Vector3.Distance(transform.position, target.position);
        Vector3 direction = (target.position - transform.position).normalized;

        if (distance_to_player <= detection_range)
        {
            rb.linearVelocity = (target.position - transform.position).normalized * move_speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
