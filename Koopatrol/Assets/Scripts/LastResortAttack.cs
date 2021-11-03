using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LastResortAttack : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    GameObject towerInfo;
    GameObject Attack;
    GameObject Castle;
    public bool used = false;
    public int costs;
    public int damage;
    public int damageperc;
    public float freezetime;
    float progress = 0f;
    public string description;
    public bool isTornado = false;
    List<Transform> Paths = new List<Transform>();
    List<Transform> PastPaths = new List<Transform>();
    Transform NextPath = null;
    int moveDirection = -1;


    // Start is called before the first frame update
    void Start()
    {
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        Attack = GameObject.FindGameObjectWithTag("LastResortAttack");
        Castle = GameObject.FindGameObjectWithTag("Castle");
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        GameObject[] pathtowers = GameObject.FindGameObjectsWithTag("PathTower");
        Paths.Add(Castle.transform);
        foreach (GameObject path in paths) { Paths.Add(path.transform); }
        foreach (GameObject path in pathtowers) { Paths.Add(path.transform); }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Assets.CoinCounter.GetCoinCount() >= costs && !used)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
        }
        if (used && progress <= 10 && !isTornado)
        {
            if (progress == 0f)
            {
                Attack.transform.position = Castle.transform.position;
                if (Map.gameSpeed != 0) GetComponent<AudioSource>().Play();
            }
            Attack.transform.localScale = new Vector3(Attack.transform.localScale.x + (10 * Time.deltaTime), Attack.transform.localScale.y + (10 * Time.deltaTime), 1);
            progress += Time.deltaTime;
            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(Map.Enemies);
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().isClone && !enemy.GetComponent<EnemyHealth>().HitByLava && Vector3.Distance(enemy.transform.position, Attack.transform.position) <= 50 * Attack.transform.localScale.x && Vector3.Distance(enemy.transform.position, Attack.transform.position) >= 50 * (Attack.transform.localScale.x - (10 * Time.deltaTime)) - 10)
                {
                    enemy.GetComponent<EnemyHealth>().HitByLava = true;
                    if (damage != 0) enemy.GetComponent<EnemyHealth>().Hurt(damage);
                    else enemy.GetComponent<EnemyHealth>().Hurt(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(enemy.GetComponent<EnemyHealth>().MaxHealth) / 100 * damageperc)));
                    if (freezetime != 0) enemy.GetComponent<EnemyBehaviour>().Freeze(freezetime, true);
                }
            }
        }
        else if (used && progress >= 10 && progress <= 15 && !isTornado)
        {
            Attack.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            Attack.transform.localScale = new Vector3(0f, 0f, 0f);
            progress = 20;
        }
        else if (used && isTornado && progress != 20)
        {
            Move();
            Attack.transform.Rotate(0, 0, -2f, Space.Self);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!used && Assets.CoinCounter.GetCoinCount() >= costs)
            {
                used = true;
                Assets.CoinCounter.ChangeCoinCounter(-costs, false);
                Attack.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                if (isTornado)
                {
                    Attack.transform.position = Castle.transform.position;
                    NextPath = Castle.transform;
                    Attack.transform.localScale = new Vector3(1f, 1f, 1f);
                    foreach (Transform path in Paths)
                    {
                        if (path.transform.position == NextPath.transform.position)
                        {
                            PastPaths.Add(path);
                            Paths.Remove(path);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void Reset()
    {
        Attack.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        Attack.transform.localScale = new Vector3(0f, 0f, 0f);
        progress = 0;
        used = false;
        PastPaths.Clear();
        Paths.Clear();
        Castle = GameObject.FindGameObjectWithTag("Castle");
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        GameObject[] pathtowers = GameObject.FindGameObjectsWithTag("PathTower");
        Paths.Add(Castle.transform);
        foreach (GameObject path in paths) { Paths.Add(path.transform); }
        foreach (GameObject path in pathtowers) { Paths.Add(path.transform); }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (towerInfo.GetComponent<TowerInfo>().selectedTower != gameObject) towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
    }
    private void Move()
    {
        if (Paths.Count >= 1)
        {
            bool atPath = false;
            if (Attack.transform.position == NextPath.transform.position) atPath = true;
            if (Attack.transform.position.y >= NextPath.transform.position.y && moveDirection == 0) atPath = true;
            if (Attack.transform.position.y <= NextPath.transform.position.y && moveDirection == 1) atPath = true;
            if (Attack.transform.position.x >= NextPath.transform.position.x && moveDirection == 2) atPath = true;
            if (Attack.transform.position.x <= NextPath.transform.position.x && moveDirection == 3) atPath = true;
            if (atPath)
            {
                PastPaths.Add(NextPath);
                Paths.Remove(NextPath);
                NextPath = null;
                List<Transform> possiblePaths = new List<Transform>();
                float x = Attack.transform.position.x;
                float y = Attack.transform.position.y;
                foreach (Transform path in Paths)
                {
                    if ((path.position.x - x >= -10 && path.position.x - x <= 10 && path.position.y - y >= -70 && path.position.y - y <= 70) || (path.position.y - y >= -10 && path.position.y - y <= 10 && path.position.x - x >= -70 && path.position.x - x <= 70)) possiblePaths.Add(path);
                }
                if (possiblePaths.Count != 0)
                {
                    if (possiblePaths.Count == 1) NextPath = possiblePaths[0];
                    else NextPath = possiblePaths[Map.randomizer.Next(0, possiblePaths.Count)];
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
            }
            if (NextPath != null)
            {
                switch (moveDirection)
                {
                    case 0:
                        Attack.transform.position = new Vector3(Attack.transform.position.x, Attack.transform.position.y + (30 * Time.deltaTime), Attack.transform.position.z);
                        if (Attack.transform.position.y >= NextPath.transform.position.y) Attack.transform.position = new Vector3(Attack.transform.position.x, NextPath.transform.position.y, Attack.transform.position.z);
                        break;
                    case 1:
                        Attack.transform.position = new Vector3(Attack.transform.position.x, Attack.transform.position.y - (30 * Time.deltaTime), Attack.transform.position.z);
                        if (Attack.transform.position.y <= NextPath.transform.position.y) Attack.transform.position = new Vector3(Attack.transform.position.x, NextPath.transform.position.y, Attack.transform.position.z);
                        break;
                    case 2:
                        Attack.transform.position = new Vector3(Attack.transform.position.x + (30 * Time.deltaTime), Attack.transform.position.y, Attack.transform.position.z);
                        if (Attack.transform.position.x >= NextPath.transform.position.x) Attack.transform.position = new Vector3(NextPath.transform.position.x, Attack.transform.position.y, Attack.transform.position.z);
                        break;
                    case 3:
                        Attack.transform.position = new Vector3(Attack.transform.position.x - (30 * Time.deltaTime), Attack.transform.position.y, Attack.transform.position.z);
                        if (Attack.transform.position.x <= NextPath.transform.position.x) Attack.transform.position = new Vector3(NextPath.transform.position.x, Attack.transform.position.y, Attack.transform.position.z);
                        break;
                }
                List<GameObject> enemies = new List<GameObject>();
                enemies.AddRange(Map.Enemies);
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone && !enemy.GetComponent<EnemyHealth>().HitByLava && Vector3.Distance(enemy.transform.position, Attack.transform.position) < 50)
                    {
                        enemy.GetComponent<EnemyHealth>().HitByLava = true;
                        enemy.GetComponent<EnemyHealth>().Hurt(damage);
                        enemy.GetComponent<EnemyBehaviour>().HitByTornado();
                    }
                }
            }
            else
            {
                Attack.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                Attack.transform.localScale = new Vector3(0f, 0f, 0f);
                progress = 20;
            }
        }
    }
}
