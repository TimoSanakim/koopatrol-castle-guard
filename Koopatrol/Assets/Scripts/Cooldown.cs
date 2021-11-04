using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    public float maxcooldown;
    public float cooldowntimer;
    public float cooldowncent;
    public bool isClone;
    public string objectname;

    // Start is called before the first frame update
    void Start()
    {
        if (isClone)
        {
            maxcooldown = GetMaxCooldown();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClone)
        {
            gameObject.GetComponent<Image>().fillAmount = cooldowncent;
            cooldowntimer = gameObject.GetComponentInParent<MapLocation>().cooldown;
            if (cooldowntimer == 0)
            {
                maxcooldown = GetMaxCooldown();
                cooldowntimer = maxcooldown;
            }
            cooldowncent = cooldowntimer / maxcooldown;
            if (cooldowntimer == maxcooldown) gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            else gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }
    int GetMaxCooldown()
    {
        int towerLevel = gameObject.GetComponentInParent<MapLocation>().towerLevel;
        if (gameObject.GetComponentInParent<MapLocation>().towerWasBuffed) towerLevel += 1;
        switch (gameObject.GetComponentInParent<MapLocation>().towerType)
        {
            case "GoombaTower":
                return Convert.ToInt32(Assets.GoombaTower.GetCooldown(towerLevel));
            case "KoopaTower":
                return Convert.ToInt32(Assets.KoopaTower.GetCooldown(towerLevel));
            case "FreezieTower":
                return Convert.ToInt32(Assets.FreezieTower.GetCooldown(towerLevel));
            case "Thwomp":
                return Convert.ToInt32(Assets.Thwomp.GetCooldown(towerLevel));
            case "BulletBlaster":
                return Convert.ToInt32(Assets.BulletBlaster.GetCooldown(towerLevel));
            case "PiranhaPlant":
                return Convert.ToInt32(Assets.PiranhaPlant.GetCooldown(towerLevel));
            case "Bowser":
                return Convert.ToInt32(Assets.Bowser.GetCooldown(towerLevel));
        }
        return -1;
    }

    public GameObject CreateCooldownCounter(GameObject parent)
    {
        GameObject cooldown = Instantiate(gameObject);
        cooldown.transform.position = parent.transform.position;
        cooldown.transform.SetParent(parent.transform, true);
        cooldown.GetComponent<CanvasGroup>().alpha = 1f;
        cooldown.GetComponent<Cooldown>().isClone = true;
        cooldown.tag = "CooldownCounter";
        return cooldown;   
    }
}
