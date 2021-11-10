using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class levelCreatorEntry : MonoBehaviour
{
    public GameObject map;
    public void Remove()
    {
        map.GetComponent<levelCreator>().RemoveEntry(gameObject);
    }
}
