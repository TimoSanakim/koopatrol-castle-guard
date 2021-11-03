using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSettingValues : MonoBehaviour
{
    [SerializeField]
    GameObject MusicVolume;
    [SerializeField]
    GameObject SoundVolume;
    // Start is called before the first frame update
    void Start()
    {
        MusicVolume.GetComponentInChildren<Slider>().value = Map.MusicVolume;
        SoundVolume.GetComponentInChildren<Slider>().value = Map.SoundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
