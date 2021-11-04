using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSettingValues : MonoBehaviour
{
    [SerializeField]
    GameObject MusicVolume;
    [SerializeField]
    GameObject SoundVolume;
    [SerializeField]
    GameObject ShowCooldowns;
    // Start is called before the first frame update
    void Start()
    {
        MusicVolume.GetComponentInChildren<Slider>().value = Map.MusicVolume;
        MusicVolume.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Music volume " + Map.MusicVolume + "%";
        SoundVolume.GetComponentInChildren<Slider>().value = Map.SoundVolume;
        SoundVolume.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Sound volume " + Map.SoundVolume + "%";
        ShowCooldowns.GetComponentInChildren<Slider>().value = Map.ShowCooldowns;
        if (Map.ShowCooldowns == 0) ShowCooldowns.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Show cooldowns all";
        else ShowCooldowns.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Show cooldowns " + Map.ShowCooldowns + "+";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
