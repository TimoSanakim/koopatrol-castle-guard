using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectionOption : MonoBehaviour, IPointerEnterHandler
{
    public string Description = "ERROR";
    public GameObject TextField;
    public GameObject Image;
    public Sprite Sprite = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextField.GetComponent<Text>().text = Description;
        Image.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        if (Sprite != null)
        {
            Image.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            Image.GetComponent<Image>().sprite = Sprite;
        }
    }
}
