using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class draggingTower : MonoBehaviour
{
    public int towerCost = 0;
    public string towerType = "none";
    public AudioClip towerSound;
    public List<Sprite> towerSprites;
    public Assets.ValidPosition validPosition;
    public bool dragging = false;
    Color valid = new Color(0.3f, 1f, 0.3f, 0.5f);
    Color notvalid;
    Color notvalidpath;

    // Start is called before the first frame update
    void Start()
    {
        notvalidpath = GameObject.FindGameObjectWithTag("Path").GetComponent<Image>().color;
        notvalid = GameObject.FindGameObjectWithTag("Ground").GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
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
                    if (spot.tag == "Ground" && spot.GetComponent<MapLocation>().isNextToPath != 0) spot.GetComponent<Image>().color = valid;
                }
            }
        }
    }
}
