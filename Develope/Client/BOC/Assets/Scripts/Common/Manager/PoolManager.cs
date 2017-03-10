using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public static Vector3 HIDE_POS = new Vector3(-1000, -1000, 0);
    
    private static Dictionary<string, Queue<GameObject>> _battleGOCache;
    private static Dictionary<Type, Queue<object>> _classPool;
    private static Dictionary<string, Sprite> _spriteDic;
    private static Transform _cacheLayer;
    public static void Setup()
    {
        _battleGOCache = new Dictionary<string, Queue<GameObject>>();
        _classPool = new Dictionary<Type, Queue<object>>();
        _spriteDic = new Dictionary<string, Sprite>();
    }

    public static void AddSprite(string url, Sprite sprite)
    {
        if (_spriteDic.ContainsKey(url))
            return;
        _spriteDic.Add(url, sprite);
    }
    
    public static Sprite GetSprite(string url)
    {
        Sprite sprite;
        _spriteDic.TryGetValue(url, out sprite);
        return sprite;
    }



    public static void AddGameObject(string url, GameObject go)
    {
        if (url == null || url == "")
            return;
        Queue<GameObject> queue;
        if (_battleGOCache.ContainsKey(url))
        {
            queue = _battleGOCache[url];
        }
        else
        {
            queue = new Queue<GameObject>();
            _battleGOCache.Add(url, queue);
        }
        queue.Enqueue(go);
        go.transform.localPosition = HIDE_POS;
    }

    public static GameObject GetGameObject(string url)
    {
        GameObject go = null;
        if (_battleGOCache.ContainsKey(url))
        {
            Queue<GameObject> queue = _battleGOCache[url];
            if (queue.Count > 0)
            {	
                go = queue.Dequeue();
                go.transform.localPosition = Vector3.zero;
                //go.SetActive(true);
            }
        }
        return go;
    }

    public static void DisposeGameObjects()
    {
        foreach (Queue<GameObject> queue in _battleGOCache.Values)
        {
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject go = queue.Dequeue();
                GameObject.Destroy(go);
            }
        }
        _battleGOCache.Clear();
    }


    public static void Clear()
    {        
        DisposeGameObjects();
        
        _battleGOCache.Clear();
        _spriteDic.Clear();
        _classPool.Clear();
    }

    public static void RegistClassPool(Type type, int size)
    {
        _classPool[type] = new Queue<object>(size);
    }

    public static T GetClassInstance<T>() where T : new()
    {
        T instance;
        Queue<object> queue = _classPool[typeof(T)];
        if (queue.Count > 0)
        {
            instance = (T)queue.Dequeue();
        }
        else
            instance = new T();
        return instance;
    }

    public static void PushClassCache<T>(T t)
    {
        Queue<object> queue = _classPool[typeof(T)];
        queue.Enqueue(t);
    }


}
