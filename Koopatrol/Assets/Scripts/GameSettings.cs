using System;
using System.Collections;
using System.Collections.Generic;
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
        Map.LoadedLevel = "MainMenu";
        SceneManager.LoadScene(Map.LoadedLevel);


    }

    public void pauseGame(){
        if(!Map.paused){
            Map.paused = true;
            Time.timeScale = 0;
            gameObject.GetComponentInChildren<Text>().text = "Resume Game";
        }
        else{
            Map.paused = false;
            Time.timeScale = Map.gameSpeed;
            gameObject.GetComponentInChildren<Text>().text = "Pause Game";
        }
    }
    public void changeSpeed()
    {
        Map.gameSpeed = Convert.ToInt32(gameObject.GetComponentInChildren<Slider>().value);
        if (!Map.paused && !CastleHealth.castleDead) Time.timeScale = Map.gameSpeed;
    }
}
