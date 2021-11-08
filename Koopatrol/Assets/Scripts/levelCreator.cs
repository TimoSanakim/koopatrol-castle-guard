using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class levelCreator : MonoBehaviour, IPointerClickHandler
{
    public GameObject SelectedOption;
    public GameObject SelectedOptionHighlight;
    public GameObject BackgroundTile;
    public GameObject Tile;
    public Sprite[] backgroundTiles;
    public Sprite[] blockedTiles;
    public Color[] PathColors;
    public Sprite[] backgrounds;
    public Sprite[] foregrounds;
    public GameObject Background;
    public GameObject Foreground;
    public GameObject GroundOption;
    public GameObject PathOption;
    public GameObject BlockedOption;
    int style = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeSelectedOption(GameObject option)
    {
        SelectedOption = option;
        SelectedOptionHighlight.transform.position = option.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Assets.CoinCounter.CoinCount = 300;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject background = Instantiate(BackgroundTile);
            background.transform.SetParent(gameObject.transform.GetChild(0), true);
            background.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            float x = background.transform.localPosition.x;
            float y = background.transform.localPosition.y;
            background.transform.localPosition = new Vector2(Convert.ToInt32(x / 50) * 50, Convert.ToInt32(y / 50) * 50);
            background.transform.localScale = new Vector3(1, 1, 1);
            if (SelectedOption.name == "PlaceObstruction") background.GetComponent<Image>().sprite = SelectedOption.GetComponent<Image>().sprite;

            GameObject tile = Instantiate(Tile);
            tile.transform.SetParent(gameObject.transform.GetChild(1), true);
            tile.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            tile.transform.localPosition = new Vector2(Convert.ToInt32(x / 50) * 50, Convert.ToInt32(y / 50) * 50);
            tile.transform.localScale = new Vector3(1, 1, 1);
            if (SelectedOption.name != "PlaceObstruction" && SelectedOption.name != "PlaceGround")
            {
                tile.GetComponent<Image>().sprite = SelectedOption.GetComponent<Image>().sprite;
                tile.GetComponent<Image>().color = SelectedOption.GetComponent<Image>().color;
                if (SelectedOption.name == "PlacePath") tile.tag = "Path";
                else tile.tag = "Castle";
            }
            else if (SelectedOption.name == "PlaceObstruction") tile.tag = "BlockedGround";
            else tile.tag = "Ground";
            tile.GetComponent<MapLocation>().RecalculateNeighbors();
            tile.GetComponent<MapLocation>().Recalculate();
        }
    }
    public void ChangeStyle()
    {
        style += 1;
        if (style == 3) style = 0;
        GroundOption.GetComponent<Image>().sprite = backgroundTiles[style];
        PathOption.GetComponent<Image>().color = PathColors[style];
        BlockedOption.GetComponent<Image>().sprite = blockedTiles[style];
        foreach (GameObject tile in Map.Tiles)
        {
            if (tile.tag == "Path") tile.GetComponent<Image>().color = PathColors[style];
            tile.transform.parent.transform.parent.GetChild(0).GetChild(tile.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = backgroundTiles[style];
            if (tile.tag == "BlockedGround") tile.transform.parent.transform.parent.GetChild(0).GetChild(tile.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = blockedTiles[style];
        }
        if (style == 0)
        {
            Background.GetComponent<Image>().sprite = backgrounds[0];
            Background.GetComponent<MovingBackOrForeground>().moveRight = 0.1f;
            Background.GetComponent<MovingBackOrForeground>().moveUp = 0f;
            Foreground.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            Foreground.GetComponent<MovingBackOrForeground>().moveRight = 0f;
            Foreground.GetComponent<MovingBackOrForeground>().moveUp = 0f;
        }
        if (style == 1)
        {
            Background.GetComponent<Image>().sprite = backgrounds[1];
            Background.GetComponent<MovingBackOrForeground>().moveRight = 0f;
            Background.GetComponent<MovingBackOrForeground>().moveUp = 0f;
            Foreground.GetComponent<Image>().sprite = foregrounds[0];
            Foreground.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            Foreground.GetComponent<MovingBackOrForeground>().moveRight = 0.5f;
            Foreground.GetComponent<MovingBackOrForeground>().moveUp = -1f;
        }
        if (style == 2)
        {
            Background.GetComponent<Image>().sprite = backgrounds[2];
            Background.GetComponent<MovingBackOrForeground>().moveRight = 0f;
            Background.GetComponent<MovingBackOrForeground>().moveUp = 0f;
            Foreground.GetComponent<Image>().sprite = foregrounds[1];
            Foreground.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            Foreground.GetComponent<MovingBackOrForeground>().moveRight = 2f;
            Foreground.GetComponent<MovingBackOrForeground>().moveUp = -0.5f;
        }
        Background.transform.localPosition = new Vector3(0, 0, 0);
        Foreground.transform.localPosition = new Vector3(0, 0, 0);
    }
}
