using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSpawne : MonoBehaviour
{
    public ObjectPooler objectPooler;
    public Transform platform;

    private void Awake()
    {
        objectPooler = ObjectPooler.Instance;
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }
    private void OnSceneWasLoaded(Scene result, LoadSceneMode loadSceneMode)
    {
        foreach (var pool in objectPooler.pools)
        {
            for (int i = 0; i < pool.size; i++)
            {
                float spawnPosX = Random.Range(platform.position.x - platform.localScale.x / 2 + 80, platform.position.x + platform.localScale.x / 2 - 40);
                float spawnPosY = Random.Range(1, 1000);
                spawnPosY = spawnPosY < 700 ? 1 : 3;
                Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY);
                objectPooler.SpawnFromPool(pool.tag, spawnPos, Quaternion.identity);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
    }
}
