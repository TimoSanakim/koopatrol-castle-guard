using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class levelSelectLoadCustomOptions : MonoBehaviour
{
    public GameObject CustomOption;
    public bool isHighScoreMenu = false;
    int ID = 0;
    // Start is called before the first frame update
    void Start()
    {
        while (File.Exists(Application.dataPath + "/customlevel" + ID))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/customlevel" + ID);
            levelCreator.SaveObject saveObject = JsonUtility.FromJson<levelCreator.SaveObject>(saveString);
            GameObject newOption = Instantiate(CustomOption);
            newOption.transform.SetParent(gameObject.transform.GetChild(0).GetChild(0));
            newOption.transform.localScale = new Vector3(1, 1, 1);
            if (isHighScoreMenu) newOption.transform.localPosition = new Vector3(CustomOption.transform.localPosition.x, CustomOption.transform.localPosition.y - 150 - (50 * ID), CustomOption.transform.localPosition.z);
            else newOption.transform.localPosition = new Vector3(CustomOption.transform.localPosition.x, CustomOption.transform.localPosition.y - 200 - (50 * ID), CustomOption.transform.localPosition.z);
            newOption.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = saveObject.name;
            newOption.GetComponent<switchScene>().CustomMap = ID;
            newOption.GetComponent<CanvasGroup>().alpha = 1;
            newOption.GetComponent<CanvasGroup>().interactable = true;
            newOption.GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (saveObject.gamemode == 0) newOption.GetComponent<LevelSelectionOption>().Description = "A custom map with normal mode!";
            if (saveObject.gamemode == 1) newOption.GetComponent<LevelSelectionOption>().Description = "A custom map with limited-coin mode!";
            if (saveObject.gamemode == 2) newOption.GetComponent<LevelSelectionOption>().Description = "A custom map with endless mode!";
            ID++;
            
        }
        if (isHighScoreMenu) gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x, 150 + 50 * ID);
        else gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x, 200 + 50 * ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
