using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialImage : MonoBehaviour
{ 
    public void tutorialscript(){
        Map.SkippedTutorial = true;
    }
    void Update()
    {
        if (Map.SkippedTutorial) Destroy(gameObject);
    }

}
