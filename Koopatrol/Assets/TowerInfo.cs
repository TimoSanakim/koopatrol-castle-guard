using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo : MonoBehaviour
{
    bool hidden = true;
    public byte slide;
    public GameObject selectedTower;
    GameObject sellButton;
    // Start is called before the first frame update
    void Start()
    {
        sellButton = GameObject.FindGameObjectWithTag("SellTowerButton");
    }

    // Update is called once per frame
    void Update()
    {
        if (slide == 1)
        {
            if (gameObject.GetComponent<RectTransform>().transform.position.y == 10)
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
                temp.y += 1;
                gameObject.GetComponent<RectTransform>().transform.position = temp;
                if (gameObject.GetComponent<RectTransform>().transform.position.y == 10) slide = 0;
            }
        }
        else if (slide == 2 || slide == 3)
        {
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.position;
            temp.y -= 1;
            gameObject.GetComponent<RectTransform>().transform.position = temp;
            if (temp.y == -10)
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
            sellButton.SetActive(false);
        }
        else if (selectedTower.GetComponent<MapLocation>() != null)
        {
            //Placed tower
            sellButton.SetActive(true);
        }
    }
}
