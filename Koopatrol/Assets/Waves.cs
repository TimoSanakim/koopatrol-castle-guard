using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{

    [System.Serializable]
    public class serializableClass
    {
        public List<GameObject> wave;
    }
    public List<serializableClass> TheWaves = new List<serializableClass>();


    public int[] Amountspawntimes;
    public int wavecount;

    public int enemiesWaveIndex;
    public int waveIndex;

    public float waveDelay;
    public GameObject SpawnEnemies;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (enemiesWaveIndex == TheWaves[waveIndex].wave.Count)
        {    
            timeBetweenWaves();
            if(waveDelay == 0){
            SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = false;
            waveIndex++;
            enemiesWaveIndex = 0;
            Debug.Log(waveIndex);
            waveDelay= 5;         
            }
        }

    }

    void timeBetweenWaves(){
        SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
        Debug.Log(waveDelay);
        if(waveDelay > 0){
                waveDelay -= Time.deltaTime;
        }
        else{
            waveDelay = 0;
        }

    }
}
