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
    public bool dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color valid = new Color(0.3f, 1f, 0.3f, 0.5f);
        Color notvalid = new Color(0f, 0f, 0f, 0f);
        Color notvalidpath = new Color(1f, 1f, 1f, 1f);
        if (!dragging)
        {
            foreach (GameObject spot in Map.Tiles)
            {
                if (spot.tag == "Ground") spot.GetComponent<Image>().color = notvalid;
                else if (spot.tag == "Path") spot.GetComponent<Image>().color = notvalidpath;
            }
        }
        else
        {
            if (validPosition == Assets.ValidPosition.AnyGround)
            {
                foreach (GameObject spot in Map.Tiles)
                {
                    if (spot.tag == "Ground") spot.GetComponent<Image>().color = valid;
                }
            }
            else if (validPosition == Assets.ValidPosition.Path)
            {
                foreach (GameObject spot in Map.Tiles)
                {
                    if (spot.tag == "Path") spot.GetComponent<Image>().color = valid;
                }
            }
            else if (validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis)
            {
                foreach (GameObject spot in Map.Tiles)
                {
                    if (spot.tag == "Path" && spot.GetComponent<MapLocation>().isNextToPath != 0) spot.GetComponent<Image>().color = valid;
                }
            }
        }
    }
}
