using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour
{
    bool hidden = true;
    public byte slide;
    public GameObject selectedTower;
    GameObject sellButton;
    GameObject towerDescription;
    // Start is called before the first frame update
    void Start()
    {
        sellButton = GameObject.FindGameObjectWithTag("SellButton");
        towerDescription = GameObject.FindGameObjectWithTag("TowerDescription");
    }

    // Update is called once per frame
    void Update()
    {
        if (slide == 1)
        {
            if (gameObject.GetComponent<RectTransform>().transform.position.y == 20)
            {
                slide = 3;
            }
            else
            {
                if (hidden)
                {
                    SetInfo();
                    hidden = false;
                }
                Vector3 temp = gameObject.GetComponent<RectTransform>().transform.position;
                temp.y += 2;
                gameObject.GetComponent<RectTransform>().transform.position = temp;
                if (gameObject.GetComponent<RectTransform>().transform.position.y == 20) slide = 0;
            }
        }
        else if (slide == 2 || slide == 3)
        {
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.position;
            temp.y -= 2;
            gameObject.GetComponent<RectTransform>().transform.position = temp;
            if (temp.y == -20)
            {
                if (slide == 2) slide = 0;
                else slide = 1;
                hidden = true;
            }
        }
    }

    public void HideInfo()
    {
        if (slide == 0) slide = 2;
    }
    public void SellTower()
    {
        if (selectedTower.GetComponent<MapLocation>() != null)
        {
            selectedTower.GetComponent<MapLocation>().SellTower();
            HideInfo();
        }
    }
    void SetInfo()
    {
        if (selectedTower.GetComponent<TowerOption>() != null)
        {
            //Tower Menu
            towerDescription.GetComponent<Text>().text = "Buy cost: " + Convert.ToString(selectedTower.GetComponent<TowerOption>().towerCost) + ". Sell return: " + Convert.ToString(selectedTower.GetComponent<TowerOption>().towerSellCost) + "." + selectedTower.GetComponent<TowerOption>().description;
            sellButton.SetActive(false);
            Vector3 temp = towerDescription.GetComponent<RectTransform>().transform.position;
            temp.x = 131.18f;
            towerDescription.GetComponent<RectTransform>().transform.position = temp;
        }
        else if (selectedTower.GetComponent<MapLocation>() != null)
        {
            //Placed tower
            towerDescription.GetComponent<Text>().text = "Sell return: " + Convert.ToString(selectedTower.GetComponent<MapLocation>().towerSellCost) + "." + selectedTower.GetComponent<MapLocation>().description;
            sellButton.SetActive(true);
            Vector3 temp = towerDescription.GetComponent<RectTransform>().transform.position;
            temp.x = 221.84f;
            towerDescription.GetComponent<RectTransform>().transform.position = temp;
        }
    }
}
