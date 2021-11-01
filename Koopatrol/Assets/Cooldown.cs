using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject CreateCooldownCounter(GameObject parent)
    {
        GameObject cooldown = Instantiate(gameObject);
        cooldown.transform.position = parent.transform.position;
        cooldown.transform.SetParent(parent.transform, true);
        cooldown.GetComponent<CanvasGroup>().alpha = 1f;
        cooldown.tag = "CooldownCounter";
        return cooldown;
    }
}
