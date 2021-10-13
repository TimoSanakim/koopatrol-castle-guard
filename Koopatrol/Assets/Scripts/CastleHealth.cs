using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CastleHealth : MonoBehaviour
{
    public static bool castleDead = false;
    public int HealthCastle;
    GameObject Music;

    // Start is called before the first frame update
    void Start()
    {
        HealthCastle = Convert.ToInt32(gameObject.GetComponent<Text>().text);
        Music = GameObject.FindGameObjectWithTag("Music");
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthCastle <= 0)
        {
            HealthCastle = 0;
            if (!castleDead)
            {
                Music.GetComponent<Music>().PlayNew("GameOver");
                castleDead = true;
                Map.gameSpeed = 0;
                Time.timeScale = Map.gameSpeed;
            }
        }
        if (!Music.GetComponent<AudioSource>().isPlaying && castleDead)
        {
            GameSettings.restartGame();
        }
        gameObject.GetComponent<Text>().text = Convert.ToString(HealthCastle);
    }
}
