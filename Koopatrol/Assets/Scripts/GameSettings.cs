using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        if(File.Exists(Application.dataPath + "/savedata")){
            string saveString = File.ReadAllText(Application.dataPath + "/savedata");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            Map.MusicVolume = saveObject.musicvolume;
            Map.SoundVolume = saveObject.soundvolume;
            Map.DefaultTargetPriority = saveObject.defaultfocus;
            
        }
        Map.paused = true;
        Time.timeScale = 0;
        Map.gameSpeed = 1;
    }
    public static void restartGame()
    {
        Map.Enemies.Clear();
        Map.Tiles.Clear();
        Map.bowserPlaced = false;
        Map.paused = true;
        Time.timeScale = 0;
        Map.gameSpeed = 1;
        CastleHealth.castleDead = false;
        Map.Victory = false;
        Assets.CoinCounter.CoinCount = 0;
        Map.PossiblePaths = null;
        SceneManager.LoadScene(Map.LoadedLevel);
    }
    public static void backToMenu()
    {
        Map.Enemies.Clear();
        Map.Tiles.Clear();
        Map.bowserPlaced = false;
        Map.paused = true;
        Map.gameSpeed = 1;
        Time.timeScale = 0;
        CastleHealth.castleDead = false;
        Map.Victory = false;
        Assets.CoinCounter.CoinCount = 0;
        Map.LoadedLevel = "Levelselect";
        Map.PossiblePaths = null;
        SceneManager.LoadScene(Map.LoadedLevel);
    }

    public void pauseGame()
    {
        if (!Map.paused)
        {
            Map.paused = true;
            Time.timeScale = 0;
            gameObject.GetComponentInChildren<Text>().text = "Resume Game";
        }
        else
        {
            Map.paused = false;
            Time.timeScale = Map.gameSpeed;
            gameObject.GetComponentInChildren<Text>().text = "Pause Game";
            Destroy(GameObject.FindGameObjectWithTag("Startbutton"));
        }
    }
    public void changeSpeed()
    {
        Map.gameSpeed = Convert.ToInt32(gameObject.GetComponentInChildren<Slider>().value);
        if (!Map.paused && !CastleHealth.castleDead) Time.timeScale = Map.gameSpeed;
    }
    public void changeMusicVolume()
    {
        Map.MusicVolume = Convert.ToInt32(gameObject.GetComponentInChildren<Slider>().value);
        gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Music volume " + Map.MusicVolume + "%";
        //TODO: When a scene is loaded, update Music's volume to this value
    }
    public void changeSoundVolume()
    {
        Map.SoundVolume = Convert.ToInt32(gameObject.GetComponentInChildren<Slider>().value);
        gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Sound volume " + Map.SoundVolume + "%";
        //TODO: When a scene is loaded, update all sound sources, except Music's volume to this value
    }
    public void changeShownCooldowns()
    {
        Map.ShowCooldowns = Convert.ToInt32(gameObject.GetComponentInChildren<Slider>().value);
        if (Map.ShowCooldowns == 0) gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Show cooldowns all";
        else gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Show cooldowns " + Map.ShowCooldowns + "+";
    }
    public void changeDefaultTargetPriority()
    {
        Map.DefaultTargetPriority += 1;
        if (Map.DefaultTargetPriority == 6) Map.DefaultTargetPriority = 0;
        switch (Map.DefaultTargetPriority)
        {
            case 0:
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Nearest";
                break;
            case 1:
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Shortest Path";
                break;
            case 2:
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Longest Path";
                break;
            case 3:
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Least Health";
                break;
            case 4:
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Most Health";
                break;
            case 5:
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Most Damage";
                break;
        }
        //TODO: When a scene is loaded, change all MapLocations without any towers to this value
    }
    public void savesettings(){
        SaveObject saveObject = new SaveObject{
        musicvolume = Map.MusicVolume,
        soundvolume = Map.SoundVolume,
        defaultfocus = Map.DefaultTargetPriority,

    };
    string json = JsonUtility.ToJson(saveObject);
    Debug.Log("json"+json);

    SaveObject loadedSaveObject = JsonUtility.FromJson<SaveObject>(json);

    File.WriteAllText(Application.dataPath + "/savedata",json);
    }
    private class SaveObject {
        public int musicvolume;
        public int soundvolume;
        public int defaultfocus;
    }
}
