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
            //Input.mousePosition
            Debug.Log((Input.mousePosition.x - (gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta.x * gameObject.transform.parent.localScale.x * 0.5) - gameObject.transform.localPosition.x) + " | " + (Input.mousePosition.y - (gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta.y * gameObject.transform.parent.localScale.y * 0.5 - gameObject.transform.localPosition.y)));


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
                tile.GetComponent<Image>().color = new Color(1, 1, 1);
            }
        }
    }
}
