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
    GameObject Music;
    public int MaxHealth;
    bool dying = false;
    bool playedDeathSound = false;
    public bool HitByLava = false;
    int deathTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
        Music = GameObject.FindGameObjectWithTag("Music");
    }

    // Update is called once per frame
    void Update()
    {
        if (dying)
        {
            if (deathTime >= 60 && deathTime <= 100)
            {
                if (GetComponent<EnemyBehaviour>().deathSound != null && !playedDeathSound)
                {
                    GetComponent<AudioSource>().clip = GetComponent<EnemyBehaviour>().deathSound;
                    GetComponent<AudioSource>().Play();
                    playedDeathSound = true;
                }
                gameObject.transform.Rotate(0, 0, -3, Space.Self);
                gameObject.transform.localScale = new Vector3(Convert.ToSingle(gameObject.transform.localScale.x + 0.01f), Convert.ToSingle(gameObject.transform.localScale.x + 0.01f), Convert.ToSingle(gameObject.transform.localScale.x + 1f));
            }
            else if (deathTime >= 100 && deathTime <= 240)
            {
                gameObject.transform.Rotate(0, 0, -3, Space.Self);
                gameObject.transform.localScale = new Vector3(Convert.ToSingle(gameObject.transform.localScale.x - 0.01f), Convert.ToSingle(gameObject.transform.localScale.x - 0.01f), Convert.ToSingle(gameObject.transform.localScale.x - 1f));
            }
            if (!Music.GetComponent<AudioSource>().isPlaying)
            {
                GameSettings.restartGame();
            }
            deathTime += 1;
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
            if (GetComponent<EnemyBehaviour>().deathSound != null)
            {
                GetComponent<AudioSource>().clip = GetComponent<EnemyBehaviour>().deathSound;
                GetComponent<AudioSource>().Play();
            }
        }
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (GetComponent<EnemyBehaviour>().finalEnemy)
        {
            Music.GetComponent<Music>().PlayNew("Victory");
            GetComponent<EnemyBehaviour>().moveSpeed = 0f;
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets) Destroy(bullet);
            Map.Victory = true;
            Map.gameSpeed = 0;
            dying = true;
            Time.timeScale = Map.gameSpeed;
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
