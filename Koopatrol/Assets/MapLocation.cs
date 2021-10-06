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
    GameObject draggingTower;
    GameObject towerInfo;
    GameObject map;
    public string towerType = "none";
    public string description = "";
    //1 = path left or right, and not above or below
    //2 = path up or down, and not to sides
    //0 = any other situation
    int isNextToPath = 0;
    float cooldown = 0;
    bool wasDragging = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if ((gameObject.tag == "Tower" || gameObject.tag == "PathTower") && wasDragging == false)
            {
                towerInfo.GetComponent<TowerInfo>().slide = 1;
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
    void DestroyTower()
    {
        gameObject.GetComponent<Image>().sprite = null;
        Color temp = Color.white;
        temp.a = 0f;
        gameObject.GetComponent<Image>().color = temp;
        towerType = "none";
        towerSellCost = 0;
        cooldown = 0;
        description = "";
        if (gameObject.tag == "PathTower")
        {
            gameObject.tag = "Path";
        }
        else
        {
            gameObject.tag = "Ground";
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && draggingTower.GetComponent<draggingTower>().towerType != "none")
        {
            if (draggingTower.GetComponent<draggingTower>().towerCost > Assets.CoinCounter.GetCoinCount())
            {
                Debug.Log("Not enough money to place tower");
                draggingTower.GetComponent<draggingTower>().towerType = "none";
            }
            else if ((gameObject.tag == "Ground") && (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.AnyGround || (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath != 0)))
            {
                PlaceTower();
                gameObject.tag = "Tower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost);
                draggingTower.GetComponent<draggingTower>().towerType = "none";
            }
            else if ((gameObject.tag == "Path") && draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.Path)
            {
                PlaceTower();
                gameObject.tag = "PathTower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost);
                draggingTower.GetComponent<draggingTower>().towerType = "none";
            }
        }
    }
    void PlaceTower()
    {
        gameObject.GetComponent<Image>().sprite = draggingTower.GetComponent<Image>().sprite;
        gameObject.GetComponent<Image>().color = draggingTower.GetComponent<Image>().color;
        towerType = draggingTower.GetComponent<draggingTower>().towerType;
        towerSellCost = draggingTower.GetComponent<draggingTower>().towerSellCost;
        cooldown = 0;
        if (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath == 2) gameObject.GetComponent<Image>().sprite = draggingTower.GetComponent<draggingTower>().yTowerImage;
    }

    // Start is called before the first frame update
    void Start()
    {
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        bulletOriginal = GameObject.FindGameObjectWithTag("Bullet");
        bulletList = GameObject.FindGameObjectWithTag("BulletList");
        map = GameObject.FindGameObjectWithTag("Map");
        if (gameObject.tag == "Ground")
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
            case "BulletBlaster":
                BulletBlaster();
                break;
            case "FreezieTower":
                FreezieTower();
                break;
            case "Bowser":
                Bowser();
                break;
        }
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
                if (y - path.transform.position.y >= -50 && y - path.transform.position.y <= 50 && x - path.transform.position.x >= -50 && x - path.transform.position.x <= 50)
                {
                    hasPath = true;
                }
            }
            if (hasPath)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (y - enemy.transform.position.y >= -5 && y - enemy.transform.position.y <= 5 && x - enemy.transform.position.x >= -5 && x - enemy.transform.position.x <= 5)
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
            if (distance < lowestDistance)
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
            GameObject enemy = GetEnemy(Assets.GoomaTower.range);
            if (enemy != null)
            {
                cooldown = Assets.GoomaTower.cooldown;
                CreateBullet(Assets.GoomaTower.bulletImage, Assets.GoomaTower.damage, Assets.GoomaTower.speed, enemy.transform.position);
            }
        }
    }
    void KoopaTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.KoopaTower.range);
            if (enemy != null)
            {
                cooldown = Assets.KoopaTower.cooldown;
                CreateBullet(Assets.KoopaTower.bulletImage, Assets.KoopaTower.damage, Assets.KoopaTower.speed, enemy.transform.position);
            }
        }
    }
    void BulletBlaster()
    {
        if (isNextToPath != 0)
        {
            if (cooldown == 0)
            {
                GameObject enemy = GetEnemy();
                if (enemy != null)
                {
                    cooldown = Assets.BulletBlaster.cooldown;
                    CreateBullet(Assets.BulletBlaster.bulletImage, Assets.BulletBlaster.damage, Assets.BulletBlaster.speed, enemy.transform.position);
                }
            }
        }
        else
        {
            Debug.Log("Bullet blaster placed on non-valid location; destroying.");
            DestroyTower();
        }
    }
    void FreezieTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.FreezieTower.range);
            if (enemy != null)
            {
                cooldown = Assets.FreezieTower.cooldown;
                CreateBullet(Assets.FreezieTower.bulletImage, Assets.FreezieTower.freezeTime, Assets.FreezieTower.speed, enemy.transform.position);
            }
        }
    }
    void Bowser()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.Bowser.range);
            if (enemy != null)
            {
                cooldown = Assets.Bowser.cooldown;
                CreateBullet(Assets.Bowser.bulletImage, Assets.Bowser.damage, Assets.Bowser.speed, enemy);
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
