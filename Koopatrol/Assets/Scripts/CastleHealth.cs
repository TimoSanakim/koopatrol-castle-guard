using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CastleHealth : MonoBehaviour
{
    bool castleDead = false;
    public int HealthCastle;

    // Start is called before the first frame update
    void Start()
    {
        HealthCastle = Convert.ToInt32(gameObject.GetComponent<Text>().text);
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthCastle <= 0)
        {
            HealthCastle = 0;
            if (!castleDead)
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayNew("GameOver");
                castleDead = true;
            }
        }
        gameObject.GetComponent<Text>().text = Convert.ToString(HealthCastle);
    }
}
