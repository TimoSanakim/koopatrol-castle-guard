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



    //[System.Serializable]

    //public class Point
    //{
    //    public int[] wave;
    //}

    //public List<Point> TheWaves;





    //public GameObject[] waves;
    //public float[] wavetime;
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
        Debug.Log(waveDelay);
        

        if (enemiesWaveIndex == TheWaves[waveIndex].wave.Count)
        {
                waveIndex++;
                enemiesWaveIndex = 0;

        }

    }
}
