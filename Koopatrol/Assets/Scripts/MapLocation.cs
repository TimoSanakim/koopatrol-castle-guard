using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapLocation : MonoBehaviour, IDropHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    static GameObject bulletOriginal;
    static GameObject bulletList;
    public int towerSellCost = 0;
    public int towerLevel = 0;
    GameObject draggingTower;
    GameObject towerInfo;
    GameObject map;
    public string towerType = "none";
    public string description = "";
    //1 = path left or right, and not above or below
    //2 = path up or down, and not to sides
    //0 = any other situation
    public int isNextToPath = 0;
    Sprite originalImage;
    float cooldown = 0;
    bool wasDragging = false;
    public bool towerBuffed = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if ((gameObject.tag == "Tower" || gameObject.tag == "PathTower") && wasDragging == false)
            {
                towerInfo.GetComponent<TowerInfo>().ShowInfo();
                towerInfo.GetComponent<TowerInfo>().selectedTower = gameObject;
            }
            wasDragging = false;
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle click");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right click");
        }
    }
    public void SellTower()
    {
        Assets.CoinCounter.ChangeCoinCounter(towerSellCost);
        DestroyTower();
    }
    public void DestroyTower()
    {
        if (towerType == "Bowser") map.GetComponent<Map>().bowserPlaced = false;
        gameObject.GetComponent<Image>().sprite = null;
        Color temp = Color.white;
        temp.a = 0f;
        gameObject.GetComponent<Image>().color = temp;
        towerType = "none";
        towerSellCost = 0;
        cooldown = 0;
        towerLevel = 0;
        description = "";
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        if (gameObject.tag == "PathTower")
        {
            gameObject.tag = "Path";
        }
        else
        {
            gameObject.tag = "Ground";
        }
        gameObject.GetComponent<Image>().sprite = originalImage;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && draggingTower.GetComponent<draggingTower>().towerType != "none")
        {
            if (draggingTower.GetComponent<draggingTower>().towerCost > Assets.CoinCounter.GetCoinCount())
            {
                Debug.Log("Not enough money to place tower");
            }
            else if ((gameObject.tag == "Ground") && (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.AnyGround || (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath != 0)))
            {
                PlaceTower();
                gameObject.tag = "Tower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost);
            }
            else if ((gameObject.tag == "Path") && draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.Path)
            {
                PlaceTower();
                gameObject.tag = "PathTower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost);
            }
        }
        draggingTower.GetComponent<draggingTower>().towerType = "none";
    }
    void PlaceTower()
    {
        gameObject.GetComponent<Image>().sprite = draggingTower.GetComponent<Image>().sprite;
        gameObject.GetComponent<Image>().color = draggingTower.GetComponent<Image>().color;
        towerType = draggingTower.GetComponent<draggingTower>().towerType;
        towerSellCost = draggingTower.GetComponent<draggingTower>().towerSellCost;
        towerLevel = 1;
        cooldown = 0;
        if (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath == 2) gameObject.GetComponent<Image>().sprite = draggingTower.GetComponent<draggingTower>().yTowerImage;
        if (towerType == "Bowser") map.GetComponent<Map>().bowserPlaced = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        bulletOriginal = GameObject.FindGameObjectWithTag("Bullet");
        bulletList = GameObject.FindGameObjectWithTag("BulletList");
        map = GameObject.FindGameObjectWithTag("Map");
        originalImage = gameObject.GetComponent<Image>().sprite;
        if (gameObject.tag == "Ground" || gameObject.tag == "Tower")
        {
            GameObject[] field = GameObject.FindGameObjectsWithTag("Path");
            bool xPath = false;
            bool yPath = false;
            foreach (GameObject path in field)
            {
                if (gameObject.transform.position.x - path.transform.position.x >= 30 && gameObject.transform.position.x - path.transform.position.x <= 70 && gameObject.transform.position.y == path.transform.position.y)
                {
                    xPath = true;
                }
                else if (gameObject.transform.position.x - path.transform.position.x >= -70 && gameObject.transform.position.x - path.transform.position.x <= -30 && gameObject.transform.position.y == path.transform.position.y)
                {
                    xPath = true;
                }
                else if (gameObject.transform.position.y - path.transform.position.y >= 30 && gameObject.transform.position.y - path.transform.position.y <= 70 && gameObject.transform.position.x == path.transform.position.x)
                {
                    yPath = true;
                }
                else if (gameObject.transform.position.y - path.transform.position.y >= -70 && gameObject.transform.position.y - path.transform.position.y <= -30 && gameObject.transform.position.x == path.transform.position.x)
                {
                    yPath = true;
                }
            }
            if (xPath || yPath)
            {
                if (xPath && !yPath)
                {
                    isNextToPath = 1;
                    //gameObject.GetComponent<Image>().color = Color.black;
                }
                else if (yPath && !xPath)
                {
                    isNextToPath = 2;
                    //gameObject.GetComponent<Image>().color = Color.black;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (towerBuffed) towerLevel += 1; 
        if (cooldown != 0) cooldown -= 1 * Time.deltaTime;
        if (cooldown < 0) cooldown = 0;
        switch (towerType)
        {
            case "GoombaTower":
                GoombaTower();
                break;
            case "KoopaTower":
                KoopaTower();
                break;
            case "FreezieTower":
                FreezieTower();
                break;
            case "Thwomp":
                Thwomp();
                break;
            case "BulletBlaster":
                BulletBlaster();
                break;
            case "PiranhaPlant":
                PiranhaPlant();
                break;
            case "MagikoopaTower":
                MagikoopaTower();
                break;
            case "Bowser":
                Bowser();
                break;
        }
        if (towerBuffed) towerLevel -= 1;
        towerBuffed = false;
    }
    void LookAt(Vector3 targetPosition)
    {
        Vector3 difference = targetPosition - gameObject.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
    //Get based on isNextToPath
    GameObject GetEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        int first = 0;
        int second = 0;
        int whileLoop = 0;
        bool hasPath = true;
        GameObject target = null;
        while (whileLoop != 2)
        {
            foreach (GameObject path in paths)
            {
                if (y - path.transform.position.y >= -45 && y - path.transform.position.y <= 45 && x - path.transform.position.x >= -45 && x - path.transform.position.x <= 45)
                {
                    hasPath = true;
                }
            }
            if (hasPath)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone && y - enemy.transform.position.y >= -5 && y - enemy.transform.position.y <= 5 && x - enemy.transform.position.x >= -5 && x - enemy.transform.position.x <= 5)
                    {
                        if (whileLoop == 0 && target == null) target = enemy;
                        else if (whileLoop == 1 && target == null) target = enemy;
                        else if (whileLoop == 1 && first < second) target = enemy;
                        x = gameObject.transform.position.x;
                        y = gameObject.transform.position.y;
                        whileLoop += 1;
                    }
                }
            }
            else
            {
                x = gameObject.transform.position.x;
                y = gameObject.transform.position.y;
                whileLoop += 1;
            }
            if (isNextToPath == 1)
            {
                if (whileLoop == 0) x -= 10;
                else x += 10;
            }
            else if (isNextToPath == 2)
            {
                if (whileLoop == 0) y += 10;
                else y -= 10;
            }
            if (whileLoop == 0) first += 1;
            else second += 1;
            hasPath = false;
        }
        return target;
    }
    //Circular range
    GameObject GetEnemy(float range)
    {
        GameObject target = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float lowestDistance = range;
        float distance;
        foreach (GameObject enemy in enemies)
        {
            distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
            if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance)
            {
                target = enemy;
                lowestDistance = distance;
            }
        }
        return target;
    }
    GameObject GetEnemyOnPath()
    {
        GameObject target = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float lowestDistance = 5;
        float distance;
        foreach (GameObject enemy in enemies)
        {
            distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
            if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance)
            {
                target = enemy;
                lowestDistance = distance;
            }
        }
        return target;
    }
    void CreateBullet(int image, int power, float speed, GameObject homingTarget)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().homingTarget = homingTarget;
        bullet.GetComponent<Assets.Bullet>().power = power;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void CreateBullet(int image, int power, float speed, Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().targetPosition = targetPosition;
        bullet.GetComponent<Assets.Bullet>().power = power;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void CreateBullet(int image, float freezeTime, float speed, GameObject homingTarget)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().homingTarget = homingTarget;
        bullet.GetComponent<Assets.Bullet>().freezeAmount = freezeTime;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void CreateBullet(int image, float freezeTime, float speed, Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().targetPosition = targetPosition;
        bullet.GetComponent<Assets.Bullet>().freezeAmount = freezeTime;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void GoombaTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.GoomaTower.GetRange(towerLevel));
            if (enemy != null)
            {
                cooldown = Assets.GoomaTower.GetCooldown(towerLevel);
                CreateBullet(Assets.GoomaTower.bulletImage, Assets.GoomaTower.GetDamage(towerLevel), Assets.GoomaTower.GetSpeed(towerLevel), enemy.transform.position);
            }
        }
    }
    void KoopaTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.KoopaTower.GetRange(towerLevel));
            if (enemy != null)
            {
                cooldown = Assets.KoopaTower.GetCooldown(towerLevel);
                CreateBullet(Assets.KoopaTower.bulletImage, Assets.KoopaTower.GetDamage(towerLevel), Assets.KoopaTower.GetSpeed(towerLevel), enemy.transform.position);
            }
        }
    }
    void FreezieTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.FreezieTower.GetRange(towerLevel));
            if (enemy != null)
            {
                cooldown = Assets.FreezieTower.GetCooldown(towerLevel);
                CreateBullet(Assets.FreezieTower.bulletImage, Assets.FreezieTower.GetFreezeTime(towerLevel), Assets.FreezieTower.GetSpeed(towerLevel), enemy.transform.position);
            }
        }
    }
    void Thwomp()
    {
        if (cooldown == 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().isClone && Vector3.Distance(enemy.transform.position, gameObject.transform.position) < Assets.Thwomp.GetRange(towerLevel))
                {
                    cooldown = Assets.Thwomp.GetCooldown(towerLevel);
                    enemy.GetComponent<EnemyBehaviour>().Stagger(Assets.Thwomp.GetStaggerTime(towerLevel));
                    enemy.GetComponent<EnemyBehaviour>().Freeze(-1);
                }
            }
        }
    }
    void BulletBlaster()
    {
        if (isNextToPath == 1)
        {
            if (cooldown == 0)
            {
                GameObject enemy = GetEnemy();
                if (enemy != null)
                {
                    cooldown = Assets.BulletBlaster.GetCooldown(towerLevel);
                    CreateBullet(Assets.BulletBlaster.bulletImage, Assets.BulletBlaster.GetDamage(towerLevel), Assets.BulletBlaster.GetSpeed(towerLevel), new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
                }
            }
        }
        else if (isNextToPath == 2)
        {
            if (cooldown == 0)
            {
                GameObject enemy = GetEnemy();
                if (enemy != null)
                {
                    cooldown = Assets.BulletBlaster.GetCooldown(towerLevel);
                    CreateBullet(Assets.BulletBlaster.bulletImage, Assets.BulletBlaster.GetDamage(towerLevel), Assets.BulletBlaster.GetSpeed(towerLevel), new Vector3(transform.position.x, enemy.transform.position.y, enemy.transform.position.z));
                }
            }
        }
        else
        {
            Debug.Log("Bullet blaster placed on non-valid location; destroying.");
            DestroyTower();
        }
    }
    void PiranhaPlant()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemyOnPath();
            if (enemy != null)
            {
                cooldown = Assets.PiranhaPlant.GetCooldown(towerLevel);
                enemy.GetComponent<EnemyBehaviour>().Stagger(Assets.PiranhaPlant.GetStaggerTime(towerLevel));
                enemy.GetComponent<EnemyBehaviour>().Freeze(-1);
                enemy.GetComponent<EnemyHealth>().Hurt(Assets.PiranhaPlant.GetDamage(towerLevel));
            }
        }
    }
    void MagikoopaTower()
    {
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
        foreach (GameObject tower in allTowers)
        {
            if (Vector3.Distance(tower.transform.position, gameObject.transform.position) <= Assets.MagikoopaTower.GetRange(towerLevel))
            {
                tower.GetComponent<MapLocation>().towerBuffed = true;
            }
        }
    }
    void Bowser()
    {
        GameObject enemy = GetEnemy(Assets.Bowser.GetRange(towerLevel));
        LookAt(enemy.transform.position);
        if (cooldown == 0)
        {
            if (enemy != null)
            {
                cooldown = Assets.Bowser.GetCooldown(towerLevel);
                CreateBullet(Assets.Bowser.bulletImage, Assets.Bowser.GetDamage(towerLevel), Assets.Bowser.GetSpeed(towerLevel), enemy);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        wasDragging = true;
        map.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }
}
