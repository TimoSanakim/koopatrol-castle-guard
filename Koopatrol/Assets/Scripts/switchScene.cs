using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour
{
    public string loadingScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void sceneSwitch()
    {
        Debug.Log("load scene");
        SceneManager.LoadScene(loadingScene);
    }
    public void quitgame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }
}
