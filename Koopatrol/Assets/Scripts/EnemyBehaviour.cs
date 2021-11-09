using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float frozenTime = 0;
    float hitTime = 0;
    public GameObject enemyOriginal;
    public string enemyType;
    public bool finalEnemy = false;
    GameObject CastleHealth;
    Vector3 previousPosition;
    bool wasPushedBack = false;

    int moveDirection = 0;
    int lastDirection = 0;
    int offsetDirection = -1;

    public bool isClone;


    public List<Transform> Paths = new List<Transform>();
    public List<Transform> PastPaths = new List<Transform>();
    public Transform NextPath = null;
    Transform StartingPosition;
    float specialBehavior = 0f;

    public float moveSpeed = 0f;
    float tornadoTime = 0f;
    public AudioClip spawnSound;
    public AudioClip deathSound;
    public AudioClip specialSound;
    public string GetDescription()
    {
        if (enemyType == "Toad") return "<sprite=6>=" + GetComponent<EnemyHealth>().Health + "/" + GetComponent<EnemyHealth>().MaxHealth + "<sprite=1>=2| Special behavior: None.";
        if (enemyType == "Captain Toad") return "<sprite=6>=" + GetComponent<EnemyHealth>().Health + "/" + GetComponent<EnemyHealth>().MaxHealth + "<sprite=1>=2| Special behavior: None.";
        if (enemyType == "Yoshi") return "<sprite=6>=" + GetComponent<EnemyHealth>().Health + "/" + GetComponent<EnemyHealth>().MaxHealth + "<sprite=1>=3| Special behavior: Fast movement.";
        if (enemyType == "Luigi") return "<sprite=6>=" + GetComponent<EnemyHealth>().Health + "/" + GetComponent<EnemyHealth>().MaxHealth + "<sprite=1>=2| Special behavior: Scared of bullet blasters.";
        if (enemyType == "Bob-Omb Buddy") return "<sprite=6>=" + GetComponent<EnemyHealth>().Health + "/" + GetComponent<EnemyHealth>().MaxHealth + "<sprite=1>=2| Special behavior: Explodes on death, destroying nearby towers.";
        if (enemyType == "Mario") return "<sprite=6>=" + GetComponent<EnemyHealth>().Health + "/" + GetComponent<EnemyHealth>().MaxHealth + "<sprite=1>=2| Special behavior: Destroys goomba towers, removes all castle health when reached.";
        return "<sprite=6>=" + GetComponent<EnemyHealth>().Health + "/" + GetComponent<EnemyHealth>().MaxHealth + "<sprite=1>=2| Special behavior: Unknown.";
    }

    // Use this for initialization
    private void Start()
    {
        if (isClone)
        {
            gameObject.GetComponent<AudioSource>().volume = Convert.ToSingle(Map.SoundVolume) / 100;
            CastleHealth = GameObject.FindGameObjectWithTag("CastleHealth");
            NextPath = Paths[0];
            StartingPosition = Paths[0];
            PastPaths.Add(Paths[0]);
            Paths.RemoveAt(0);
            if (spawnSound != null){
                GetComponent<AudioSource>().clip = spawnSound;
                GetComponent<AudioSource>().Play();
            }
            
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isClone)
        {
            if (frozenTime <= 2 && enemyType == "Luigi" && specialBehavior != 0) Luigi();
            else if (frozenTime <= 2 && enemyType == "Mario" && specialBehavior != 0) Mario();
            else if (tornadoTime == 0 && frozenTime <= 2 && hitTime == 0 && specialBehavior == 0)
            {
                Move();
            }
            else if (tornadoTime != 0 && specialBehavior == 0)
            {
                PushedByTornado();
            }
            if (frozenTime != 0)
            {
                frozenTime -= Time.deltaTime;
                if (frozenTime < 0) frozenTime = 0;
                if (frozenTime <= 2) gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
                else gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
            }
            if (hitTime != 0)
            {
                hitTime -= Time.deltaTime;
                if (hitTime < 0) hitTime = 0;
            }
            if (tornadoTime != 0)
            {
                tornadoTime -= Time.deltaTime;
                if (tornadoTime < 0) tornadoTime = 0;
            }
        }
    }

    public void Freeze(float seconds, bool force)
    {
        if (frozenTime == 0 || force)
        {
            frozenTime = 2 + seconds;
        }
        if (seconds == -1 && frozenTime > 2)
        {
            frozenTime = 2;
        }
    }
    public void Stagger(float duration, bool force)
    {
        if (hitTime == 0)
        {
            hitTime = duration;
        }
        else if (hitTime <= 0.1 && force)
        {
            hitTime = duration;
        }
    }
    public void HitByTornado()
    {
        tornadoTime = 10;
        if (PastPaths.Count >= 1) NextPath = PastPaths[PastPaths.Count - 1];
        if (moveDirection == 0)
        {
            moveDirection = 1;
        }
        else if (moveDirection == 1)
        {
            moveDirection = 0;
        }
        else if (moveDirection == 2)
        {
            moveDirection = 3;
        }
        else if (moveDirection == 3)
        {
            moveDirection = 2;
        }
    }

    bool LookForTower(string towerType, float x, float y)
    {
        bool found = false;
        bool stop = false;
        List<GameObject> paths = new List<GameObject>();
        switch (moveDirection)
        {
            case 0:
                while (!stop)
                {
                    y += 50;
                    stop = true;
                    foreach (GameObject path in Map.Tiles)
                    {
                        if ((path.tag == "Path" || path.tag == "PathTower") && x - path.transform.localPosition.x >= -10 && x - path.transform.localPosition.x <= 10 && y - path.transform.localPosition.y >= -10 && y - path.transform.localPosition.y <= 10)
                        {
                            paths.Add(path);
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in Map.Tiles)
                    {
                        if ((tower.tag == "Tower" || tower.tag == "PathTower") && x - tower.transform.localPosition.x >= -10 && x - tower.transform.localPosition.x <= 10 && y - tower.transform.localPosition.y >= -10 && y - tower.transform.localPosition.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType)
                            {
                                found = true;
                                tower.GetComponent<MapLocation>().highlight = true;
                                foreach (GameObject path in paths)
                                {
                                    path.GetComponent<MapLocation>().highlight = true;
                                }
                            }
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
                    foreach (GameObject path in Map.Tiles)
                    {
                        if ((path.tag == "Path" || path.tag == "PathTower") && x - path.transform.localPosition.x >= -10 && x - path.transform.localPosition.x <= 10 && y - path.transform.localPosition.y >= -10 && y - path.transform.localPosition.y <= 10)
                        {
                            paths.Add(path);
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in Map.Tiles)
                    {
                        if ((tower.tag == "Tower" || tower.tag == "PathTower") && x - tower.transform.localPosition.x >= -10 && x - tower.transform.localPosition.x <= 10 && y - tower.transform.localPosition.y >= -10 && y - tower.transform.localPosition.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType)
                            {
                                found = true;
                                tower.GetComponent<MapLocation>().highlight = true;
                                foreach (GameObject path in paths)
                                {
                                    path.GetComponent<MapLocation>().highlight = true;
                                }
                            }
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
                    foreach (GameObject path in Map.Tiles)
                    {
                        if ((path.tag == "Path" || path.tag == "PathTower") && x - path.transform.localPosition.x >= -10 && x - path.transform.localPosition.x <= 10 && y - path.transform.localPosition.y >= -10 && y - path.transform.localPosition.y <= 10)
                        {
                            paths.Add(path);
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in Map.Tiles)
                    {
                        if ((tower.tag == "Tower" || tower.tag == "PathTower") && x - tower.transform.localPosition.x >= -10 && x - tower.transform.localPosition.x <= 10 && y - tower.transform.localPosition.y >= -10 && y - tower.transform.localPosition.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType)
                            {
                                found = true;
                                tower.GetComponent<MapLocation>().highlight = true;
                                foreach (GameObject path in paths)
                                {
                                    path.GetComponent<MapLocation>().highlight = true;
                                }
                            }
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
                    foreach (GameObject path in Map.Tiles)
                    {
                        if ((path.tag == "Path" || path.tag == "PathTower") && x - path.transform.localPosition.x >= -10 && x - path.transform.localPosition.x <= 10 && y - path.transform.localPosition.y >= -10 && y - path.transform.localPosition.y <= 10)
                        {
                            paths.Add(path);
                            stop = false;
                        }
                    }
                    foreach (GameObject tower in Map.Tiles)
                    {
                        if ((tower.tag == "Tower" || tower.tag == "PathTower") && x - tower.transform.localPosition.x >= -10 && x - tower.transform.localPosition.x <= 10 && y - tower.transform.localPosition.y >= -10 && y - tower.transform.localPosition.y <= 10)
                        {
                            if (tower.GetComponent<MapLocation>().towerType == towerType)
                            {
                                found = true;
                                tower.GetComponent<MapLocation>().highlight = true;
                                foreach (GameObject path in paths)
                                {
                                    path.GetComponent<MapLocation>().highlight = true;
                                }
                            }
                            break;
                        }
                    }
                }
                break;
        }
        return found;
    }

    int TargetTower(string towerType, float x, float y)
    {
        foreach (GameObject spot in Map.Tiles)
        {
            if (x - spot.transform.localPosition.x >= -10 && x - spot.transform.localPosition.x <= 10 && y - spot.transform.localPosition.y >= 40 && y - spot.transform.localPosition.y <= 60)
            {
                if (spot.GetComponent<MapLocation>().towerType == towerType) return 0;
            }
            if (x - spot.transform.localPosition.x >= -10 && x - spot.transform.localPosition.x <= 10 && y - spot.transform.localPosition.y >= -60 && y - spot.transform.localPosition.y <= -40)
            {
                if (spot.GetComponent<MapLocation>().towerType == towerType) return 1;
            }
            if (x - spot.transform.localPosition.x >= 40 && x - spot.transform.localPosition.x <= 60 && y - spot.transform.localPosition.y >= -10 && y - spot.transform.localPosition.y <= 10)
            {
                if (spot.GetComponent<MapLocation>().towerType == towerType) return 2;
            }
            if (x - spot.transform.localPosition.x >= -60 && x - spot.transform.localPosition.x <= -40 && y - spot.transform.localPosition.y >= -10 && y - spot.transform.localPosition.y <= 10)
            {
                if (spot.GetComponent<MapLocation>().towerType == towerType) return 3;
            }
        }
        return -1;
    }

    public void DestroyTowers(float x, float y)
    {
        foreach (GameObject spot in Map.Tiles)
        {
            if (x - spot.transform.localPosition.x >= -25 && x - spot.transform.localPosition.x <= 25 && y - spot.transform.localPosition.y >= 40 && y - spot.transform.localPosition.y <= 60)
            {
                if (spot.GetComponent<MapLocation>().towerLevel >= 1) spot.GetComponent<MapLocation>().DestroyTower();
            }
            if (x - spot.transform.localPosition.x >= -25 && x - spot.transform.localPosition.x <= 25 && y - spot.transform.localPosition.y >= -60 && y - spot.transform.localPosition.y <= -40)
            {
                if (spot.GetComponent<MapLocation>().towerLevel >= 1) spot.GetComponent<MapLocation>().DestroyTower();
            }
            if (x - spot.transform.localPosition.x >= 40 && x - spot.transform.localPosition.x <= 60 && y - spot.transform.localPosition.y >= -25 && y - spot.transform.localPosition.y <= 25)
            {
                if (spot.GetComponent<MapLocation>().towerLevel >= 1) spot.GetComponent<MapLocation>().DestroyTower();
            }
            if (x - spot.transform.localPosition.x >= -60 && x - spot.transform.localPosition.x <= -40 && y - spot.transform.localPosition.y >= -25 && y - spot.transform.localPosition.y <= 25)
            {
                if (spot.GetComponent<MapLocation>().towerLevel >= 1) spot.GetComponent<MapLocation>().DestroyTower();
            }
        }
    }
    void Luigi()
    {
        if (specialBehavior <= .4f)
        {
            switch (lastDirection)
            {
                case 0:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.z);
                    break;
                case 1:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.z);
                    break;
                case 2:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                    break;
                case 3:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
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
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, NextPath.transform.localPosition.y - 25, gameObject.transform.localPosition.z);
                    break;
                case 1:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, NextPath.transform.localPosition.y + 25, gameObject.transform.localPosition.z);
                    break;
                case 2:
                    gameObject.transform.localPosition = new Vector3(NextPath.transform.localPosition.x - 25, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                    break;
                case 3:
                    gameObject.transform.localPosition = new Vector3(NextPath.transform.localPosition.x + 25, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                    break;
            }
        }
    }

    void Mario()
    {
        if (specialBehavior >= 1f)
        {
            switch (offsetDirection)
            {
                case 0:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.z);
                    break;
                case 1:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.z);
                    break;
                case 2:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                    break;
                case 3:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                    break;
            }
        }
        if (specialBehavior <= 1f)
        {
            foreach (GameObject tower in Map.Tiles)
            {
                if (gameObject.transform.localPosition.x - tower.transform.localPosition.x >= -10 && gameObject.transform.localPosition.x - tower.transform.localPosition.x <= 10 && gameObject.transform.localPosition.y - tower.transform.localPosition.y >= -10 && gameObject.transform.localPosition.y - tower.transform.localPosition.y <= 10)
                {
                    if (tower.GetComponent<MapLocation>().towerType == "GoombaTower")
                    {
                        GetComponent<AudioSource>().clip = specialSound;
                        GetComponent<AudioSource>().Play();
                        tower.GetComponent<MapLocation>().DestroyTower();
                    }
                }
            }
        }
        if (specialBehavior <= 1f)
        {
            switch (offsetDirection)
            {
                case 0:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.z);
                    break;
                case 1:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.z);
                    break;
                case 2:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                    break;
                case 3:
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - (moveSpeed * 2 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                    break;
            }
        }
        specialBehavior -= Time.deltaTime;
        if (specialBehavior <= 0)
        {
            specialBehavior = 0f;
            transform.localPosition = previousPosition;
            offsetDirection = -1;
        }
    }
    private void Move()
    {
        if (Paths.Count >= 1)
        {
            bool atPath = false;
            if (transform.localPosition == NextPath.transform.localPosition) atPath = true;
            if (transform.localPosition.y >= NextPath.transform.localPosition.y && moveDirection == 0) atPath = true;
            if (transform.localPosition.y <= NextPath.transform.localPosition.y && moveDirection == 1) atPath = true;
            if (transform.localPosition.x >= NextPath.transform.localPosition.x && moveDirection == 2) atPath = true;
            if (transform.localPosition.x <= NextPath.transform.localPosition.x && moveDirection == 3) atPath = true;
            if (atPath)
            {
                PastPaths.Add(NextPath);
                Paths.Remove(NextPath);
                if (enemyType == "Mario") transform.localPosition = NextPath.transform.localPosition;
                List<Transform> possiblePaths = new List<Transform>();
                float x = transform.localPosition.x;
                float y = transform.localPosition.y;
                if (offsetDirection == 0) y += 25;
                else if (offsetDirection == 1) y -= 25;
                else if (offsetDirection == 2) x += 25;
                else if (offsetDirection == 3) x -= 25;
                if (Paths.Count != 0) NextPath = Paths[0];
                float diffx = NextPath.localPosition.x - x;
                float diffy = NextPath.localPosition.y - y;
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
                if (moveDirection != lastDirection && enemyType == "Luigi" && transform.localPosition != StartingPosition.transform.localPosition && !wasPushedBack)
                {
                    offsetDirection = -1;
                    if (LookForTower("BulletBlaster", x, y))
                    {
                        previousPosition = transform.localPosition;
                        specialBehavior = 1.5f;
                        GetComponent<AudioSource>().clip = specialSound;
                        GetComponent<AudioSource>().Play();
                    }
                }
                else if (enemyType == "Mario")
                {
                    offsetDirection = -1;
                    offsetDirection = TargetTower("GoombaTower", x, y);
                    if (offsetDirection != -1)
                    {
                        previousPosition = transform.localPosition;
                        specialBehavior = 2f;
                    }
                }
                wasPushedBack = false;
            }
            
            if (NextPath != null)
            {
                switch (moveDirection)
                {
                    case 0:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + (moveSpeed * Time.deltaTime), gameObject.transform.localPosition.z);
                        if (transform.localPosition.y >= NextPath.transform.localPosition.y) gameObject.transform.localPosition = new Vector3(transform.localPosition.x, NextPath.transform.localPosition.y, transform.localPosition.z);
                        break;
                    case 1:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - (moveSpeed * Time.deltaTime), gameObject.transform.localPosition.z);
                        if (transform.localPosition.y <= NextPath.transform.localPosition.y) gameObject.transform.localPosition = new Vector3(transform.localPosition.x, NextPath.transform.localPosition.y, transform.localPosition.z);
                        break;
                    case 2:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (moveSpeed * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                        if (transform.localPosition.x >= NextPath.transform.localPosition.x) gameObject.transform.localPosition = new Vector3(NextPath.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case 3:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - (moveSpeed * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                        if (transform.localPosition.x <= NextPath.transform.localPosition.x) gameObject.transform.localPosition = new Vector3(NextPath.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
        else
        {
            CastleHealth.GetComponent<CastleHealth>().HealthCastle -= gameObject.GetComponent<EnemyHealth>().Health;
            if (enemyType == "Mario") CastleHealth.GetComponent<CastleHealth>().HealthCastle = 0;
            Map.Enemies.Remove(gameObject);
            if (GetComponent<EnemyHealth>().towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
            {
                GetComponent<EnemyHealth>().towerInfo.GetComponent<TowerInfo>().HideInfo();
            }
            Destroy(gameObject);
        }
    }
    private void PushedByTornado()
    {
        wasPushedBack = true;
        if (PastPaths.Count >= 1)
        {
            bool atPath = false;
            if (transform.localPosition == NextPath.transform.localPosition) atPath = true;
            if (transform.localPosition.y >= NextPath.transform.localPosition.y && moveDirection == 0) atPath = true;
            if (transform.localPosition.y <= NextPath.transform.localPosition.y && moveDirection == 1) atPath = true;
            if (transform.localPosition.x >= NextPath.transform.localPosition.x && moveDirection == 2) atPath = true;
            if (transform.localPosition.x <= NextPath.transform.localPosition.x && moveDirection == 3) atPath = true;
            if (atPath)
            {
                Paths.Insert(0, NextPath);
                PastPaths.Remove(NextPath);
                NextPath = null;
                float x = transform.localPosition.x;
                float y = transform.localPosition.y;
                if (offsetDirection == 0) y += 25;
                else if (offsetDirection == 1) y -= 25;
                else if (offsetDirection == 2) x += 25;
                else if (offsetDirection == 3) x -= 25;
                if (PastPaths.Count != 0)
                {
                    NextPath = PastPaths[PastPaths.Count - 1];
                    float diffx = NextPath.localPosition.x - x;
                    float diffy = NextPath.localPosition.y - y;
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
                }
            }
            if (NextPath != null)
            {
                switch (moveDirection)
                {
                    case 0:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + (20 * Time.deltaTime), gameObject.transform.localPosition.z);
                        if (transform.localPosition.y >= NextPath.transform.localPosition.y) gameObject.transform.localPosition = new Vector3(transform.localPosition.x, NextPath.transform.localPosition.y, transform.localPosition.z);
                        break;
                    case 1:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - (20 * Time.deltaTime), gameObject.transform.localPosition.z);
                        if (transform.localPosition.y <= NextPath.transform.localPosition.y) gameObject.transform.localPosition = new Vector3(transform.localPosition.x, NextPath.transform.localPosition.y, transform.localPosition.z);
                        break;
                    case 2:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (20 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                        if (transform.localPosition.x >= NextPath.transform.localPosition.x) gameObject.transform.localPosition = new Vector3(NextPath.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case 3:
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - (20 * Time.deltaTime), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
                        if (transform.localPosition.x <= NextPath.transform.localPosition.x) gameObject.transform.localPosition = new Vector3(NextPath.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
        else
        {
            NextPath = StartingPosition.transform;
            foreach (Transform path in Paths)
            {
                if (path.transform.localPosition == NextPath.transform.localPosition)
                {
                    PastPaths.Add(path);
                    Paths.Remove(path);
                    tornadoTime = 0;
                    break;
                }
            }
        }
    }
}
