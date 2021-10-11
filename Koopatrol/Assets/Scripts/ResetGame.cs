using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{
    bool paused = true;
    void Start()
    {
        Time.timeScale = 0;   
    }
    public void restartGame()
    {
        Map.Enemies.Clear();
        Map.Tiles.Clear();
        Map.bowserPlaced = false;
        SceneManager.LoadScene("BowsersCastle");
        
        
    }

    public void pauseGame(){
        if(!paused){
            paused = true;
            Time.timeScale = 0; 
            gameObject.GetComponentInChildren<Text>().text = "Resume Game";
        }
        else{
            paused = false;
            Time.timeScale = 1;   
            gameObject.GetComponentInChildren<Text>().text = "Pause Game";
        }
    }
}
