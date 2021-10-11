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
        
        
    }

    public void Hurt(int damage)
    {
        HealthEnemy -= damage;
        gameObject.GetComponent<EnemyBehaviour>().Stagger(0.1f);
        if (HealthEnemy <= 0)
        {
            death();
        }
    }

    void death()
    {
        Assets.CoinCounter.ChangeCoinCounter(enemyCoin);
        Map.Enemies.Remove(gameObject);
        Destroy(gameObject);
    }

}
