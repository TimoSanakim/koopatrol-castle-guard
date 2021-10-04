using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float frozenTime = 0;
    public Transform enemydubes;
    public GameObject enemyToadOriginal;

    int EndWaypoint = 13;

    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Transform[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    // Use this for initialization
    private void Start()
    {
        transform.SetParent(enemydubes, false);
        // Set position of Enemy as position of the first waypoint
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (frozenTime <= 2)
        {
            // Move Enemy
            Move();
            EndOfPath();
        }
        if (frozenTime != 0)
        {
            frozenTime -= 1 * Time.deltaTime;
            if (frozenTime < 0) frozenTime = 0;
        }
    }

    public void Freeze(float seconds)
    {
        if (frozenTime == 0)
        {
            frozenTime = 2 + seconds;
        }
    }

    // Method that actually make Enemy walk
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1)
        {

            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
               transform.position = Vector2.MoveTowards(transform.position,
               waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }

    void EndOfPath()
    {
        if (transform.position == waypoints[EndWaypoint].transform.position)
        {
            CastleHealth.HealthCastle -= 1;
            enemyToadOriginal =  GameObject.Find("enemy(toad)");
            if (enemyToadOriginal.transform.position == waypoints[EndWaypoint].transform.position)
            {
                enemyToadOriginal.transform.position = waypoints[0].transform.position;
               

            }
            else
            {
                
                Destroy(gameObject);
                
            }

        }

    }
}
