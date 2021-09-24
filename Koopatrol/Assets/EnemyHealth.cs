using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static float HealthEnemy = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HealthEnemy == 0)
        {
            death();
        }

        //test death
        if (Input.GetKey(KeyCode.Space))
        {
            death();
        }
    }

    void death()
    {
        Destroy(gameObject);
    }

}
