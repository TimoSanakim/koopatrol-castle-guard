using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerInfo : MonoBehaviour
{
    public bool hidden = true;
    public byte slide = 0;
    public GameObject selectedTower;
    GameObject sellButton;
    GameObject targetButton;
    GameObject upgradeButton;
    GameObject NewtowerDescription;
    GameObject rangeCircle;
    GameObject mapRange;
    GameObject fallbackTower;
    Color replacedColor = new Color(1f, 1f, 1f, 1f);

    public AudioClip upgradeSound;
    public AudioClip errorSound;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = Convert.ToSingle(Map.SoundVolume) / 100;
        sellButton = GameObject.FindGameObjectWithTag("SellButton");
        targetButton = GameObject.FindGameObjectWithTag("TargetButton");
        upgradeButton = GameObject.FindGameObjectWithTag("UpgradeButton");
        NewtowerDescription = GameObject.FindGameObjectWithTag("NewTowerDescription");
        rangeCircle = GameObject.FindGameObjectWithTag("RangeCircle");
        selectedTower = GameObject.FindGameObjectWithTag("TowerOption");
        fallbackTower = selectedTower;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedTower == null)
        {
            selectedTower = fallbackTower;
            replacedColor = new Color(1f, 1f, 1f, 1f);
        }
        if ((slide == 0 || slide == 1) && !hidden && selectedTower.GetComponent<MapLocation>() != null)
        {
            if (mapRange != null && selectedTower.GetComponent<MapLocation>().towerType != "BulletBlaster") mapRange.GetComponent<RangeCircle>().killNextTime = false;
            else if (selectedTower.GetComponent<MapLocation>().towerType != "BulletBlaster") mapRange = rangeCircle.GetComponent<RangeCircle>().CreateRangeCircle(selectedTower);
            else if (selectedTower.GetComponent<MapLocation>().towerType == "BulletBlaster") selectedTower.GetComponent<MapLocation>().bulletBlasterRangeVisualization();
        }
        if (slide == 1)
        {
            if (hidden)
            {
                SetInfo();
                hidden = false;
            }
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.localPosition;
            temp.y = temp.y + 30;
            if (temp.y >= 350)
            {
                temp.y = 350;
                slide = 0;
            }
            gameObject.GetComponent<RectTransform>().transform.localPosition = temp;
        }
        else if (slide == 2 || slide == 3)
        {
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.localPosition;
            temp.y = temp.y - 30;
            if (temp.y <= 0)
            {
                temp.y = 0;
                if (slide == 2) slide = 0;
                else slide = 1;
                hidden = true;
            }
            gameObject.GetComponent<RectTransform>().transform.localPosition = temp;
        }
    }

    public void HideInfo()
    {
        if (slide == 0 || slide == 1) slide = 2;
        if (selectedTower != null && selectedTower.GetComponent<Image>() != null) selectedTower.GetComponent<Image>().color = replacedColor;
        foreach (GameObject spot in Map.Tiles)
        {
            if (spot.GetComponent<MapLocation>().rangeIndicating) spot.GetComponent<MapLocation>().RemoveRangeIndication();
        }
    }

    public void ShowInfo(GameObject selectedObject)
    {
        foreach (GameObject spot in Map.Tiles)
        {
            if (spot.GetComponent<MapLocation>().rangeIndicating) spot.GetComponent<MapLocation>().RemoveRangeIndication();
        }
        mapRange = null;
        if (selectedTower.GetComponent<Image>() != null) selectedTower.GetComponent<Image>().color = replacedColor;
        selectedTower = selectedObject;
        if (selectedTower.GetComponent<Image>() != null) replacedColor = selectedTower.GetComponent<Image>().color;
        if (selectedTower.GetComponent<MapLocation>() != null) selectedTower.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
        if (hidden) slide = 1;
        else SetInfo();

    }
    public void SellTower()
    {
        if (selectedTower.GetComponent<MapLocation>() != null)
        {
            Assets.CoinCounter.ChangeCoinCounter(GetSellCost(), true);
            selectedTower.GetComponent<MapLocation>().DestroyTower();
            selectedTower = null;
            HideInfo();
        }
    }
    public void SwitchTarget()
    {
        if (selectedTower.GetComponent<MapLocation>() != null)
        {
            selectedTower.GetComponent<MapLocation>().TargetPriority += 1;
            if (selectedTower.GetComponent<MapLocation>().TargetPriority == 6) selectedTower.GetComponent<MapLocation>().TargetPriority = 0;
            SetInfo();
        }
    }
    public void UpgradeTower()
    {
        if (selectedTower.GetComponent<MapLocation>() != null)
        {
            if (GetUpgradeCost() <= Assets.CoinCounter.GetCoinCount())
            {
                Assets.CoinCounter.ChangeCoinCounter(-GetUpgradeCost(), false);
                selectedTower.GetComponent<Image>().sprite = selectedTower.GetComponent<MapLocation>().towerSprites[selectedTower.GetComponent<MapLocation>().towerLevel];
                selectedTower.GetComponent<MapLocation>().towerLevel += 1;
                SetInfo();
                GetComponent<AudioSource>().clip = upgradeSound;
                GetComponent<AudioSource>().Play();
            }
            else
            {
                GetComponent<AudioSource>().clip = errorSound;
                GetComponent<AudioSource>().Play();
            }
        }
    }
    int GetSellCost()
    {
        switch (selectedTower.GetComponent<MapLocation>().towerType)
        {
            case "GoombaTower":
                return Assets.GoombaTower.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "KoopaTower":
                return Assets.KoopaTower.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "FreezieTower":
                return Assets.FreezieTower.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Thwomp":
                return Assets.Thwomp.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "BulletBlaster":
                return Assets.BulletBlaster.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "PiranhaPlant":
                return Assets.PiranhaPlant.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "MagikoopaTower":
                return Assets.MagikoopaTower.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Bowser":
                return Assets.Bowser.GetSellCost(selectedTower.GetComponent<MapLocation>().towerLevel);
        }
        return 0;
    }
    int GetUpgradeCost()
    {
        switch (selectedTower.GetComponent<MapLocation>().towerType)
        {
            case "GoombaTower":
                return Assets.GoombaTower.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "KoopaTower":
                return Assets.KoopaTower.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "FreezieTower":
                return Assets.FreezieTower.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Thwomp":
                return Assets.Thwomp.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "BulletBlaster":
                return Assets.BulletBlaster.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "PiranhaPlant":
                return Assets.PiranhaPlant.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "MagikoopaTower":
                return Assets.MagikoopaTower.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Bowser":
                return Assets.Bowser.GetUpgradeCost(selectedTower.GetComponent<MapLocation>().towerLevel);
        }
        return 0;
    }
    string GetDesciptionMap()
    {
        switch (selectedTower.GetComponent<MapLocation>().towerType)
        {
            case "GoombaTower":
                return Assets.GoombaTower.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "KoopaTower":
                return Assets.KoopaTower.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "FreezieTower":
                return Assets.FreezieTower.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Thwomp":
                return Assets.Thwomp.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "BulletBlaster":
                return Assets.BulletBlaster.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "PiranhaPlant":
                return Assets.PiranhaPlant.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "MagikoopaTower":
                return Assets.MagikoopaTower.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Bowser":
                return Assets.Bowser.GetDescription(selectedTower.GetComponent<MapLocation>().towerLevel);
        }
        return "";
    }
    bool CanTarget()
    {
        switch (selectedTower.GetComponent<MapLocation>().towerType)
        {
            case "Thwomp":
                return false;
            case "PiranhaPlant":
                return false;
            case "MagikoopaTower":
                return false;
        }
        return true;
    }
    string GetDesciptionOption()
    {
        switch (selectedTower.GetComponent<TowerOption>().towerType)
        {
            case "GoombaTower":
                return Assets.GoombaTower.GetDescription(1);
            case "KoopaTower":
                return Assets.KoopaTower.GetDescription(1);
            case "FreezieTower":
                return Assets.FreezieTower.GetDescription(1);
            case "Thwomp":
                return Assets.Thwomp.GetDescription(1);
            case "BulletBlaster":
                return Assets.BulletBlaster.GetDescription(1);
            case "PiranhaPlant":
                return Assets.PiranhaPlant.GetDescription(1);
            case "MagikoopaTower":
                return Assets.MagikoopaTower.GetDescription(1);
            case "Bowser":
                return Assets.Bowser.GetDescription(1);
        }
        return "";
    }
    public void SetInfo()
    {
        if (selectedTower.GetComponent<TowerOption>() != null)
        {
            //Tower Menu
            NewtowerDescription.GetComponent<TextMeshProUGUI>().text = GetDesciptionOption();
            sellButton.SetActive(false);
            targetButton.SetActive(false);
            upgradeButton.SetActive(false);
        }
        else if (selectedTower.GetComponent<MapLocation>() != null)
        {
            //Placed tower
            sellButton.SetActive(true);
            if (CanTarget()) targetButton.SetActive(true);
            if (selectedTower.GetComponent<MapLocation>().TargetPriority == 0)
            {
                targetButton.GetComponentInChildren<Text>().text = "Focus:\nNearest Enemy";
            }
            else if (selectedTower.GetComponent<MapLocation>().TargetPriority == 1)
            {
                targetButton.GetComponentInChildren<Text>().text = "Focus:\nShortest Path";
            }
            else if (selectedTower.GetComponent<MapLocation>().TargetPriority == 2)
            {
                targetButton.GetComponentInChildren<Text>().text = "Focus:\nLongest Path";
            }
            else if (selectedTower.GetComponent<MapLocation>().TargetPriority == 3)
            {
                targetButton.GetComponentInChildren<Text>().text = "Focus:\nLeast Health";
            }
            else if (selectedTower.GetComponent<MapLocation>().TargetPriority == 4)
            {
                targetButton.GetComponentInChildren<Text>().text = "Focus:\nMost Health";
            }
            else if (selectedTower.GetComponent<MapLocation>().TargetPriority == 5)
            {
                targetButton.GetComponentInChildren<Text>().text = "Focus:\nMost Damage";
            }
            sellButton.GetComponentInChildren<Text>().text = "Sell\n" + Convert.ToString(GetSellCost()) + " coins";
            NewtowerDescription.GetComponent<TextMeshProUGUI>().text = GetDesciptionMap();
            int UpgradeCost = GetUpgradeCost();
            if (UpgradeCost != 0)
            {
                upgradeButton.SetActive(true);
                upgradeButton.GetComponentInChildren<Text>().text = "Upgrade\n" + Convert.ToString(GetUpgradeCost() + " coins");
            }
            else
            {
                upgradeButton.SetActive(false);
            }
        }
        else if (selectedTower.GetComponent<LastResortAttack>() != null)
        {
            //Lava attack
            NewtowerDescription.GetComponent<TextMeshProUGUI>().text = selectedTower.GetComponent<LastResortAttack>().description;
            sellButton.SetActive(false);
            targetButton.SetActive(false);
            upgradeButton.SetActive(false);
        }
        else if (selectedTower.GetComponent<EnemyHealth>() != null)
        {
            //Enemy
            NewtowerDescription.GetComponent<TextMeshProUGUI>().text = selectedTower.GetComponent<EnemyBehaviour>().GetDescription();
            sellButton.SetActive(false);
            targetButton.SetActive(false);
            upgradeButton.SetActive(false);
        }
    }
}
