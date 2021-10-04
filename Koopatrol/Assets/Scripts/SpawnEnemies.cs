using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public Transform LevelEnemies;
    public GameObject enemyOriginal;
    GameObject waves;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public int spawnAmount;
    float timer;

    private void Start()
    {
        waves = GameObject.FindGameObjectWithTag("Wavemanager");
    }



    void Update()
    {
        Debug.Log(timer);
        if(!stopSpawning){
            timer+=Time.deltaTime;
            if (timer >= spawnTime)
            {
                enemyOriginal = waves.GetComponent<Waves>().TheWaves [waves.GetComponent<Waves>().waveIndex] .wave[waves.GetComponent<Waves>().enemiesWaveIndex];
                //enemyOriginal = waves.GetComponent<Waves>().enemiesWave[waves.GetComponent<Waves>().enemiesWaveIndex];
                GameObject enemy = Instantiate(enemyOriginal, transform.position, transform.rotation);
                enemy.GetComponent<EnemyBehaviour>().isClone = true;
                enemy.GetComponent<CanvasGroup>().alpha = 1f;
                enemy.tag = "Enemy";
                spawnTime += spawnDelay;
                spawnAmount++;
                waves.GetComponent<Waves>().enemiesWaveIndex++;
            }    
        }
    }
}
