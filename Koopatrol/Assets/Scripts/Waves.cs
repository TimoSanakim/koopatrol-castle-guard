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
    public GameObject EndlessBobOmbBuddy;
    public List<GameObject> StartingPositions;
    byte endlessMarioCount = 0;
    int lastMusicChange = 0;
    int round = 1;

    public float waveDelay;
    public float currentWaveDelay;

    public GameObject SpawnEnemies;
    GameObject RoundCounter;
    GameObject Music;
    List<GameObject> AllPaths;
    // Start is called before the first frame update
    void Start()
    {
        currentWaveDelay = waveDelay;
        RoundCounter = GameObject.FindGameObjectWithTag("RoundCounter");
        Music = GameObject.FindGameObjectWithTag("Music");
        if (RoundCounter != null) RoundCounter.GetComponent<Text>().text = "Round: " + round;
        SearchForPaths();
    }
    public void SearchForPaths()
    {
        AllPaths = new List<GameObject>();
        GameObject[] Castles = GameObject.FindGameObjectsWithTag("Castle");
        GameObject[] Paths = GameObject.FindGameObjectsWithTag("Path");
        GameObject[] PathTowers = GameObject.FindGameObjectsWithTag("PathTower");
        foreach (GameObject p in Castles)
        {
            AllPaths.Add(p);
        }
        foreach (GameObject p in Paths)
        {
            AllPaths.Add(p);
        }
        foreach (GameObject p in PathTowers)
        {
            AllPaths.Add(p);
        }
        Map.PossiblePaths = new List<List<Transform>>();
        foreach (GameObject position in StartingPositions)
        {
            List<Transform> positions = new List<Transform>();
            positions.Add(position.transform);
            GetNextPath(positions);
        }
        if (Map.PossiblePaths.Count == 0)
        {
            if (AllPaths.Count != 0) Map.WriteToLog("No valid paths to castle detected!");
            else Map.WriteToLog("Left click to place a tile, right click to remove it. You can set towers and upgrade them as normal.");
            SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = true;
        }
        if (gameObject.transform.parent.transform.parent.GetComponent<levelCreator>() == null && TheWaves[0].music != null && TheWaves[0].music != "") Music.GetComponent<Music>().PlayNew(TheWaves[0].music);
    }
    void GetNextPath(List<Transform> pastPositiones)
    {
        List<Transform> PastPaths = new List<Transform>();
        PastPaths.AddRange(pastPositiones);
        float x = PastPaths[PastPaths.Count - 1].localPosition.x;
        float y = PastPaths[PastPaths.Count - 1].localPosition.y;
        foreach (GameObject t in AllPaths)
        {
            if (!PastPaths.Contains(t.transform))
            {
                if (t.transform.localPosition.x >= x - 10 && t.transform.localPosition.x <= x + 10 && t.transform.localPosition.y >= y + 40 && t.transform.localPosition.y <= y + 60)
                {
                    List<Transform> newPaths = new List<Transform>();
                    newPaths.AddRange(PastPaths);
                    newPaths.Add(t.transform);
                    if (t.tag == "Castle")
                    {
                        List<Transform> MapValue = new List<Transform>();
                        MapValue.AddRange(newPaths);
                        Map.PossiblePaths.Add(MapValue);
                    }
                    else GetNextPath(newPaths);
                }
                if (t.transform.localPosition.x >= x - 10 && t.transform.localPosition.x <= x + 10 && t.transform.localPosition.y >= y - 60 && t.transform.localPosition.y <= y - 40)
                {
                    List<Transform> newPaths = new List<Transform>();
                    newPaths.AddRange(PastPaths);
                    newPaths.Add(t.transform);
                    if (t.tag == "Castle")
                    {
                        List<Transform> MapValue = new List<Transform>();
                        MapValue.AddRange(newPaths);
                        Map.PossiblePaths.Add(MapValue);
                    }
                    else GetNextPath(newPaths);
                }
                if (t.transform.localPosition.y >= y - 10 && t.transform.localPosition.y <= y + 10 && t.transform.localPosition.x >= x + 40 && t.transform.localPosition.x <= x + 60)
                {
                    List<Transform> newPaths = new List<Transform>();
                    newPaths.AddRange(PastPaths);
                    newPaths.Add(t.transform);
                    if (t.tag == "Castle")
                    {
                        List<Transform> MapValue = new List<Transform>();
                        MapValue.AddRange(newPaths);
                        Map.PossiblePaths.Add(MapValue);
                    }
                    else GetNextPath(newPaths);
                }
                if (t.transform.localPosition.y >= y - 10 && t.transform.localPosition.y <= y + 10 && t.transform.localPosition.x >= x - 60 && t.transform.localPosition.x <= x - 40)
                {
                    List<Transform> newPaths = new List<Transform>();
                    newPaths.AddRange(PastPaths);
                    newPaths.Add(t.transform);
                    if (t.tag == "Castle")
                    {
                        List<Transform> MapValue = new List<Transform>();
                        MapValue.AddRange(newPaths);
                        Map.PossiblePaths.Add(MapValue);
                    }
                    else GetNextPath(newPaths);
                }
            }
        }
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
    bool MarioExists() {
        foreach (GameObject enemy in Map.Enemies)
        {
            if (enemy.GetComponent<EnemyBehaviour>().enemyType == "Mario") return true;
        }
        return false;
    }
    void AddEnemy(int weight, int maxweight)
    {
        int totalweight = weight;
        bool stop = false;
        int variable = Map.randomizer.Next(0, 8000);
        if (!hasSpawnedMario && round == 40 && TheWaves[TheWaves.Count - 1].wave.Count == 0 && !MarioExists())
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
            totalweight += 25;
            stop = true;
        }
        else if (hasSpawnedMario && variable >= 7000 && round >= 40 && TheWaves[TheWaves.Count - 1].wave.Count == 0 && !MarioExists() && totalweight + 25 < maxweight)
        {
            if (endlessMarioCount <= 3) endlessMarioCount += 1;
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
            totalweight += 25;
            stop = true;
            if (endlessMarioCount >= 1) TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
            if (endlessMarioCount >= 2) TheWaves[TheWaves.Count - 1].wave.Add(EndlessMario.gameObject);
        }
        else if (!hasSpawnedLuigi && round == 20 && TheWaves[TheWaves.Count - 1].wave.Count == 0)
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessLuigi.gameObject);
            totalweight += 20;
        }
        else if (hasSpawnedLuigi && variable >= 7700 && Map.randomizer.Next(0, 2) == 1 && round >= 40 && totalweight + 20 < maxweight)
        {
            TheWaves[TheWaves.Count - 1].wave.Add(EndlessBobOmbBuddy.gameObject);
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
