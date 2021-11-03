using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaField : MonoBehaviour
{
    public bool isClone;
    public bool killNextTime = false;
    public bool grow = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, 0.07f, Space.Self);
        if (isClone)
        {
            if (killNextTime) Destroy(gameObject);
            else
            {
                killNextTime = true;
                if (grow)
                {
                    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + Time.deltaTime, gameObject.transform.localScale.y + Time.deltaTime, 1);
                    if (gameObject.transform.localScale.x >= 2.75f)
                    {
                        gameObject.transform.localScale = new Vector3(2.75f, 2.75f, 1);
                        grow = false;
                        List<GameObject> enemies = new List<GameObject>();
                        enemies.AddRange(Map.Enemies);
                        foreach (GameObject enemy in enemies)
                        {
                            if (enemy.GetComponent<EnemyBehaviour>().isClone && Vector3.Distance(enemy.transform.position, transform.position) <= 75)
                            {
                                enemy.GetComponent<EnemyHealth>().Hurt(5);
                            }
                        }
                    }
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - Time.deltaTime, gameObject.transform.localScale.y - Time.deltaTime, 1);
                    if (gameObject.transform.localScale.x <= 1)
                    {
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        grow = true;
                    }
                }
            }
        }
    }
    public GameObject CreateLavaField(GameObject parent)
    {
        GameObject lavaField = Instantiate(gameObject);
        lavaField.transform.position = parent.transform.position;
        lavaField.transform.SetParent(parent.transform.parent, true);
        lavaField.GetComponent<CanvasGroup>().alpha = 1f;
        lavaField.GetComponent<LavaField>().isClone = true;
        lavaField.tag = "LavaAttack";
        return lavaField;
    }
}
