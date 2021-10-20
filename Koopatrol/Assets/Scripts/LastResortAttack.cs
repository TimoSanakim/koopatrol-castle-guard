using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LastResortAttack : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    GameObject towerInfo;
    GameObject LavaAttack;
    Vector3 Castle;
    bool used = false;
    public int costs;
    public int damage;
    public float progress = 0f;
    public string description = "A last resort attack, dealing 15 damage to all enemies on the map. Can only be used once!";


    // Start is called before the first frame update
    void Start()
    {
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        LavaAttack = GameObject.FindGameObjectWithTag("LavaAttack");
        Castle = GameObject.FindGameObjectWithTag("Castle").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Assets.CoinCounter.GetCoinCount() >= costs && !used)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0.6f;
        }
        if (used && progress <= 10)
        {
            if (progress == 0f)
            {
                LavaAttack.transform.position = Castle;
                if (Map.gameSpeed != 0) GetComponent<AudioSource>().Play();
            }
            LavaAttack.transform.localScale = new Vector3(LavaAttack.transform.localScale.x + (10 * Time.deltaTime), LavaAttack.transform.localScale.y + (10 * Time.deltaTime), 1);
            LavaAttack.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            progress += Time.deltaTime;
            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(Map.Enemies);
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().isClone && !enemy.GetComponent<EnemyHealth>().HitByLava && Vector3.Distance(enemy.transform.position, LavaAttack.transform.position) <= 50 * LavaAttack.transform.localScale.x && Vector3.Distance(enemy.transform.position, LavaAttack.transform.position) >= 50 * (LavaAttack.transform.localScale.x - (10 * Time.deltaTime)) - 10)
                {
                    enemy.GetComponent<EnemyHealth>().HitByLava = true;
                    enemy.GetComponent<EnemyHealth>().Hurt(damage);
                }
            }
        }
        else if (used && progress >= 10 && progress <= 15)
        {
            LavaAttack.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            LavaAttack.transform.localScale = new Vector3(0f, 0f, 0f);
            progress = 20;
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
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (towerInfo.GetComponent<TowerInfo>().selectedTower != gameObject) towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
    }
}
