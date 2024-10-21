using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    ParticleSystem _particle;

    [SerializeField] bool PlayOnEnable;

    void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (PlayOnEnable)
        {
            Debug.Log("particle play");
            _particle.Play();
        }
    }
}
