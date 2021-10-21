using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayNew("Toads");
    }
    public AudioClip[] Clips;
    public AudioClip[] IntroClips;
    public AudioClip GameOver;
    public AudioClip Victory;

    public string music = "Toads";
    bool musicStopped = false;

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying && !musicStopped)
        {
            switch (music)
            {
                case "Toads":
                    gameObject.GetComponent<AudioSource>().clip = Clips[0] ;
                    break;
                case "Yoshi":
                    gameObject.GetComponent<AudioSource>().clip = Clips[1];
                    break;
                case "Luigi":
                    gameObject.GetComponent<AudioSource>().clip = Clips[2];
                    break;
                case "Mario":
                    gameObject.GetComponent<AudioSource>().clip = Clips[3];
                    break;
            }
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<AudioSource>().loop = true;
        }
    }
    public void PlayNew(string music)
    {
        gameObject.GetComponent<AudioSource>().Stop();
        switch (music)
        {
            case "Toads":
                gameObject.GetComponent<AudioSource>().clip = IntroClips[0];
                break;
            case "Yoshi":
                gameObject.GetComponent<AudioSource>().clip = IntroClips[1];
                break;
            case "Luigi":
                gameObject.GetComponent<AudioSource>().clip = IntroClips[2];
                break;
            case "Mario":
                gameObject.GetComponent<AudioSource>().clip = IntroClips[3];
                break;
            case "GameOver":
                gameObject.GetComponent<AudioSource>().clip = GameOver;
                musicStopped = true;
                break;
            case "Victory":
                gameObject.GetComponent<AudioSource>().clip = Victory;
                musicStopped = true;
                break;
        }
        this.music = music;
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<AudioSource>().loop = false;
    }
}
