using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaAttack : MonoBehaviour
{
    int damage;
    int damageperc;
    float freezetime;
    float progress = 0f;
    bool isClone = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isClone)
        {
            if (progress <= 10)
            {
                transform.localScale = new Vector3(transform.localScale.x + (10 * Time.deltaTime), transform.localScale.y + (10 * Time.deltaTime), 1);
                progress += Time.deltaTime;
                List<GameObject> enemies = new List<GameObject>();
                enemies.AddRange(Map.Enemies);
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone && !enemy.GetComponent<EnemyHealth>().HitByLava && Vector3.Distance(enemy.transform.position, transform.position) <= 50 * transform.localScale.x && Vector3.Distance(enemy.transform.position, transform.position) >= 50 * (transform.localScale.x - (10 * Time.deltaTime)) - 10)
                    {
                        enemy.GetComponent<EnemyHealth>().HitByLava = true;
                        if (damage != 0) enemy.GetComponent<EnemyHealth>().Hurt(damage);
                        else enemy.GetComponent<EnemyHealth>().Hurt(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(enemy.GetComponent<EnemyHealth>().MaxHealth) / 100 * damageperc)));
                        if (freezetime != 0) enemy.GetComponent<EnemyBehaviour>().Freeze(freezetime, true);
                    }
                }
            }
            else if (progress >= 10 && progress <= 15)
            {
                Destroy(gameObject);
            }
        }
    }
    public static void CreateLavaAttack(GameObject Castle, GameObject Attack, int damage, int damageperc, float freezetime)
    {
        GameObject LavaAttack = Instantiate(Attack);
        LavaAttack.GetComponent<LavaAttack>().damage = damage;
        LavaAttack.GetComponent<LavaAttack>().damageperc = damageperc;
        LavaAttack.GetComponent<LavaAttack>().freezetime = freezetime;
        LavaAttack.transform.position = Castle.transform.position;
        LavaAttack.transform.SetParent(GameObject.FindGameObjectWithTag("Map").transform, true);
        LavaAttack.GetComponent<LavaAttack>().isClone = true;
        LavaAttack.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        LavaAttack.transform.localScale = new Vector3(0, 0, 1);
    }
}
