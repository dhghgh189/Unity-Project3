using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask whatIsTarget;
    
    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();    
    }

    void OnEnable()
    {
        _rb.velocity = Vector3.zero;
    }

    public void AddForce(Vector3 force, ForceMode mode)
    {
        _rb.AddForce(force, mode);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (((1 << other.gameObject.layer) & whatIsTarget) != 0)
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsTarget) != 0)
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
