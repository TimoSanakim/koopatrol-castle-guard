using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waves : MonoBehaviour
{

    [System.Serializable]
    public class serializableClass
    {
        public List<GameObject> wave;
    }
    public List<serializableClass> TheWaves = new List<serializableClass>();


    public int[] Amountspawntimes;

    public int enemiesWaveIndex;
    public int waveIndex;

    public float waveDelay;
    public float currentWaveDelay;
    
    public GameObject SpawnEnemies;
    GameObject RoundCounter;
    // Start is called before the first frame update
    void Start()
    {
        currentWaveDelay = waveDelay;
        RoundCounter = GameObject.FindGameObjectWithTag("RoundCounter");
        RoundCounter.GetComponent<Text>().text = "Round: " + (waveIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {

        if (enemiesWaveIndex == TheWaves[waveIndex].wave.Count && waveIndex < TheWaves.Count - 1)
        {    
            timeBetweenWaves();
            if (currentWaveDelay == 0)
            {
                SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = false;
                waveIndex++;
                RoundCounter.GetComponent<Text>().text = "Round: " + (waveIndex + 1);
                enemiesWaveIndex = 0;
                currentWaveDelay = waveDelay;
            }
        }
        else if (waveIndex == TheWaves.Count - 1 && enemiesWaveIndex == TheWaves[waveIndex].wave.Count)
        {
            SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
        }

    }

    void timeBetweenWaves(){
        SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
        if(currentWaveDelay > 0){
                currentWaveDelay -= Time.deltaTime;
        }
        else{
            currentWaveDelay = 0;
        }

    }
}
