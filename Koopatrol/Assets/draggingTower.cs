using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class draggingTower : MonoBehaviour
{
    public int towerCost = 0;
    public int towerSellCost = 0;
    public string towerType = "none";
    public string description;
    public Sprite yTowerImage;
    public Assets.ValidPosition validPosition;
}
