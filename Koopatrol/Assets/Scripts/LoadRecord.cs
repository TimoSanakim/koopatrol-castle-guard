using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LoadRecord : MonoBehaviour
{
    int i;
    public string topscore;
    public GameObject [] recordnames;
    
    // Start is called before the first frame update
    void Start()
    {
        if(File.Exists(Application.dataPath + "/savedata")){
            string saveString = File.ReadAllText(Application.dataPath + "/savedata");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            recordnames[0].GetComponent<TextMeshProUGUI>().text = saveObject.recordname + ":    " + saveObject.recordscore;

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private class SaveObject {
        public string recordname;
        public int recordscore;
    }
}
