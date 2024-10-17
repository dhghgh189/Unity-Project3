using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float destroyTime;

    void OnEnable()
    {
        Destroy(gameObject, destroyTime);
    }
}
