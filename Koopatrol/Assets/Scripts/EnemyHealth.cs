using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyHealth : MonoBehaviour, IPointerClickHandler
{
    public int enemyCoin = 1;
    public int Health;
    public GameObject towerInfo;
    public int MaxHealth;
    bool dying = false;
    float deathTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        if (dying)
        {
            deathTime += Time.deltaTime;
            if (deathTime >= 1f && deathTime <= 1.5f)
            {
                gameObject.transform.Rotate(0, 0, Time.deltaTime * -200, Space.Self);
                gameObject.transform.localScale = new Vector3(Convert.ToSingle(gameObject.transform.localScale.x + (1f * Time.deltaTime)), Convert.ToSingle(gameObject.transform.localScale.x + (1f * Time.deltaTime)), Convert.ToSingle(gameObject.transform.localScale.x + (1f * Time.deltaTime)));
            }
            else if (deathTime > 1.5f)
            {
                gameObject.transform.Rotate(0, 0, Time.deltaTime * -200, Space.Self);
                gameObject.transform.localScale = new Vector3(Convert.ToSingle(gameObject.transform.localScale.x - (1f * Time.deltaTime)), Convert.ToSingle(gameObject.transform.localScale.x - (1f * Time.deltaTime)), Convert.ToSingle(gameObject.transform.localScale.x - (1f * Time.deltaTime)));
            }
            if (deathTime >= 3f) Destroy(gameObject);
        }
    }

    public void Hurt(int damage)
    {
        Health -= damage;
        gameObject.GetComponent<EnemyBehaviour>().Stagger(0.1f);
        if (Health <= 0)
        {
            death();
        }
        else
        {
            Color newcolor = new Color(1f, Convert.ToSingle(Health-1) / Convert.ToSingle(MaxHealth), Convert.ToSingle(Health-1) / Convert.ToSingle(MaxHealth), 1f);
            gameObject.GetComponent<Image>().color = newcolor;
            if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
            {
                towerInfo.GetComponent<TowerInfo>().SetInfo();
            }
        }
    }

    void death()
    {
        Assets.CoinCounter.ChangeCoinCounter(enemyCoin, false);
        Map.Enemies.Remove(gameObject);
        if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
        {
            towerInfo.GetComponent<TowerInfo>().HideInfo();
        }
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (GetComponent<EnemyBehaviour>().enemyType == "Mario")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayNew("Victory");
            GetComponent<EnemyBehaviour>().moveSpeed = 0f;
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets) { bullet.GetComponent<Assets.Bullet>().timeFlying = 60; }
            dying = true;
            if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
            {
                towerInfo.GetComponent<TowerInfo>().HideInfo();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            towerInfo.GetComponent<TowerInfo>().ShowInfo();
            towerInfo.GetComponent<TowerInfo>().selectedTower = gameObject;
        }
    }

}
