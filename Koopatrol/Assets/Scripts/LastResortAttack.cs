using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LastResortAttack : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    GameObject towerInfo;
    GameObject Attack;
    public bool used = false;
    public int costs;
    public int damage;
    public int damageperc;
    public float freezetime;
    public string description;
    public bool disabled = false;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = Convert.ToSingle(Map.SoundVolume) / 100;
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        Attack = GameObject.FindGameObjectWithTag("LastResortAttack");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Assets.CoinCounter.GetCoinCount() >= costs && !used && !disabled)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!used && Assets.CoinCounter.GetCoinCount() >= costs)
            {
                used = true;
                Assets.CoinCounter.ChangeCoinCounter(-costs, false);
                if (Attack.GetComponent<Tornado>() != null)
                {
                    for (int i = 0; i < Map.PossiblePaths.Count; i++) {
                        Tornado.CreateTornado(i, Attack, damage, damageperc, freezetime);
                    }
                }
                else if (Attack.GetComponent<LavaAttack>() != null)
                {
                    foreach (GameObject castle in GameObject.FindGameObjectsWithTag("Castle"))
                    {
                        LavaAttack.CreateLavaAttack(castle, Attack, damage, damageperc, freezetime);
                    }
                }
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (towerInfo.GetComponent<TowerInfo>().selectedTower != gameObject || towerInfo.GetComponent<TowerInfo>().hidden) towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
    }
}
