using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackOrForeground : MonoBehaviour
{
    public float moveRight = 0f;
    public float moveUp = 0f;
    Vector3 HomePos;
    // Start is called before the first frame update
    void Start()
    {
        HomePos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + moveRight, gameObject.transform.position.y + moveUp, gameObject.transform.position.z);
        if (gameObject.transform.position.x >= HomePos.x + 1024f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 2048f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.x >= HomePos.x - 1024f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 2048f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.y >= HomePos.y + 1024f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 2048f, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.y >= HomePos.y - 1024f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2048f, gameObject.transform.position.z);
        }
    }
}