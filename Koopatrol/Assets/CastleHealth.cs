using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CastleHealth : MonoBehaviour
{

    public int HealthCastle = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindGameObjectWithTag("CastleHealth").GetComponent<Text>().text = Convert.ToString(HealthCastle);

        if (HealthCastle == 0)
        {
            Destroy(gameObject);
        }
    }
}
