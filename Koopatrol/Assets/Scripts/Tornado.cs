using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tornado : MonoBehaviour
{
    int damage;
    int damageperc;
    float freezetime;
    List<Transform> Paths = new List<Transform>();
    List<Transform> PastPaths = new List<Transform>();
    Transform NextPath = null;
    int moveDirection = -1;
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
            Move();
            transform.Rotate(0, 0, -2f, Space.Self);
        }
    }
    private void Move()
    {
        if (Paths.Count >= 1)
        {
            bool atPath = false;
            if (moveDirection == -1) atPath = true;
            if (gameObject.transform.position.y >= NextPath.transform.position.y && moveDirection == 0) atPath = true;
            if (gameObject.transform.position.y <= NextPath.transform.position.y && moveDirection == 1) atPath = true;
            if (gameObject.transform.position.x >= NextPath.transform.position.x && moveDirection == 2) atPath = true;
            if (gameObject.transform.position.x <= NextPath.transform.position.x && moveDirection == 3) atPath = true;
            if (atPath)
            {
                PastPaths.Add(NextPath);
                Paths.Remove(NextPath);
                float x = gameObject.transform.position.x;
                float y = gameObject.transform.position.y;
                if (Paths.Count != 0) NextPath = Paths[Paths.Count - 1];
                float diffx = NextPath.position.x - x;
                float diffy = NextPath.position.y - y;
                if (diffy > 0)
                {
                    moveDirection = 0;
                }
                else if (diffy < 0)
                {
                    moveDirection = 1;
                }
                else if (diffx > 0)
                {
                    moveDirection = 2;
                }
                else if (diffx < 0)
                {
                    moveDirection = 3;
                }
            }
            if (NextPath != null)
            {
                switch (moveDirection)
                {
                    case 0:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (30 * Time.deltaTime), gameObject.transform.position.z);
                        if (gameObject.transform.position.y >= NextPath.transform.position.y) gameObject.transform.position = new Vector3(gameObject.transform.position.x, NextPath.transform.position.y, gameObject.transform.position.z);
                        break;
                    case 1:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (30 * Time.deltaTime), gameObject.transform.position.z);
                        if (gameObject.transform.position.y <= NextPath.transform.position.y) gameObject.transform.position = new Vector3(gameObject.transform.position.x, NextPath.transform.position.y, gameObject.transform.position.z);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x + (30 * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z);
                        if (gameObject.transform.position.x >= NextPath.transform.position.x) gameObject.transform.position = new Vector3(NextPath.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                        break;
                    case 3:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x - (30 * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z);
                        if (gameObject.transform.position.x <= NextPath.transform.position.x) gameObject.transform.position = new Vector3(NextPath.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                        break;
                }
                List<GameObject> enemies = new List<GameObject>();
                enemies.AddRange(Map.Enemies);
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone && !enemy.GetComponent<EnemyHealth>().HitByLava && Vector3.Distance(enemy.transform.position, gameObject.transform.position) < 50)
                    {
                        enemy.GetComponent<EnemyHealth>().HitByLava = true;
                        if (damage != 0) enemy.GetComponent<EnemyHealth>().Hurt(damage);
                        else enemy.GetComponent<EnemyHealth>().Hurt(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(enemy.GetComponent<EnemyHealth>().MaxHealth) / 100 * damageperc)));
                        if (freezetime != 0) enemy.GetComponent<EnemyBehaviour>().Freeze(freezetime, true);
                        enemy.GetComponent<EnemyBehaviour>().HitByTornado();
                    }
                }
            }
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            Destroy(gameObject);
        }
    }
    public static void CreateTornado(int index, GameObject Attack, int damage, int damageperc, float freezetime)
    {
        GameObject tornado = Instantiate(Attack);
        tornado.GetComponent<Tornado>().damage = damage;
        tornado.GetComponent<Tornado>().damageperc = damageperc;
        tornado.GetComponent<Tornado>().freezetime = freezetime;
        tornado.GetComponent<Tornado>().Paths.AddRange(Map.PossiblePaths[index]);
        tornado.GetComponent<Tornado>().NextPath = tornado.GetComponent<Tornado>().Paths[tornado.GetComponent<Tornado>().Paths.Count - 1];
        tornado.transform.position = tornado.GetComponent<Tornado>().NextPath.position;
        tornado.transform.SetParent(GameObject.FindGameObjectWithTag("Map").transform, true);
        tornado.GetComponent<Tornado>().isClone = true;
        tornado.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        tornado.transform.localScale = new Vector3(1, 1, 1);
        tornado.GetComponent<Tornado>().PastPaths.Add(tornado.GetComponent<Tornado>().Paths[0]);
        tornado.GetComponent<Tornado>().Paths.RemoveAt(0);
    }
}
