using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float frozenTime = 0;
    public GameObject enemyOriginal;
    public string enemyType;
    GameObject CastleHealth;

    int moveDirection = 0;
    int lastDirection = 0;
    int offsetDirection = -1;

    public bool isClone;


    // Array of waypoints to walk from one to the next one
    [SerializeField]
    List<Transform> Paths = new List<Transform>();
    List<Transform> PastPaths = new List<Transform>();
    Transform NextPath = null;
    public GameObject startingPosition;
    float specialBehavior = 0f;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Use this for initialization
    private void Start()
    {
        if (isClone)
        {
            CastleHealth = GameObject.FindGameObjectWithTag("CastleHealth");

            GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
            GameObject[] obstructedPaths = GameObject.FindGameObjectsWithTag("PathTower");
            foreach (GameObject path in paths)
            {
                Paths.Add(path.transform);
            }
            foreach (GameObject path in obstructedPaths)
            {
                Paths.Add(path.transform);
            }
            // Set position of Enemy as position of the first waypoint
            transform.position = startingPosition.transform.position;
            NextPath = startingPosition.transform;

        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isClone)
        {
            if (enemyType == "Luigi" && specialBehavior != 0) Luigi();
            if (frozenTime <= 2 && specialBehavior == 0)
            {
                // Move Enemy
                Move();
            }
            if (frozenTime != 0)
            {
                frozenTime -= 1 * Time.deltaTime;
                if (frozenTime < 0) frozenTime = 0;
            }
        }
    }

    public void Freeze(float seconds)
    {
        if (frozenTime == 0)
        {
            frozenTime = 2 + seconds;
        }
    }

    bool LookForTower(string towerType, float x, float y)
    {
        bool found = false;
        bool stop = false;
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] pathTowers = GameObject.FindGameObjectsWithTag("PathTower");
        List<GameObject> allTowers = new List<GameObject>();
        foreach (GameObject tower in towers)
        {
            allTowers.Add(tower);
        }
        foreach (GameObject tower in pathTowers)
        {
            allTowers.Add(tower);
        }
        switch (moveDirection)
        {
            case 0:
                while (!stop)
                {
                    y += 50;
                    stop = true;
                    foreach (GameObject path in paths)
                    {
                        if (x - path.transform.position.x >= -10 && x - path.transform.position.x <= 10 && y - path.transform.position.y >= -10 && y - path.transform.position.y <= 10)
                        {
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in allTowers)
                    {
                        if (x - tower.transform.position.x >= -10 && x - tower.transform.position.x <= 10 && y - tower.transform.position.y >= -10 && y - tower.transform.position.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType) found = true;
                            break;
                        }
                    }
                }
                break;
            case 1:
                while (!stop)
                {
                    y -= 50;
                    stop = true;
                    foreach (GameObject path in paths)
                    {
                        if (x - path.transform.position.x >= -10 && x - path.transform.position.x <= 10 && y - path.transform.position.y >= -10 && y - path.transform.position.y <= 10)
                        {
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in allTowers)
                    {
                        if (x - tower.transform.position.x >= -10 && x - tower.transform.position.x <= 10 && y - tower.transform.position.y >= -10 && y - tower.transform.position.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType) found = true;
                            break;
                        }
                    }
                }
                break;
            case 2:
                while (!stop)
                {
                    x += 50;
                    stop = true;
                    foreach (GameObject path in paths)
                    {
                        if (x - path.transform.position.x >= -10 && x - path.transform.position.x <= 10 && y - path.transform.position.y >= -10 && y - path.transform.position.y <= 10)
                        {
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in allTowers)
                    {
                        if (x - tower.transform.position.x >= -10 && x - tower.transform.position.x <= 10 && y - tower.transform.position.y >= -10 && y - tower.transform.position.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType) found = true;
                            break;
                        }
                    }
                }
                break;
            case 3:
                while (!stop)
                {
                    x -= 50;
                    stop = true;
                    foreach (GameObject path in paths)
                    {
                        if (x - path.transform.position.x >= -10 && x - path.transform.position.x <= 10 && y - path.transform.position.y >= -10 && y - path.transform.position.y <= 10)
                        {
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in allTowers)
                    {
                        if (x - tower.transform.position.x >= -10 && x - tower.transform.position.x <= 10 && y - tower.transform.position.y >= -10 && y - tower.transform.position.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType) found = true;
                            break;
                        }
                    }
                }
                break;
        }
        return found;
    }

    void Luigi()
    {
        if (specialBehavior <= .4f)
        {
            switch (lastDirection)
            {
                case 0:
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.position.z);
                    break;
                case 1:
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.position.z);
                    break;
                case 2:
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z);
                    break;
                case 3:
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.position.z);
                    break;
            }
        }
        offsetDirection = lastDirection;
        specialBehavior -= Time.deltaTime;
        if (specialBehavior <= 0)
        {
            specialBehavior = 0f;
            switch (lastDirection)
            {
                case 0:
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, NextPath.transform.position.y - 25, gameObject.transform.position.z);
                    break;
                case 1:
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, NextPath.transform.position.y + 25, gameObject.transform.position.z);
                    break;
                case 2:
                    gameObject.transform.position = new Vector3(NextPath.transform.position.x - 25, gameObject.transform.position.y, gameObject.transform.position.z);
                    break;
                case 3:
                    gameObject.transform.position = new Vector3(NextPath.transform.position.x + 25, gameObject.transform.position.z);
                    break;
            }
        }
    }

    private void Move()
    {
        if (Paths.Count >= 1)
        {
            bool atPath = false;
            if (transform.position == NextPath.transform.position) atPath = true;
            if (transform.position.y >= NextPath.transform.position.y && moveDirection == 0) atPath = true;
            if (transform.position.y <= NextPath.transform.position.y && moveDirection == 1) atPath = true;
            if (transform.position.x >= NextPath.transform.position.x && moveDirection == 2) atPath = true;
            if (transform.position.x <= NextPath.transform.position.x && moveDirection == 3) atPath = true;
            if (atPath)
            {
                PastPaths.Add(NextPath);
                Paths.Remove(NextPath);
                NextPath = null;
                List<Transform> possiblePaths = new List<Transform>();
                float x = transform.position.x;
                float y = transform.position.y;
                if (offsetDirection == 0) y += 25;
                else if (offsetDirection == 1) y -= 25;
                else if (offsetDirection == 2) x += 25;
                else if (offsetDirection == 3) x -= 25;
                foreach (Transform path in Paths)
                {
                    if ((path.position.x - x >= -10 && path.position.x - x <= 10 && path.position.y - y >= -70 && path.position.y - y <= 70) || (path.position.y - y >= -10 && path.position.y - y <= 10 && path.position.x - x >= -70 && path.position.x - x <= 70)) possiblePaths.Add(path);
                }
                if (possiblePaths.Count != 0)
                {
                    var r = new System.Random();
                    if (possiblePaths.Count == 1) NextPath = possiblePaths[0];
                    else NextPath = possiblePaths[r.Next(0, possiblePaths.Count)];
                    float diffx = NextPath.position.x - x;
                    float diffy = NextPath.position.y - y;
                    lastDirection = moveDirection;
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
                    if (moveDirection != lastDirection && enemyType == "Luigi" && transform.position != startingPosition.transform.position)
                    {
                        offsetDirection = -1;
                        if (LookForTower("BulletBlaster", x, y)) specialBehavior = 1.5f;
                    }
                }
            }
            if (NextPath != null)
            {
                switch (moveDirection)
                {
                    case 0:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (moveSpeed * Time.deltaTime), gameObject.transform.position.z);
                        if (transform.position.y >= NextPath.transform.position.y) gameObject.transform.position = new Vector3(transform.position.x, NextPath.transform.position.y, transform.position.z);
                        break;
                    case 1:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (moveSpeed * Time.deltaTime), gameObject.transform.position.z);
                        if (transform.position.y <= NextPath.transform.position.y) gameObject.transform.position = new Vector3(transform.position.x, NextPath.transform.position.y, transform.position.z);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x + (moveSpeed * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z);
                        if (transform.position.x >= NextPath.transform.position.x) gameObject.transform.position = new Vector3(NextPath.transform.position.x, transform.position.y, transform.position.z);
                        break;
                    case 3:
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x - (moveSpeed * Time.deltaTime), gameObject.transform.position.z);
                        if (transform.position.x <= NextPath.transform.position.x) gameObject.transform.position = new Vector3(NextPath.transform.position.x, transform.position.y, transform.position.z); 
                        break;
                }
            }
            else
            {
                CastleHealth.GetComponent<CastleHealth>().HealthCastle -= gameObject.GetComponent<EnemyHealth>().HealthEnemy;
                Destroy(gameObject);
            }
        }
    }
}