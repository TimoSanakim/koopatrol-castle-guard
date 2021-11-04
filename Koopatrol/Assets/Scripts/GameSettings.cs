using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }
    public static void restartGame()
    {
        Map.Enemies.Clear();
        Map.Tiles.Clear();
        Map.bowserPlaced = false;
        Map.paused = true;
        Map.gameSpeed = 1;
        CastleHealth.castleDead = false;
        Map.Victory = false;
        Assets.CoinCounter.CoinCount = 0;
        SceneManager.LoadScene(Map.LoadedLevel);


    }
    public static void backToMenu()
    {
        Map.Enemies.Clear();
        Map.Tiles.Clear();
        Map.bowserPlaced = false;
        Map.paused = true;
        Map.gameSpeed = 1;
        CastleHealth.castleDead = false;
        Map.Victory = false;
        Assets.CoinCounter.CoinCount = 0;
        Map.LoadedLevel = "MainMenu";
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
    }
    public void changeSoundVolume()
    {
        Map.SoundVolume = Convert.ToInt32(gameObject.GetComponentInChildren<Slider>().value);
        gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Sound volume " + Map.SoundVolume + "%";
    }
    public void changeShownCooldowns()
    {
        Map.ShowCooldowns = Convert.ToInt32(gameObject.GetComponentInChildren<Slider>().value);
        if (Map.ShowCooldowns == 0) gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Show cooldowns all";
        else gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Show cooldowns " + Map.ShowCooldowns + "+";
    }
}
