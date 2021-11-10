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
        while (i < 3)
        {

            if (File.Exists(Application.dataPath + "/recorddata" + i))
            {
                string saveString = File.ReadAllText(Application.dataPath + "/recorddata" + i);
                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
                recordnames[i].GetComponent<TextMeshProUGUI>().text = saveObject.recordname + ":    " + saveObject.recordscore;
            }
            i++;
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
