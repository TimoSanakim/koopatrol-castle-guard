using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{ 
    public void tutorialscript(){
        Map.SkippedTutorial = true;
    }
    void Update()
    {
        if (Map.SkippedTutorial) Destroy(gameObject);
    }

}
