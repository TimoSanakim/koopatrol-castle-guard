using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CastleHealth : MonoBehaviour
{

    public int HealthCastle;

    // Start is called before the first frame update
    void Start()
    {
        HealthCastle = Convert.ToInt32(gameObject.GetComponent<Text>().text);
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthCastle < 0) HealthCastle = 0;
        gameObject.GetComponent<Text>().text = Convert.ToString(HealthCastle);
    }
}
