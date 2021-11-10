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
        public float timeFlying = 0;
        public bool isClone = false;
        public float initSpeed = 0.05208333333333333333333333333333f;
        public float speed;
        public float freezeAmount = 0;
        public Sprite[] bulletSprites;
        // Use this for initialization
        public void LookAt(Vector3 targetPosition)
        {
            Vector3 difference = targetPosition - gameObject.transform.localPosition;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
        void Start()
        {
            speed = (speed/50) * initSpeed * Screen.width;
        }
        // Update is called once per frame
        void Update()
        {
            if (isClone)
            {
                timeFlying += Time.deltaTime;
                if (homingTarget != null && !homingTarget.GetComponent<EnemyHealth>().dying) LookAt(homingTarget.transform.localPosition);
                gameObject.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
                foreach (GameObject enemy in Map.Enemies)
                {
                    if (Vector3.Distance(enemy.transform.localPosition, gameObject.transform.localPosition) <= 20)
                    {
                        if (power != 0) enemy.GetComponent<EnemyHealth>().Hurt(power);
                        if (freezeAmount != 0) enemy.GetComponent<EnemyBehaviour>().Freeze(freezeAmount, false);
                        Destroy(gameObject);
                        break;
                    }
                }
                if (timeFlying >= 60) Destroy(gameObject);
            }
        }
    }
}