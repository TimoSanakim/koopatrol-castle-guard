using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LastResortAttack : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    GameObject towerInfo;
    AudioSource audioData;
    bool used = false;
    public int costs;
    public int damage;
    public string description = "A last resort attack, dealing 15 damage to all enemies on the map. Can only be used once!";


    // Start is called before the first frame update
    void Start()
    {
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!used && Assets.CoinCounter.GetCoinCount() >= costs)
            {
                List<GameObject> enemies = new List<GameObject>();
                enemies.AddRange(Map.Enemies);
                used = true;
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone)
                    {
                        enemy.GetComponent<EnemyHealth>().Hurt(damage);
                    }
                }
                Assets.CoinCounter.ChangeCoinCounter(-costs, false);
                audioData = GetComponent<AudioSource>();
                audioData.Play(0);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        towerInfo.GetComponent<TowerInfo>().ShowInfo();
        towerInfo.GetComponent<TowerInfo>().selectedTower = gameObject;
    }
}
