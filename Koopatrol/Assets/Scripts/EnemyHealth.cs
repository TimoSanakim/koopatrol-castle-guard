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
    bool dying = false;
    float deathTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        if (dying)
        {
            deathTime += Time.deltaTime;
            if (deathTime >= 1f && deathTime <= 1.5f) gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (200 * Time.deltaTime), gameObject.transform.position.z);
            else if (deathTime > 1.5f) gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (200 * Time.deltaTime), gameObject.transform.position.z);
            if (deathTime >= 4f) Destroy(gameObject);
        }
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
        if (GetComponent<EnemyBehaviour>().enemyType == "Mario")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayNew("Victory");
            GetComponent<EnemyBehaviour>().moveSpeed = 0f;
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets) { bullet.GetComponent<Assets.Bullet>().timeFlying = 3599; }
            dying = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
