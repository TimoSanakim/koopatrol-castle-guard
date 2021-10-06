using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastResortAttack : MonoBehaviour
{
    bool used = false;
    public int costs;
    public int damage;
    public GameObject hurt;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
        
            if(!used){
            used = true;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if(enemy.GetComponent<EnemyBehaviour>().isClone){
                        enemy.GetComponent<EnemyHealth>().Hurt(damage);
                    }
                }
           Assets.CoinCounter.ChangeCoinCounter(-costs);
           }
        }
    }
}
