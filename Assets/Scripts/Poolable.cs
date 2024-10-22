using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    private void Start()
    {
        ExSceneManager.Instance.OnChangedScene += ReturnToPool;
    }

    // callback
    public void ReturnToPool()
    {
        if (gameObject.activeInHierarchy)
        {
            if (PoolManager.Instance.Push(gameObject) == false)
                Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (ExSceneManager.Instance != null)
            ExSceneManager.Instance.OnChangedScene -= ReturnToPool;
    }
}
