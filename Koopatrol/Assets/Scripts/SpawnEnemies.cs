using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public Transform LevelEnemies;
    public GameObject enemyOriginal;
    GameObject waves;
    GameObject enemydubes;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public int spawnAmount;
    float timer;

    private void Start()
    {
        waves = GameObject.FindGameObjectWithTag("Wavemanager");
        enemydubes = GameObject.FindGameObjectWithTag("EnemyList");
    }



    void Update()
    {
        if(!stopSpawning){
            timer+=Time.deltaTime;
            if (timer >= spawnTime)
            {
                enemyOriginal = waves.GetComponent<Waves>().TheWaves [waves.GetComponent<Waves>().waveIndex] .wave[waves.GetComponent<Waves>().enemiesWaveIndex];
                //enemyOriginal = waves.GetComponent<Waves>().enemiesWave[waves.GetComponent<Waves>().enemiesWaveIndex];
                GameObject enemy = Instantiate(enemyOriginal);
                enemy.transform.position = gameObject.transform.position;
                enemy.transform.SetParent(enemydubes.transform, true);
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