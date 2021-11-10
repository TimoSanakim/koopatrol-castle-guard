using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    GameObject LastResortAttack;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] TowerOptions = GameObject.FindGameObjectsWithTag("TowerOption");
        foreach (GameObject option in TowerOptions)
        {
            if (option.GetComponent<LastResortAttack>() != null) LastResortAttack = option;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            Assets.CoinCounter.ChangeCoinCounter(50, true);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Map.bowserPlaced = false;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LastResortAttack.GetComponent<LastResortAttack>().used = false;
            foreach (GameObject enemy in Map.Enemies)
            {
                enemy.GetComponent<EnemyHealth>().HitByLava = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Map.Enemies.Count != 0) Map.Enemies[0].GetComponent<EnemyHealth>().Hurt(100000);
        }*/
    }
}
