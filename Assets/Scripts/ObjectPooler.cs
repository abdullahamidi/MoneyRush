using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    // Start is called before the first frame update
    //void OnEnable()
    //{
    //    poolDictionary = new Dictionary<string, Queue<GameObject>>();

    //    foreach (Pool pool in pools)
    //    {
    //        Queue<GameObject> objectPool = new Queue<GameObject>();

    //        for (int i = 0; i < pool.size; i++)
    //        {
    //            GameObject obj = Instantiate(pool.prefab);
    //            obj.SetActive(false);
    //            objectPool.Enqueue(obj);
    //        }

    //        poolDictionary.Add(pool.tag, objectPool);
    //    }
    //}

    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        GameObject object2Spawn = poolDictionary[tag].Dequeue();

        object2Spawn.SetActive(true);
        object2Spawn.transform.position = pos;
        object2Spawn.transform.rotation = rot;

        poolDictionary[tag].Enqueue(object2Spawn);

        return object2Spawn;
    }
}
