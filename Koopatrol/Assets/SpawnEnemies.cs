using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public Transform LevelEnemies;
    public GameObject enemyOriginal;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;


    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
        
    }
    
    public void SpawnObject()
    {
        Instantiate(enemyOriginal, transform.position, transform.rotation);
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }

        
    }
}
