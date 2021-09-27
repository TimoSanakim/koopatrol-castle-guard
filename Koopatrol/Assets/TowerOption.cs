using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerOption : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    GameObject draggingTower;
    public int towerCost;
    public int towerSellCost;
    GameObject coinCounter;
    public string towerType;
    public Assets.ValidPosition validPosition;
    // Start is called before the first frame update
    void Start()
    {
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        coinCounter = GameObject.FindGameObjectWithTag("CoinCounter");
    }

    // Update is called once per frame
    void Update()
    {
        if (Convert.ToInt32(coinCounter.GetComponent<Text>().text) >= towerCost)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingTower.SetActive(true);
        draggingTower.GetComponent<Image>().sprite = this.gameObject.GetComponent<Image>().sprite;
        draggingTower.GetComponent<Image>().color = this.gameObject.GetComponent<Image>().color;
        draggingTower.GetComponent<draggingTower>().towerCost = this.towerCost;
        draggingTower.GetComponent<draggingTower>().towerSellCost = this.towerSellCost;
        draggingTower.GetComponent<draggingTower>().towerType = this.towerType;
        draggingTower.GetComponent<draggingTower>().validPosition = this.validPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingTower.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggingTower.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}
