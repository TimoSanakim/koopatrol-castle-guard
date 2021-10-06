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
    GameObject[] paths;
    GameObject[] ground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        paths = GameObject.FindGameObjectsWithTag("Path");
        ground = GameObject.FindGameObjectsWithTag("Ground");
        Color valid = new Color(0.3f, 1f, 0.3f, 0.5f);
        Color notvalid = new Color(0f, 0f, 0f, 0f);
        Color notvalidpath = new Color(1f, 1f, 1f, 1f);
        if (towerType == "none")
        {
            foreach (GameObject spot in ground)
            {
                spot.GetComponent<Image>().color = notvalid;
            }
            foreach (GameObject spot in paths)
            {
                spot.GetComponent<Image>().color = notvalidpath;
            }
        }
        else
        {
            if (validPosition == Assets.ValidPosition.AnyGround)
            {
                foreach (GameObject spot in ground)
                {
                    spot.GetComponent<Image>().color = valid;
                }
            }
            else if (validPosition == Assets.ValidPosition.Path)
            {
                foreach (GameObject spot in paths)
                {
                    spot.GetComponent<Image>().color = valid;
                }
            }
            else if (validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis)
            {
                foreach (GameObject spot in ground)
                {
                    if (spot.GetComponent<MapLocation>().isNextToPath != 0) spot.GetComponent<Image>().color = valid;
                    else spot.GetComponent<Image>().color = notvalid;
                }
            }
        }
    }
}
