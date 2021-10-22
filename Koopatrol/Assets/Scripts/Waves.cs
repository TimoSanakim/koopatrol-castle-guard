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
    byte endlessMarioCount = 0;
    int lastMusicChange = 0;
    int round = 1;

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
        RoundCounter.GetComponent<Text>().text = "Round: " + round;
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
                round += 1;
                if (waveIndex == 0) waveIndex++;
                else TheWaves.RemoveAt(0);
                if (round <= 9998)
                {
                    RoundCounter.GetComponent<Text>().text = "Round: " + round;
                }
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
                int maxweight = round + 10;
                if (maxweight > 100) maxweight = 100;
                AddEnemy(0, maxweight);
                if (TheWaves[TheWaves.Count - 1].music == "")
                {
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

    void timeBetweenWaves()
    {
        SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
        if (currentWaveDelay > 0)
        {
            currentWaveDelay -= Time.deltaTime;
        }
        else
        {
            currentWaveDelay = 0;
        }

    }
    void AddEnemy(int weight, int maxweight)
    {
        int totalweight = weight;
        bool stop = false;
        int variable = Map.randomizer.Next(0, 8000);
        if (!hasSpawnedMario && round == 40 && TheWaves[TheWaves.Count - 1].wave.Count == 0 && !Map.Enemies.Contains(EndlessMario))
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
            totalweight += 25;
            stop = true;
            endlessMarioCount += 1;
        }
        else if (hasSpawnedMario && variable >= 7000 && round >= 40 && TheWaves[TheWaves.Count - 1].wave.Count == 0 && !Map.Enemies.Contains(EndlessMario) && totalweight + 25 < maxweight)
        {
            if (endlessMarioCount <= 3) endlessMarioCount += 1;
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
            totalweight += 25;
            stop = true;
            if (endlessMarioCount >= 1) TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
            if (endlessMarioCount >= 2) TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
            if (endlessMarioCount >= 3) TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
        }
        else if (!hasSpawnedLuigi && round == 20 && TheWaves[TheWaves.Count - 1].wave.Count == 0)
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessLuigi.gameObject);
            totalweight += 20;
        }
        else if (hasSpawnedLuigi && variable >= 7700 && round >= 20 && totalweight + 20 < maxweight)
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessLuigi.gameObject);
            totalweight += 20;
        }
        else if (!hasSpawnedYoshi && round == 10 && TheWaves[TheWaves.Count - 1].wave.Count == 0)
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessYoshi.gameObject);
            totalweight += 15;
        }
        else if (hasSpawnedYoshi && variable >= 7500 && round >= 10 && totalweight + 15 < maxweight)
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessYoshi.gameObject);
            totalweight += 15;
        }
        else if (variable >= 6000 && round >= 3 && totalweight + 3 < maxweight)
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessCaptainToad.gameObject);
            totalweight += 3;
        }
        else
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessToad.gameObject);
            totalweight += 1;
        }
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
        if (!stop && TheWaves[TheWaves.Count - 1].wave.Count <= 60 && (Map.randomizer.Next(0, 20) >= 1 && weight < maxweight || weight < 10)) AddEnemy(totalweight, maxweight);
    }
}
