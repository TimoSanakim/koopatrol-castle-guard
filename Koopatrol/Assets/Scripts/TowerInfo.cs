using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour
{
    bool hidden = true;
    public byte slide = 0;
    public GameObject selectedTower;
    GameObject sellButton;
    GameObject upgradeButton;
    GameObject towerDescription;
    // Start is called before the first frame update
    void Start()
    {
        sellButton = GameObject.FindGameObjectWithTag("SellButton");
        upgradeButton = GameObject.FindGameObjectWithTag("UpgradeButton");
        towerDescription = GameObject.FindGameObjectWithTag("TowerDescription");
    }

    // Update is called once per frame
    void Update()
    {
        if (slide == 1)
        {
            if (hidden)
            {
                SetInfo();
                hidden = false;
            }
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.position;
            temp.y = temp.y + (60 * Time.deltaTime);
            if (temp.y >= 20)
            {
                temp.y = 20;
                slide = 0;
            }
            gameObject.GetComponent<RectTransform>().transform.position = temp;
        }
        else if (slide == 2 || slide == 3)
        {
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.position;
            temp.y = temp.y - (60 * Time.deltaTime);
            if (temp.y <= -20)
            {
                temp.y = -20;
                if (slide == 2) slide = 0;
                else slide = 1;
                hidden = true;
            }
            gameObject.GetComponent<RectTransform>().transform.position = temp;
        }
    }

    public void HideInfo()
    {
        if (slide == 0) slide = 2;
    }

    public void ShowInfo()
    {
        if (hidden) slide = 1;
        else slide = 3;
    }
    public void SellTower()
    {
        if (selectedTower.GetComponent<MapLocation>() != null)
        {
            selectedTower.GetComponent<MapLocation>().SellTower();
            HideInfo();
        }
    }
    public void UpgradeTower()
    {
        if (selectedTower.GetComponent<MapLocation>() != null)
        {
            if (GetUpgradeCost() <= Assets.CoinCounter.GetCoinCount())
            {
                Assets.CoinCounter.ChangeCoinCounter(-GetUpgradeCost());
                selectedTower.GetComponent<MapLocation>().towerLevel += 1;
                HideInfo();
            }
        }
    }
    int GetUpgradeCost()
    {
        switch (selectedTower.GetComponent<MapLocation>().towerType)
        {
            case "GoombaTower":
                return Assets.GoomaTower.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "KoopaTower":
                return Assets.KoopaTower.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "FreezieTower":
                return Assets.FreezieTower.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "BulletBlaster":
                return Assets.BulletBlaster.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Bowser":
                return Assets.Bowser.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
        }
        return 0;
    }
    void SetInfo()
    {
        if (selectedTower.GetComponent<TowerOption>() != null)
        {
            //Tower Menu
            towerDescription.GetComponent<Text>().text = "Buy cost: " + Convert.ToString(selectedTower.GetComponent<TowerOption>().towerCost) + ". Sell return: " + Convert.ToString(selectedTower.GetComponent<TowerOption>().towerSellCost) + "." + selectedTower.GetComponent<TowerOption>().description;
            sellButton.SetActive(false);
            upgradeButton.SetActive(false);
            Vector3 temp = towerDescription.GetComponent<RectTransform>().transform.position;
            temp.x = 87.45f;
            towerDescription.GetComponent<RectTransform>().transform.position = temp;
        }
        else if (selectedTower.GetComponent<MapLocation>() != null)
        {
            //Placed tower
            sellButton.SetActive(true);
            int UpgradeCost = GetUpgradeCost();
            Vector3 temp = towerDescription.GetComponent<RectTransform>().transform.position;
            if (UpgradeCost != 0)
            {
                upgradeButton.SetActive(true);
                temp.x = 262.36f;
                towerDescription.GetComponent<Text>().text = "Sell return: " + Convert.ToString(selectedTower.GetComponent<MapLocation>().towerSellCost) + ". Upgrade cost: " + Convert.ToString(GetUpgradeCost()) + "." + selectedTower.GetComponent<MapLocation>().description;
            }
            else
            {
                upgradeButton.SetActive(false);
                temp.x = 174.9f;
                towerDescription.GetComponent<Text>().text = "Sell return: " + Convert.ToString(selectedTower.GetComponent<MapLocation>().towerSellCost) + "." + selectedTower.GetComponent<MapLocation>().description;
            }
            towerDescription.GetComponent<RectTransform>().transform.position = temp;
        }
    }
}
