using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class Records : MonoBehaviour
{
    public TMP_InputField  setRecord;
    public GameObject recordcontainer;
    GameObject CastleHealth;
    GameObject Waves;
    public int recordHealth; 
    public int recordCoin;
    public int recordWave;
    public string recordName;
    int score;
    public bool endgame = false;

    

    // Start is called before the first frame update
    void Start()
    {
        recordcontainer = GameObject.FindGameObjectWithTag("Record");
        CastleHealth = GameObject.FindGameObjectWithTag("CastleHealth");
        Waves = GameObject.FindGameObjectWithTag("Wavemanager");
    }

    // Update is called once per frame
    void Update()
    {
        if(CastleHealth.GetComponent<CastleHealth>().HealthCastle < 1 || endgame == true){
            record();
            GameSettings.restartGame();
        }
    }
    public void record()
    {
        recordCoin = Assets.CoinCounter.GetCoinCount();
        recordHealth = CastleHealth.GetComponent<CastleHealth>().HealthCastle;
        recordWave = Waves.GetComponent<Waves>().round;

        //calculate score
        if (recordHealth < 1)
        {
            score = recordCoin * recordWave;
        }
        else
            score = recordCoin * recordHealth * recordWave;

        SaveObject saveObject;
        if (File.Exists(Application.dataPath + "/recorddata" + Map.LoadedCustomMap))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/recorddata" + Map.LoadedCustomMap);
            saveObject = JsonUtility.FromJson<SaveObject>(saveString);
        }
        else
        {
            saveObject = new SaveObject();
            saveObject.records = new List<RecordObject>();
            RecordObject record = new RecordObject();
            record.score = 0;
            record.name = "";
            saveObject.records.Add(record);
            RecordObject record2 = new RecordObject();
            record2.score = 0;
            record2.name = "";
            saveObject.records.Add(record2);
            RecordObject record3 = new RecordObject();
            record3.score = 0;
            record3.name = "";
            saveObject.records.Add(record3);
        }
        if (saveObject.records[0].score < score)
        {
            saveObject.records[2].score = saveObject.records[1].score;
            saveObject.records[2].name = saveObject.records[1].name;
            saveObject.records[1].score = saveObject.records[0].score;
            saveObject.records[1].name = saveObject.records[0].name;
            saveObject.records[0].score = score;
            saveObject.records[0].name = recordName;
        }
        else if (saveObject.records[1].score < score)
        {
            saveObject.records[2].score = saveObject.records[1].score;
            saveObject.records[2].name = saveObject.records[1].name;
            saveObject.records[1].score = score;
            saveObject.records[1].name = recordName;
        }
        else if (saveObject.records[2].score < score)
        {
            saveObject.records[2].score = score;
            saveObject.records[2].name = recordName;
        }
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.dataPath + "/recorddata" + Map.LoadedCustomMap, json);

    }
    public void setName(){
        recordName = setRecord.text;
        recordcontainer.GetComponent<CanvasGroup>().alpha = 0;
        recordcontainer.GetComponent<CanvasGroup>().interactable = false;
        recordcontainer.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void hideRecord(){
    recordcontainer.GetComponent<CanvasGroup>().alpha = 0;
    recordcontainer.GetComponent<CanvasGroup>().interactable = false;
        recordcontainer.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    [Serializable]
    private class RecordObject
    {
        public string name;
        public int score;
    }
    [Serializable]
    private class SaveObject
    {
        public List<RecordObject> records;
    }
}

