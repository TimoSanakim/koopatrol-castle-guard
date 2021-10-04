using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float frozenTime = 0;
    public GameObject enemyOriginal;

    public GameObject CastleHealth;

    public bool isClone;


    // Array of waypoints to walk from one to the next one
    [SerializeField]
    public List<Transform> Paths = new List<Transform>();
    public List<Transform> PastPaths = new List<Transform>();
    public Transform NextPath = null;
    public GameObject startingPosition;

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
            if (frozenTime <= 2)
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

    private void Move()
    {
        if (Paths.Count >= 1)
        {
            if (transform.position == NextPath.transform.position)
            {
                PastPaths.Add(NextPath);
                Paths.Remove(NextPath);
                NextPath = null;
                List<Transform> possiblePaths = new List<Transform>();
                foreach (Transform path in Paths)
                {
                    if ((path.position.x - gameObject.transform.position.x == 0 && path.position.y - gameObject.transform.position.y >= -70 && path.position.y - gameObject.transform.position.y <= 70) || (path.position.y - gameObject.transform.position.y == 0 && path.position.x - gameObject.transform.position.x >= -70 && path.position.x - gameObject.transform.position.x <= 70)) possiblePaths.Add(path);
                }
                if (possiblePaths.Count != 0)
                {
                    var r = new System.Random();
                    NextPath = possiblePaths[r.Next(0, possiblePaths.Count)];
                }
            }
            if (NextPath != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, NextPath.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                CastleHealth.GetComponent<CastleHealth>().HealthCastle -= gameObject.GetComponent<EnemyHealth>().HealthEnemy;
                Destroy(gameObject);
            }
        }
    }
}
