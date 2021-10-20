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
        public float speed = 50f;
        public float freezeAmount = 0;
        public Sprite[] bulletSprites;
        // Use this for initialization
        void Start()
        {
            if (homingTarget == null && isClone) LookAt(targetPosition);
        }
        void LookAt(Vector3 targetPosition)
        {
            Vector3 difference = targetPosition - gameObject.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
        // Update is called once per frame
        void Update()
        {
            if (isClone)
            {
                timeFlying += Time.deltaTime;
                if (homingTarget != null && !homingTarget.GetComponent<EnemyHealth>().dying) LookAt(homingTarget.transform.position);
                gameObject.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
                foreach (GameObject enemy in Map.Enemies)
                {
                    if (Vector3.Distance(enemy.transform.position, gameObject.transform.position) <= 20)
                    {
                        if (power != 0) enemy.GetComponent<EnemyHealth>().Hurt(power);
                        if (freezeAmount != 0) enemy.GetComponent<EnemyBehaviour>().Freeze(freezeAmount);
                        Destroy(gameObject);
                        break;
                    }
                }
                if (timeFlying >= 60) Destroy(gameObject);
            }
        }
    }
}