using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapLocation : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int towerCost = 0;
    public int towerSellCost = 0;
    GameObject draggingTower;
    public string towerType = "none";
    //1 = path up or down, and not to sides
    //2 = path left or right, and not above or below
    //0 = any other situation
    public int isNextToPath = 0;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click");
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle click");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (this.gameObject.tag == "Tower" || this.gameObject.tag == "PathTower")
            {
                Assets.CoinCounter.ChangeCoinCounter(towerSellCost);
                this.gameObject.GetComponent<Image>().sprite = null;
                Color temp = Color.white;
                temp.a = 0f;
                this.gameObject.GetComponent<Image>().color = temp;
                towerCost = 0;
                towerType = "none";
                towerSellCost = 0;
                if (this.gameObject.tag == "Tower")
                {
                    this.gameObject.tag = "Ground";
                }
                else
                {
                    this.gameObject.tag = "Path";
                }
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if ((this.gameObject.tag == "Ground" || this.gameObject.tag == "Tower") && (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.AnyGround || (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath != 0)))
            {
                if (this.gameObject.tag == "Tower")
                {
                    Assets.CoinCounter.ChangeCoinCounter(towerSellCost);
                }
                this.gameObject.GetComponent<Image>().sprite = draggingTower.GetComponent<Image>().sprite;
                this.gameObject.GetComponent<Image>().color = draggingTower.GetComponent<Image>().color;
                towerCost = draggingTower.GetComponent<draggingTower>().towerCost;
                towerType = draggingTower.GetComponent<draggingTower>().towerType;
                towerSellCost = draggingTower.GetComponent<draggingTower>().towerSellCost;
                this.gameObject.tag = "Tower";
                Assets.CoinCounter.ChangeCoinCounter(-towerCost);
            }
            else if ((this.gameObject.tag == "Path" || this.gameObject.tag == "PathTower") && draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.Path)
            {
                if (this.gameObject.tag == "PathTower")
                {
                    Assets.CoinCounter.ChangeCoinCounter(towerSellCost);
                }
                this.gameObject.GetComponent<Image>().sprite = draggingTower.GetComponent<Image>().sprite;
                this.gameObject.GetComponent<Image>().color = draggingTower.GetComponent<Image>().color;
                towerCost = draggingTower.GetComponent<draggingTower>().towerCost;
                towerType = draggingTower.GetComponent<draggingTower>().towerType;
                towerSellCost = draggingTower.GetComponent<draggingTower>().towerSellCost;
                this.gameObject.tag = "PathTower";
                Assets.CoinCounter.ChangeCoinCounter(-towerCost);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        if (this.gameObject.tag == "Ground")
        {
            GameObject[] field = GameObject.FindGameObjectsWithTag("Path");
            bool xPath = false;
            bool yPath = false;
            foreach (GameObject path in field)
            {
                if (this.gameObject.transform.position.x - path.transform.position.x >= 30 && this.gameObject.transform.position.x - path.transform.position.x <= 70 && this.gameObject.transform.position.y == path.transform.position.y)
                {
                    xPath = true;
                }
                else if (this.gameObject.transform.position.x - path.transform.position.x >= -70 && this.gameObject.transform.position.x - path.transform.position.x <= -30 && this.gameObject.transform.position.y == path.transform.position.y)
                {
                    xPath = true;
                }
                else if (this.gameObject.transform.position.y - path.transform.position.y >= 30 && this.gameObject.transform.position.y - path.transform.position.y <= 70 && this.gameObject.transform.position.x == path.transform.position.x)
                {
                    yPath = true;
                }
                else if (this.gameObject.transform.position.y - path.transform.position.y >= -70 && this.gameObject.transform.position.y - path.transform.position.y <= -30 && this.gameObject.transform.position.x == path.transform.position.x)
                {
                    yPath = true;
                }
            }
            if (xPath || yPath)
            {
                if (xPath && !yPath)
                {
                    isNextToPath = 1;
                    this.gameObject.GetComponent<Image>().color = Color.black;
                }
                else if (yPath && !xPath)
                {
                    isNextToPath = 2;
                    this.gameObject.GetComponent<Image>().color = Color.black;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
