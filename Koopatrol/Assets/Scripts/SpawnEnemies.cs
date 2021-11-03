using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemies : MonoBehaviour
{
    public Transform LevelEnemies;
    GameObject waves;
    GameObject enemydubes;
    GameObject towerInfo;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public int spawnAmount;
    float timer;
    List<Transform> Paths = new List<Transform>();

    private void Start()
    {
        waves = GameObject.FindGameObjectWithTag("Wavemanager");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        enemydubes = GameObject.FindGameObjectWithTag("EnemyList");
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        GameObject[] obstructedPaths = GameObject.FindGameObjectsWithTag("PathTower");
        Paths.Add(GameObject.FindGameObjectWithTag("Castle").transform);
        foreach (GameObject path in paths)
        {
            Paths.Add(path.transform);
        }
        foreach (GameObject path in obstructedPaths)
        {
            Paths.Add(path.transform);
        }
    }



    void Update()
    {
        if(!stopSpawning){
            timer+=Time.deltaTime;
            if (timer >= spawnTime)
            {
                //enemyOriginal = waves.GetComponent<Waves>().enemiesWave[waves.GetComponent<Waves>().enemiesWaveIndex];
                GameObject enemy = Instantiate(waves.GetComponent<Waves>().TheWaves[waves.GetComponent<Waves>().waveIndex].wave[waves.GetComponent<Waves>().enemiesWaveIndex]);
                enemy.transform.position = enemy.GetComponent<EnemyBehaviour>().startingPosition.transform.position;
                enemy.transform.SetParent(enemydubes.transform, true);
                enemy.GetComponent<EnemyBehaviour>().isClone = true;
                enemy.GetComponent<CanvasGroup>().alpha = 1f;
                enemy.GetComponent<CanvasGroup>().interactable = true;
                enemy.GetComponent<CanvasGroup>().blocksRaycasts = true;
                enemy.tag = "Enemy";
                spawnTime += spawnDelay;
                spawnAmount++;
                waves.GetComponent<Waves>().enemiesWaveIndex++;
                enemy.GetComponent<EnemyHealth>().towerInfo = towerInfo;
                enemy.GetComponent<EnemyBehaviour>().Paths.AddRange(Paths);
                Map.Enemies.Add(enemy);

            }    
        }
    }
}
