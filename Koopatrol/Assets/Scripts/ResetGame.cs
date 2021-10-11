using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    bool paused = false;
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
            
        }
        else{
            paused = false;
            Time.timeScale = 1;      
        }
    }
}
