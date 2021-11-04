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
    public bool wasDragging;

    // Start is called before the first frame update
    void Start()
    {
        notvalidpath = GameObject.FindGameObjectWithTag("Path").GetComponent<Image>().color;
        notvalid = GameObject.FindGameObjectWithTag("Ground").GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragging && wasDragging)
        {
            foreach (GameObject spot in Map.Tiles)
            {
                if (spot.tag == "Ground") spot.GetComponent<Image>().color = notvalid;
                else if (spot.tag == "Path") spot.GetComponent<Image>().color = notvalidpath;
            }
            wasDragging = false;
        }
        else if (dragging)
        {
            if (validPosition == Assets.ValidPosition.AnyGround)
            {
                foreach (GameObject spot in Map.Tiles)
                {
                    if (spot.tag == "Ground") spot.GetComponent<Image>().color = valid;
                }
                transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
                transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
            }
            else if (validPosition == Assets.ValidPosition.Path)
            {
                foreach (GameObject spot in Map.Tiles)
                {
                    if (spot.tag == "Path") spot.GetComponent<Image>().color = valid;
                }
                transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
                transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
            }
            else if (validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis)
            {
                foreach (GameObject spot in Map.Tiles)
                {
                    if (spot.tag == "Ground" && spot.GetComponent<MapLocation>().isNextToPath != 0) spot.GetComponent<Image>().color = valid;
                }
                transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
                transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
            }
            else if (validPosition == Assets.ValidPosition.TutorialPosition)
            {
                foreach (GameObject spot in Map.Tiles)
                {
                    if (spot == Tutorial.TutorialPosition) spot.GetComponent<Image>().color = valid;
                }
            }
            wasDragging = true;
        }
    }
}
