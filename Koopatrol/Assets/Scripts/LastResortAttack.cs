using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LastResortAttack : MonoBehaviour, IPointerClickHandler
{
    bool used = false;
    public int costs;
    public int damage;


    // Start is called before the first frame update
    void Start()
    {
        
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
                used = true;
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone)
                    {
                        enemy.GetComponent<EnemyHealth>().Hurt(damage);
                    }
                }
                Assets.CoinCounter.ChangeCoinCounter(-costs);
            }
        }
    }
}
