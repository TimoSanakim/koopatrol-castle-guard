using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEffect : MonoBehaviour
{
    bool isClone = false;
    public bool killNextTime = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isClone)
        {
            gameObject.transform.Rotate(0, 0, 1, Space.Self);
            if (killNextTime) Destroy(gameObject);
            else killNextTime = true;
        }
    }

    public GameObject CreateMagicEffect(GameObject parent)
    {
        GameObject magicEffect = Instantiate(gameObject);
        magicEffect.transform.position = parent.transform.position;
        magicEffect.transform.SetParent(parent.transform, true);
        magicEffect.GetComponent<MagicEffect>().isClone = true;
        magicEffect.GetComponent<CanvasGroup>().alpha = 1f;
        magicEffect.GetComponent<CanvasGroup>().interactable = true;
        magicEffect.GetComponent<CanvasGroup>().blocksRaycasts = true;
        magicEffect.tag = "MagicEffect";
        return magicEffect;
    }
}
