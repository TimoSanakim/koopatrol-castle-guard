using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    int towerLevel = 0;
    public float maxcooldown;
    public float cooldowntimer;
    public float cooldowncent;
    public string towertype;
    public bool isClone;
    public string objectname;

    // Start is called before the first frame update
    void Start()
    {
        objectname = name;
        if (name != "CooldownCounter") { 
        towertype = gameObject.GetComponentInParent<MapLocation>().towerType;
        if (towertype == "Thwomp") maxcooldown = Assets.Thwomp.GetCooldown(towerLevel);
        if (towertype == "FreezieTower") maxcooldown = Assets.FreezieTower.GetCooldown(towerLevel);
        if (towertype == "PiranhaPlant") maxcooldown = Assets.PiranhaPlant.GetCooldown(towerLevel);
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (name != "CooldownCounter")
        {
            gameObject.GetComponent<Image>().fillAmount = cooldowncent;
            cooldowntimer = gameObject.GetComponentInParent<MapLocation>().cooldown;
            if (cooldowntimer == 0) cooldowntimer = maxcooldown;
            cooldowncent = cooldowntimer / maxcooldown;
        }
    }

    public GameObject CreateCooldownCounter(GameObject parent)
    {
        GameObject cooldown = Instantiate(gameObject);
        cooldown.transform.position = parent.transform.position;
        cooldown.transform.SetParent(parent.transform, true);
        cooldown.GetComponent<CanvasGroup>().alpha = 1f;
        cooldown.tag = "CooldownCounter";
        return cooldown;   
    }
}
