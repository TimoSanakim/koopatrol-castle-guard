using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossBar : MonoBehaviour
{
    [SerializeField] private Image HealthbarFill;
    bool hidden = true;
    byte slide = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slide == 1)
        {
            hidden = false;
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.localPosition;
            temp.y = temp.y - 15;
            if (temp.y <= -150)
            {
                temp.y = -150;
                slide = 0;
            }
            gameObject.GetComponent<RectTransform>().transform.localPosition = temp;
        }
        else if (slide == 2)
        {
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.localPosition;
            temp.y = temp.y + 15;
            if (temp.y >= 0)
            {
                temp.y = 0;
                slide = 0;
                hidden = true;
            }
            gameObject.GetComponent<RectTransform>().transform.localPosition = temp;
        }
    }
    void ShowBar()
    {
        if (slide == 0 || slide == 1) slide = 1;
    }
    void HideBar()
    {
        if (slide == 0) slide = 2;
    }
    public void UpdateValue(int Health, int MaxHealth)
    {
        if (Health >= 0 && hidden)
        {
            ShowBar();
        }
        else if (Health <= 0 && !hidden)
        {
            HideBar();
        }
        float pct = (float)Health / (float)MaxHealth;
        HealthbarFill.fillAmount = pct;
    }
}
