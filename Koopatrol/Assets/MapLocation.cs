using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapLocation : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int towerSellCost = 0;
    GameObject draggingTower;
    GameObject towerInfo;
    public string towerType = "none";
    public string description = "";
    //1 = path left or right, and not above or below
    //2 = path up or down, and not to sides
    //0 = any other situation
    int isNextToPath = 0;
    int cooldown = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (gameObject.tag == "Tower" || gameObject.tag == "PathTower")
            {
                towerInfo.GetComponent<TowerInfo>().slide = 1;
                towerInfo.GetComponent<TowerInfo>().selectedTower = gameObject;
            }
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
        if (eventData.pointerDrag != null)
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
        if (cooldown != 0) cooldown -= 1;
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
        }
    }
    void GoombaTower()
    {

    }
    void KoopaTower()
    {

    }
    void BulletBlaster()
    {
        if (isNextToPath == 1)
        {
            //leftright
        }
        else if (isNextToPath == 2)
        {
            //updown
        }
        else
        {
            Debug.Log("Bullet blaster placed on non-valid location; destroying.");
            DestroyTower();
        }
    }
}
