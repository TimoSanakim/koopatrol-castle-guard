using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;   
    }
    public void restartGame()
    {
        Map.Enemies.Clear();
        Map.Tiles.Clear();
        Map.bowserPlaced = false;
        Map.paused = true;
        Map.gameSpeed = 1;
        SceneManager.LoadScene("BowsersCastle");
        
        
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
        if (!Map.paused) Time.timeScale = Map.gameSpeed;
    }
}
