using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PoolManager : MonoBehaviour
{
    static PoolManager _instance = null;
    public static PoolManager Instance { get { return _instance; } }

    // 풀링 오브젝트의 default pool size
    [SerializeField] int defaultPoolSize;
    public int DefaultPoolSize { get { return defaultPoolSize; } }

    Dictionary<string, StackPool> _pools;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            _pools = new Dictionary<string, StackPool>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(GameObject original, int poolSize = 0)
    {
        if (_pools.ContainsKey(original.name))
            return;

        if (poolSize == 0)
            poolSize = defaultPoolSize;

        StackPool pool = new StackPool(original, poolSize);
        _pools.Add(original.name, pool);
    }

    public GameObject Pop(string name, bool bActive = true)
    {
        // 풀을 생성하지 않고 풀링을 시도하는 경우
        if (_pools.ContainsKey(name) == false)
        {
            GameObject original = Resources.Load<GameObject>($"Prefabs/{name}");
            CreatePool(original, defaultPoolSize);
        }

        GameObject instance = _pools[name].Pop(bActive);

        return instance;
    }

    // 일반화를 통해 원하는 컴포넌트를 바로 찾아오기
    public T Pop<T>(string name, bool bActive = true) where T : Object
    {
        // 풀을 생성하지 않고 풀링을 시도하는 경우
        if (_pools.ContainsKey(name) == false)
        {
            GameObject original = Resources.Load<GameObject>($"Prefabs/{name}");
            CreatePool(original, defaultPoolSize);
        }

        GameObject instance = _pools[name].Pop(bActive);
        T component = instance.GetComponent<T>();
        if (component == null)
        {
            Debug.LogWarning("PoolManager : Get Instance Failed...");
            return null;
        }

        return component;
    }

    public GameObject Pop(GameObject original, bool bActive = true)
    {
        // 풀을 생성하지 않고 풀링을 시도하는 경우
        if (_pools.ContainsKey(original.name) == false)
        {
            CreatePool(original, defaultPoolSize);
        }

        return Pop(original.name, bActive);
    }

    public T Pop<T>(GameObject original, bool bActive = true) where T : Object
    {
        if (_pools.ContainsKey(original.name) == false)
        {
            CreatePool(original, defaultPoolSize);
        }
        
        return Pop<T>(original.name, bActive);
    }

    public bool Push(GameObject instance)
    {
        if (instance == null)
            return false;

        if (_pools.ContainsKey(instance.name) == false)
        {
            Debug.LogWarning("PoolManager : Push Instance Failed...");
            return false;
        }

        return _pools[instance.name].Push(instance);
    }

    public bool ContainsKey(string key)
    {
        return _pools.ContainsKey(key);
    }

    public void Clear()
    {
        foreach (StackPool pool in _pools.Values)
        {
            pool.Clear();
        }

        _pools.Clear();
    }
}
