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
    public GameObject SpawnEnemies;
    GameObject RoundCounter;
    // Start is called before the first frame update
    void Start()
    {
        RoundCounter = GameObject.FindGameObjectWithTag("RoundCounter");
        RoundCounter.GetComponent<Text>().text = "Round: " + (waveIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {

        if (enemiesWaveIndex == TheWaves[waveIndex].wave.Count && waveIndex < TheWaves.Count - 1)
        {    
            timeBetweenWaves();
            if (waveDelay == 0)
            {
                SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = false;
                waveIndex++;
                RoundCounter.GetComponent<Text>().text = "Round: " + (waveIndex + 1);
                enemiesWaveIndex = 0;
                waveDelay = 5;
            }
        }
        else if (waveIndex == TheWaves.Count - 1 && enemiesWaveIndex == TheWaves[waveIndex].wave.Count - 1)
        {
            SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
        }

    }

    void timeBetweenWaves(){
        SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
        if(waveDelay > 0){
                waveDelay -= Time.deltaTime;
        }
        else{
            waveDelay = 0;
        }

    }
}
