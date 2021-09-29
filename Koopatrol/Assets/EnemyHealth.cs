using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int enemyCoin = 1;
    public int HealthEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("enemyhealth" + HealthEnemy);
        if(HealthEnemy == 0)
        {
            death();
        }

        //test death
        if (Input.GetKey(KeyCode.Space))
        {
            death();
        }
    }

    void death()
    {
        Assets.CoinCounter.ChangeCoinCounter(enemyCoin);
        Destroy(gameObject);
    }

}
