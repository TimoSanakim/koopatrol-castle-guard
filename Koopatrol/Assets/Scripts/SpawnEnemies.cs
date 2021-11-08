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

    private void Start()
    {
        waves = GameObject.FindGameObjectWithTag("Wavemanager");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        enemydubes = GameObject.FindGameObjectWithTag("EnemyList");
        
    }



    void Update()
    {
        if(!stopSpawning){
            timer+=Time.deltaTime;
            if (timer >= spawnTime)
            {
                GameObject enemy = Instantiate(waves.GetComponent<Waves>().TheWaves[waves.GetComponent<Waves>().waveIndex].wave[waves.GetComponent<Waves>().enemiesWaveIndex]);
                enemy.GetComponent<EnemyBehaviour>().Paths.AddRange(Map.PossiblePaths[Map.randomizer.Next(0, Map.PossiblePaths.Count)]);
                enemy.transform.SetParent(enemydubes.transform, true);
                enemy.transform.localPosition = enemy.GetComponent<EnemyBehaviour>().Paths[0].localPosition;
                enemy.transform.localScale = new Vector3(1, 1, 1);
                enemy.GetComponent<EnemyBehaviour>().isClone = true;
                enemy.GetComponent<CanvasGroup>().alpha = 1f;
                enemy.GetComponent<CanvasGroup>().interactable = true;
                enemy.GetComponent<CanvasGroup>().blocksRaycasts = true;
                enemy.tag = "Enemy";
                spawnTime += spawnDelay;
                waves.GetComponent<Waves>().enemiesWaveIndex++;
                spawnAmount++;
                enemy.GetComponent<EnemyHealth>().towerInfo = towerInfo;
                Map.Enemies.Add(enemy);
            }
        }
    }
}
