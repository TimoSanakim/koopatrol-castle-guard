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
    int lastMusicChange = 0;

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
                if (waveIndex <= 9998) RoundCounter.GetComponent<Text>().text = "Round: " + (waveIndex + 1);
                else RoundCounter.GetComponent<Text>().text = "Round: ????";
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
                if (TheWaves[TheWaves.Count - 1].music == "") {
                    lastMusicChange += 1;
                    if (lastMusicChange >= 25)
                    {
                        if (TheWaves[TheWaves.Count - 1].wave.Contains(EndlessMario) && Music.GetComponent<Music>().music != "Mario")
                        {
                            TheWaves[TheWaves.Count - 1].music = "Mario";
                        }
                        else if (TheWaves[TheWaves.Count - 1].wave.Contains(EndlessLuigi) && Music.GetComponent<Music>().music != "Luigi")
                        {
                            TheWaves[TheWaves.Count - 1].music = "Luigi";
                        }
                        else if (TheWaves[TheWaves.Count - 1].wave.Contains(EndlessYoshi) && Music.GetComponent<Music>().music != "Yoshi")
                        {
                            TheWaves[TheWaves.Count - 1].music = "Yoshi";
                        }
                        else if (Music.GetComponent<Music>().music != "Toads")
                        {
                            TheWaves[TheWaves.Count - 1].music = "Toads";
                        }
                    }
                }
                if (TheWaves[TheWaves.Count - 1].music != "") lastMusicChange = 0;
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
        int variable = Map.randomizer.Next(0, 8000);
        if (hasSpawnedLuigi && TheWaves.Count == 30 && !hasSpawnedMario && TheWaves[TheWaves.Count - 1].wave.Count == 0) { TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject); stop = true; }
        else if (hasSpawnedLuigi && variable >= 7999 && TheWaves.Count >= 30 && hasSpawnedMario && TheWaves[TheWaves.Count - 1].wave.Count == 0) { TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject); stop = true; }
        else if (hasSpawnedYoshi && TheWaves.Count == 20 && !hasSpawnedLuigi && TheWaves[TheWaves.Count - 1].wave.Count == 0) TheWaves[TheWaves.Count - 1].wave.Add(EndlessLuigi.gameObject);
        else if (hasSpawnedYoshi && variable >= 7700 && TheWaves.Count >= 20 && hasSpawnedLuigi) TheWaves[TheWaves.Count - 1].wave.Add(EndlessLuigi.gameObject);
        else if (TheWaves.Count == 10 && !hasSpawnedYoshi && TheWaves[TheWaves.Count - 1].wave.Count == 0) TheWaves[TheWaves.Count - 1].wave.Add(EndlessYoshi.gameObject);
        else if (variable >= 7500 && TheWaves.Count >= 10 && hasSpawnedYoshi) TheWaves[TheWaves.Count - 1].wave.Add(EndlessYoshi.gameObject);
        else if (variable >= 6000 && TheWaves.Count >= 3) TheWaves[TheWaves.Count - 1].wave.Add(EndlessCaptainToad.gameObject);
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
        if (!stop && TheWaves[TheWaves.Count - 1].wave.Count <= 60 && Map.randomizer.Next(0, 20) >= 1) AddEnemy();
    }
}
