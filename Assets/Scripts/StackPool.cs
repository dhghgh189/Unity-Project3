using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackPool
{
    GameObject _original;
    Transform _parent;

    Stack<GameObject> _pool;

    public StackPool(GameObject original, int size)
    {
        _original = original;
        _pool = new Stack<GameObject>(size);
        //_parent = new GameObject($"{original.name}_Pool").transform;

        GameObject go = new GameObject($"{original.name}_Pool");
        go.transform.SetParent(PoolManager.Instance.gameObject.transform);
        _parent = go.transform;

        for (int i = 0; i < size; i++)
        {
            CreateInstance();
        }
    }

    void CreateInstance()
    {
        GameObject instance = Object.Instantiate(_original);
        instance.name = _original.name;
        instance.transform.parent = _parent;
        instance.SetActive(false);

        if (instance.GetComponent<Poolable>() == null)
        {
            Poolable poolable = instance.AddComponent<Poolable>();
        }

        _pool.Push(instance);
    }

    public GameObject Pop(bool bActive)
    {
        if (_pool.Count <= 0)
            CreateInstance();

        GameObject instance = _pool.Pop();
        instance.SetActive(bActive);

        return instance;
    }

    public bool Push(GameObject instance)
    {
        if (instance == null)
            return false;

        instance.SetActive(false);
        instance.transform.parent = _parent;
        _pool.Push(instance);

        return true;
    }

    public void Clear()
    {
        while (_pool.Count > 0)
        {
            Object.Destroy(_pool.Pop());
        }
    }
}
