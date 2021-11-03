using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectionOption : MonoBehaviour, IPointerEnterHandler
{
    public string Description;
    public GameObject TextField;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextField.GetComponent<Text>().text = Description;
    }
}
