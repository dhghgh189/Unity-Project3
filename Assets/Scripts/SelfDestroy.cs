using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float destroyTime;
    [SerializeField] bool manualUse;

    WaitForSeconds _destroyTime;
    Coroutine _destroyRoutine;

    void Awake()
    {
        _destroyTime = new WaitForSeconds(destroyTime);    
    }

    void OnEnable()
    {
        if (!manualUse)
        {
            _destroyRoutine = StartCoroutine(DestroyRoutine());
        }
    }

    public void ReserveDestroy()
    {
        if (_destroyRoutine != null)
            return;

        _destroyRoutine = StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return _destroyTime;

        if (gameObject.GetComponent<Poolable>() == null)
        {
            Destroy(gameObject);
        }
        else if (PoolManager.Instance.Push(gameObject) == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        _destroyRoutine = null;
    }
}
