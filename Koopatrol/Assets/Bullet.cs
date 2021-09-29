using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Bullet : MonoBehaviour
    {
        public GameObject homingTarget = null;
        public Vector3 targetPosition;
        public int power = 0;
        public int timeFlying = 0;
        public bool isClone = false;
        public float speed = 50f;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            timeFlying += 1;
            if (homingTarget != null)
            {
                gameObject.transform.position = Vector2.MoveTowards(transform.position, homingTarget.transform.position, speed * Time.deltaTime);
            }
            else
            {
                gameObject.transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (((gameObject.transform.position.y - enemy.transform.position.y >= 30 && gameObject.transform.position.y - enemy.transform.position.y <= 70) || (gameObject.transform.position.y - enemy.transform.position.y >= -70 && gameObject.transform.position.y - enemy.transform.position.y <= -30)) && ((gameObject.transform.position.x - enemy.transform.position.x >= 30 && gameObject.transform.position.x - enemy.transform.position.x <= 70) || (gameObject.transform.position.x - enemy.transform.position.x >= -70 && gameObject.transform.position.x - enemy.transform.position.x <= -30)))
                {
                    enemy.GetComponent<EnemyHealth>().Hurt(power);
                    Destroy(gameObject);
                }
            }
            if (timeFlying == 3600 && isClone) Destroy(gameObject);
        }
    }
}