using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerOption : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    GameObject draggingTower;
    public int towerCost;
    public int towerSellCost;
    public string description;
    GameObject coinCounter;
    GameObject towerInfo;
    GameObject map;
    public string towerType;
    public Sprite yTowerImage;
    public Assets.ValidPosition validPosition;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            towerInfo.GetComponent<TowerInfo>().slide = 1;
            towerInfo.GetComponent<TowerInfo>().selectedTower = gameObject;
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

    // Start is called before the first frame update
    void Start()
    {
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        coinCounter = GameObject.FindGameObjectWithTag("CoinCounter");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        map = GameObject.FindGameObjectWithTag("Map");
    }

    // Update is called once per frame
    void Update()
    {
        if (Convert.ToInt32(coinCounter.GetComponent<Text>().text) >= towerCost && !(towerType == "Bowser" && map.GetComponent<Map>().bowserPlaced))
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            gameObject.GetComponent<CanvasGroup>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            gameObject.GetComponent<CanvasGroup>().interactable = false;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
        draggingTower.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        draggingTower.GetComponent<Image>().color = gameObject.GetComponent<Image>().color;
        draggingTower.GetComponent<draggingTower>().towerCost = towerCost;
        draggingTower.GetComponent<draggingTower>().towerSellCost = towerSellCost;
        draggingTower.GetComponent<draggingTower>().towerType = towerType;
        draggingTower.GetComponent<draggingTower>().description = description;
        draggingTower.GetComponent<draggingTower>().yTowerImage = yTowerImage;
        draggingTower.GetComponent<draggingTower>().validPosition = validPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingTower.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggingTower.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}
