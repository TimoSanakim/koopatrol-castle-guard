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
    GameObject BossBar;
    GameObject Music;
    public int MaxHealth;
    public bool dying = false;
    bool playedDeathSound = false;
    public bool HitByLava = false;
    float deathTime = 0;
    float healthPercent;
    float blinktime = 0.2f;
    bool blink = false;
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
        Music = GameObject.FindGameObjectWithTag("Music");
        BossBar = GameObject.FindGameObjectWithTag("BossBar");
        if (BossBar != null && GetComponent<EnemyBehaviour>().finalEnemy && GetComponent<EnemyBehaviour>().isClone)
        {
            BossBar.GetComponent<BossBar>().UpdateValue(Health, MaxHealth);
            BossBar.GetComponentInChildren<Text>().text = GetComponent<EnemyBehaviour>().enemyType;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dying && GetComponent<EnemyBehaviour>().finalEnemy)
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
                gameObject.transform.localScale = new Vector3(Convert.ToSingle(gameObject.transform.localScale.x + 0.01f), Convert.ToSingle(gameObject.transform.localScale.x + 0.01f), Convert.ToSingle(gameObject.transform.localScale.x));
            }
            else if (deathTime >= 100 && deathTime <= 240)
            {
                gameObject.transform.Rotate(0, 0, -3, Space.Self);
                gameObject.transform.localScale = new Vector3(Convert.ToSingle(gameObject.transform.localScale.x - 0.01f), Convert.ToSingle(gameObject.transform.localScale.x - 0.01f), Convert.ToSingle(gameObject.transform.localScale.x));
            }
            else if (deathTime >= 240)
            {
                GameObject.FindGameObjectWithTag("recordname").GetComponent<Records>().endgame = true;
                //GameSettings.restartGame();
            }
            deathTime += 1;
        }
        else if (dying && !GetComponent<EnemyBehaviour>().finalEnemy)
        {
            deathTime += Time.deltaTime;
            if (GetComponent<EnemyBehaviour>().deathSound != null && !playedDeathSound)
            {
                GetComponent<AudioSource>().clip = GetComponent<EnemyBehaviour>().deathSound;
                GetComponent<AudioSource>().Play();
                playedDeathSound = true;
            }
            if (deathTime >= 1)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 0;
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    public int GetDamage()
    {
        return -(Health - MaxHealth);
    }
    IEnumerator blinking(){
        if (!dying)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            yield return new WaitForSeconds(blinktime);
            if (!dying) gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    void startblinking()
    {
        StartCoroutine(blinking());
    }

    public void Hurt(int damage)
    {   
        Health -= damage;
        healthPercent = (float)Health / (float)MaxHealth;
        if (healthPercent < 0.15f && !blink)
        {
            blink =true;
            InvokeRepeating("startblinking", 0.01f, blinktime * 2);
        }

        gameObject.GetComponent<EnemyBehaviour>().Stagger(0.1f, false);
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
        if (GetComponent<EnemyBehaviour>().finalEnemy)
        {
            BossBar.GetComponent<BossBar>().UpdateValue(Health, MaxHealth);
        }
        
    }

    void Explode()
    {
        GetComponent<EnemyBehaviour>().DestroyTowers(transform.localPosition.x, transform.localPosition.y);
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(Map.Enemies);
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(gameObject.transform.localPosition, enemy.transform.localPosition) <= 60) enemy.GetComponent<EnemyHealth>().Hurt(15);
        }
    }

    void death()
    {
        CancelInvoke();
        Assets.CoinCounter.ChangeCoinCounter(enemyCoin, false);
        Map.Enemies.Remove(gameObject);
        if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
        {
            towerInfo.GetComponent<TowerInfo>().HideInfo();
        }
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        gameObject.GetComponent<EnemyBehaviour>().moveSpeed = 0f;
        dying = true;
        gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
        if (GetComponent<EnemyBehaviour>().enemyType == "Bob-Omb Buddy")
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            gameObject.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
            Explode();
        }
        if (GetComponent<EnemyBehaviour>().finalEnemy)
        {
            Music.GetComponent<Music>().PlayNew("Victory");
            GetComponent<EnemyBehaviour>().moveSpeed = 0f;
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets) Destroy(bullet);
            Map.Victory = true;
            Map.gameSpeed = 0;
            Time.timeScale = Map.gameSpeed;
            if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
            {
                towerInfo.GetComponent<TowerInfo>().HideInfo();
            }
        }
        else
        {
            if (GetComponent<EnemyBehaviour>().enemyType != "Bob-Omb Buddy") gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
        }
    }

}
