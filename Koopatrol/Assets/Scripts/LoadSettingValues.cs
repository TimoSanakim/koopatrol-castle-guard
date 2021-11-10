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
    [SerializeField]
    GameObject DefaultTargetPriority;
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
        switch (Map.DefaultTargetPriority)
        {
            case 0:
                DefaultTargetPriority.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Nearest";
                break;
            case 1:
                DefaultTargetPriority.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Shortest Path";
                break;
            case 2:
                DefaultTargetPriority.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Longest Path";
                break;
            case 3:
                DefaultTargetPriority.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Least Health";
                break;
            case 4:
                DefaultTargetPriority.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Most Health";
                break;
            case 5:
                DefaultTargetPriority.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Default Focus: Most Damage";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
