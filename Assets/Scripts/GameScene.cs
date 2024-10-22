using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] PoolableInfo[] poolableObjects;

    void Start()
    {
        for (int i = 0; i < poolableObjects.Length; i++)
        {
            PoolManager.Instance.CreatePool(poolableObjects[i].Prefab, poolableObjects[i].ObjectCount);
        }
    }
}

[System.Serializable]
public class PoolableInfo
{
    public GameObject Prefab;
    public int ObjectCount;
}
