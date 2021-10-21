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
        public string music;
    }
    public List<serializableClass> TheWaves = new List<serializableClass>();

    public int enemiesWaveIndex;
    public int waveIndex;
    public bool endlessMode = false;
    public bool hasSpawnedYoshi = false;
    public bool hasSpawnedLuigi = false;
    public bool hasSpawnedMario = false;
    public GameObject EndlessToad;
    public GameObject EndlessCaptainToad;
    public GameObject EndlessYoshi;
    public GameObject EndlessLuigi;
    public GameObject EndlessMario;

    public float waveDelay;
    public float currentWaveDelay;
    
    public GameObject SpawnEnemies;
    GameObject RoundCounter;
    GameObject Music;
    // Start is called before the first frame update
    void Start()
    {
        currentWaveDelay = waveDelay;
        RoundCounter = GameObject.FindGameObjectWithTag("RoundCounter");
        Music = GameObject.FindGameObjectWithTag("Music");
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
                if (TheWaves[waveIndex].music != null && TheWaves[waveIndex].music != "") Music.GetComponent<Music>().PlayNew(TheWaves[waveIndex].music);
            }
        }
        else if (waveIndex == TheWaves.Count - 1 && enemiesWaveIndex == TheWaves[waveIndex].wave.Count)
        {
            if (endlessMode)
            {
                TheWaves.Add(new serializableClass());
                TheWaves[TheWaves.Count - 1].wave = new List<GameObject>();
                TheWaves[TheWaves.Count - 1].music = "";
                AddEnemy();
                if (TheWaves[TheWaves.Count - 1].music == "") { }
            }
            else
            {
                SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
            }
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
    void AddEnemy()
    {
        bool stop = false;
        int variable = Map.randomizer.Next(0, 10000);
        if (hasSpawnedLuigi && variable >= 9999 && TheWaves[TheWaves.Count - 1].wave.Count == 0) { TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject); stop = true; }
        else if (hasSpawnedYoshi && variable >= 9700 && !(!hasSpawnedLuigi && TheWaves[TheWaves.Count - 1].wave.Count != 0)) TheWaves[TheWaves.Count - 1].wave.Add(EndlessLuigi.gameObject);
        else if (variable >= 9500 && !(!hasSpawnedYoshi && TheWaves[TheWaves.Count - 1].wave.Count != 0)) TheWaves[TheWaves.Count - 1].wave.Add(EndlessYoshi.gameObject);
        else if (variable >= 7000) TheWaves[TheWaves.Count - 1].wave.Add(EndlessCaptainToad.gameObject);
        else TheWaves[TheWaves.Count - 1].wave.Add(EndlessToad.gameObject);
        if (TheWaves[TheWaves.Count - 1].wave.Count == 1)
        {
            if (TheWaves[TheWaves.Count - 1].wave[0].GetComponent<EnemyBehaviour>().enemyType == "Yoshi" && !hasSpawnedYoshi)
            {
                hasSpawnedYoshi = true;
                TheWaves[TheWaves.Count - 1].music = "Yoshi";
                stop = true;
            }
            else if (TheWaves[TheWaves.Count - 1].wave[0].GetComponent<EnemyBehaviour>().enemyType == "Luigi" && !hasSpawnedLuigi)
            {
                hasSpawnedLuigi = true;
                TheWaves[TheWaves.Count - 1].music = "Luigi";
                stop = true;
            }
            else if (TheWaves[TheWaves.Count - 1].wave[0].GetComponent<EnemyBehaviour>().enemyType == "Mario" && !hasSpawnedMario)
            {
                hasSpawnedMario = true;
                TheWaves[TheWaves.Count - 1].music = "Mario";
                stop = true;
            }
        }
        if (!stop && Map.randomizer.Next(0, 10) >= 1) AddEnemy();
    }
}
