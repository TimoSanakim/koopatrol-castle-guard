using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int enemyCoin = 1;
    public int Health;
    int MaxHealth;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void Hurt(int damage)
    {
        Health -= damage;
        gameObject.GetComponent<EnemyBehaviour>().Stagger(0.1f);
        if (Health <= 0)
        {
            death();
        }
        else
        {
            Color newcolor = new Color(1f, Convert.ToSingle(Health-1) / Convert.ToSingle(MaxHealth), Convert.ToSingle(Health-1) / Convert.ToSingle(MaxHealth), 1f);
            gameObject.GetComponent<Image>().color = newcolor;
        }
    }

    void death()
    {
        Assets.CoinCounter.ChangeCoinCounter(enemyCoin);
        Map.Enemies.Remove(gameObject);
        Destroy(gameObject);
    }

}
