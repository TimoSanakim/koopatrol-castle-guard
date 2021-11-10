using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

public class LoadRecord : MonoBehaviour
{
    public GameObject[] recordnames;
    public GameObject title;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Map.LoadedCustomMap);
        if (Map.LoadedCustomMap == -1)
        {
            title.GetComponent<TextMeshProUGUI>().text = "Bowser's Castle Records";
        }
        else if (Map.LoadedCustomMap == -2)
        {
            title.GetComponent<TextMeshProUGUI>().text = "Frozen Castle Records";
        }
        else if (Map.LoadedCustomMap == -3)
        {
            title.GetComponent<TextMeshProUGUI>().text = "Desert Castle Records";
        }
        else
        {
            string saveString = File.ReadAllText(Application.dataPath + "/customlevel" + Map.LoadedCustomMap);
            levelCreator.SaveObject saveObject = JsonUtility.FromJson<levelCreator.SaveObject>(saveString);
            title.GetComponent<TextMeshProUGUI>().text = saveObject.name + " Records";
        }
        if (File.Exists(Application.dataPath + "/recorddata" + Map.LoadedCustomMap))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/recorddata" + Map.LoadedCustomMap);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            if (saveObject.records[0].score != 0)
            {
                recordnames[0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = saveObject.records[0].name;
                recordnames[0].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = Convert.ToString(saveObject.records[0].score);
                recordnames[0].GetComponent<CanvasGroup>().alpha = 1;
                if (saveObject.records[1].score != 0)
                {
                    recordnames[1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = saveObject.records[1].name;
                    recordnames[1].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = Convert.ToString(saveObject.records[1].score);
                    recordnames[1].GetComponent<CanvasGroup>().alpha = 1;
                    if (saveObject.records[2].score != 0)
                    {
                        recordnames[2].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = saveObject.records[2].name;
                        recordnames[2].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = Convert.ToString(saveObject.records[2].score);
                        recordnames[2].GetComponent<CanvasGroup>().alpha = 1;
                    }
                }
            }
            else
            {
                recordnames[3].GetComponent<CanvasGroup>().alpha = 1;
            }
        }
        else
        {
            recordnames[3].GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    [Serializable]
    private class SaveRecord
    {
        public string name;
        public int score;
    }
    [Serializable]
    private class SaveObject {
        public List<SaveRecord> records;
    }
}
