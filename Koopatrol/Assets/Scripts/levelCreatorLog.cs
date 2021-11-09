using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class levelCreatorLog : MonoBehaviour
{
    public void Log(string message)
    {
        GetComponent<TextMeshProUGUI>().text = "[" + DateTime.Now + "] " + message;
    }
}
