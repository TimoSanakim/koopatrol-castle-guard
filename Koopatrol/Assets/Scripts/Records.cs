using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

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
    public void record(){
    recordCoin = Assets.CoinCounter.GetCoinCount();
    recordHealth = CastleHealth.GetComponent<CastleHealth>().HealthCastle;
    recordWave = Waves.GetComponent<Waves>().waveIndex;

    //calculate score
    if(recordHealth<1){
    score = recordCoin*recordWave;
    }
    else
    score = recordCoin*recordHealth*recordWave;
    Debug.Log("recordscore" + score);

    SaveObject saveObject = new SaveObject{
        recordname = recordName,
        recordscore = score,
    };
    string json = JsonUtility.ToJson(saveObject);
    Debug.Log("json"+json);

    SaveObject loadedSaveObject = JsonUtility.FromJson<SaveObject>(json);
    Debug.Log(loadedSaveObject.recordscore);
    File.WriteAllText(Application.dataPath + "/savedata",json);
    }
    public void setName(){
        recordName = setRecord.text;
    }
    
    public void hideRecord(){
    recordcontainer.GetComponent<CanvasGroup>().alpha = 0;
    recordcontainer.GetComponent<CanvasGroup>().interactable = false;
    }

    private class SaveObject {
        public string recordname;
        public int recordscore;
    }
}

