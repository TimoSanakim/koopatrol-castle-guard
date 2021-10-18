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
    GameObject towerInfo;
    public string towerType;
    public List<Sprite> towerSprites;
    bool canPlace = true;
    public Assets.ValidPosition validPosition;
    public AudioClip towerSound;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
    }

    // Update is called once per frame
    void Update()
    {
        if (Assets.CoinCounter.GetCoinCount() >= towerCost && !(towerType == "Bowser" && Map.bowserPlaced))
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            canPlace = true;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
            canPlace = false;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canPlace)
        {
            draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
            draggingTower.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
            draggingTower.GetComponent<Image>().color = gameObject.GetComponent<Image>().color;
            draggingTower.GetComponent<draggingTower>().towerCost = towerCost;
            draggingTower.GetComponent<draggingTower>().towerType = towerType;
            draggingTower.GetComponent<draggingTower>().towerSprites = towerSprites;
            draggingTower.GetComponent<draggingTower>().towerSound = towerSound;
            draggingTower.GetComponent<draggingTower>().validPosition = validPosition;
            draggingTower.GetComponent<draggingTower>().dragging = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingTower.GetComponent<CanvasGroup>().alpha = 0f;
        draggingTower.GetComponent<draggingTower>().dragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canPlace)
        {
            draggingTower.GetComponent<RectTransform>().position = Input.mousePosition;
            
        }
    }
}
