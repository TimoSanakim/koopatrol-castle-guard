using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector3 HomePos;
    // Start is called before the first frame update
    void Start()
    {
        HomePos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
        if (gameObject.transform.position.x >= HomePos.x + 1024f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 2048f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
