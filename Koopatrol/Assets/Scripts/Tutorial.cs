using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject draggingTower;
    [SerializeField] GameObject WaveManager;
    [SerializeField] GameObject EnemySpawner;
    [SerializeField] GameObject FocusButton;
    [SerializeField] GameObject UpgradeButton;
    [SerializeField] GameObject SellButton;
    [SerializeField] GameObject HideButton;
    [SerializeField] GameObject InfoBox;
    [SerializeField] GameObject[] towerOptions;
    [SerializeField] GameObject[] selectedPositions;
    public static GameObject TutorialPosition;

    TextMeshProUGUI TextField;
    int stepHint = 0;
    int step = 0;
    // Start is called before the first frame update
    void Start()
    {
        TextField = GetComponent<TextMeshProUGUI>();
        UpgradeButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        UpgradeButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UpgradeButton.GetComponent<CanvasGroup>().interactable = false;
        FocusButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        FocusButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        FocusButton.GetComponent<CanvasGroup>().interactable = false;
        SellButton.GetComponent<CanvasGroup>().alpha = 0.3f;
        SellButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        SellButton.GetComponent<CanvasGroup>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (step)
        {
            case 0:
                TextField.text = "Welcome to \"Koopatrol: Castle Guard\"\nBefore you begin placing towers, you might want to check the stats of the towers. Click on the highlighted tower.";
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    towerOptions[0].transform.localScale = new Vector3(towerOptions[0].transform.localScale.x + 0.003f, towerOptions[0].transform.localScale.y + 0.003f, towerOptions[0].transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        towerOptions[0].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    }
                    towerOptions[0].transform.localScale = new Vector3(towerOptions[0].transform.localScale.x - 0.003f, towerOptions[0].transform.localScale.y - 0.003f, towerOptions[0].transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().selectedTower == towerOptions[0] && !InfoBox.GetComponent<TowerInfo>().hidden)
                {
                    step += 1;
                    towerOptions[0].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    stepHint = 0;
                    TextField.text = "Each sprite has a meaning: <sprite=0>= how much damage it deals to enemies. <sprite=1>= how fast the attack moves. <sprite=2>= how long it takes before it can attack again. <sprite=3>= how far the tower can see. Press hide to hide the infobox.";
                }
                break;
            case 1:
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    HideButton.transform.localScale = new Vector3(HideButton.transform.localScale.x + 0.003f, HideButton.transform.localScale.y + 0.003f, HideButton.transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        HideButton.transform.localScale = new Vector3(1, 1, 1);
                    }
                    HideButton.transform.localScale = new Vector3(HideButton.transform.localScale.x - 0.003f, HideButton.transform.localScale.y - 0.003f, HideButton.transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().slide == 2)
                {
                    step += 1;
                    TutorialPosition = selectedPositions[0];
                    Assets.CoinCounter.ChangeCoinCounter(15, false);
                    HideButton.transform.localScale = new Vector3(1, 1, 1);
                    TextField.text = "Now that we know the info about the tower, we should go ahead and place it. Drag and drop the tower on the right spot.";
                    draggingTower.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    draggingTower.GetComponent<draggingTower>().towerType = "GoombaTower";
                    draggingTower.GetComponent<Image>().sprite = towerOptions[0].GetComponent<Image>().sprite;
                    draggingTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    draggingTower.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0;
                    draggingTower.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
                    towerOptions[0].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.TutorialPosition;
                }
                break;
            case 2:
                if (Assets.CoinCounter.GetCoinCount() == 0)
                {
                    step += 1;
                    stepHint = 0;
                    TextField.text = "Now, an enemy will appear. Select the enemy to see details about it.";
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 0;
                    Map.gameSpeed = 1;
                    Time.timeScale = 1;
                    Map.paused = false;
                    towerOptions[0].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.None;
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[0].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[0].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[0].transform.position, selectedPositions[0].transform.position, Convert.ToSingle(stepHint) / 300f);
                    }
                }
                break;
            case 3:
                if (Map.Enemies.Count == 1)
                {
                    Map.paused = true;
                    Time.timeScale = 0;
                    Map.gameSpeed = 1;
                    if (stepHint <= 60)
                    {
                        stepHint += 1;
                        Map.Enemies[0].transform.localScale = new Vector3(Map.Enemies[0].transform.localScale.x + 0.003f, Map.Enemies[0].transform.localScale.y + 0.003f, Map.Enemies[0].transform.localScale.z);
                    }
                    else if (stepHint <= 120)
                    {
                        stepHint += 1;
                        if (stepHint == 120)
                        {
                            stepHint = 0;
                            Map.Enemies[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
                        }
                        Map.Enemies[0].transform.localScale = new Vector3(Map.Enemies[0].transform.localScale.x - 0.003f, Map.Enemies[0].transform.localScale.y - 0.003f, Map.Enemies[0].transform.localScale.z);
                    }
                    if (InfoBox.GetComponent<TowerInfo>().selectedTower == Map.Enemies[0])
                    {
                        step += 1;
                        Map.Enemies[0].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                        stepHint = 0;
                        TextField.text = "<sprite=6>= how much health and max health the enemy has. <sprite=1>= how fast the enemy moves. Some enemies have special behaviors, we get to those later. Each enemy deal damage equal to their health to your castle if they reach it. Press the hide button to continue.";
                    }
                }
                break;
            case 4:
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    HideButton.transform.localScale = new Vector3(HideButton.transform.localScale.x + 0.003f, HideButton.transform.localScale.y + 0.003f, HideButton.transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        HideButton.transform.localScale = new Vector3(1, 1, 1);
                    }
                    HideButton.transform.localScale = new Vector3(HideButton.transform.localScale.x - 0.003f, HideButton.transform.localScale.y - 0.003f, HideButton.transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().slide == 2)
                {
                    step += 1;
                    HideButton.transform.localScale = new Vector3(1, 1, 1);
                    TextField.text = "Let's see if our tower can handle this alone.";
                    Map.paused = false;
                    Time.timeScale = 1;
                    Map.gameSpeed = 1;
                }
                break;
            case 5:
                if (Map.Enemies.Count == 0)
                {
                    step += 1;
                    TextField.text = "Hmm... That was rather easy, wasn't it?\nOh look, we got 2 coins from defeating that enemy. The stronger the enemy, the more goins it gives.\nOh, look, more enemies are appearing.";
                    WaveManager.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 5;
                    EnemySpawner.GetComponent<SpawnEnemies>().stopSpawning = false;
                }
                break;
            case 6:
                if (Map.Enemies.Count == 8)
                {
                    step += 1;
                }
                break;
            case 7:
                if (Map.Enemies.Count == 7)
                {
                    step += 1;
                    TextField.text = "I don't think it can handle this alone... Here, I'll give you some more coins so you can place another tower near the castle.";
                    TutorialPosition = selectedPositions[1];
                    Assets.CoinCounter.ChangeCoinCounter(15, false);
                    Map.paused = true;
                    Time.timeScale = 0;
                    Map.gameSpeed = 0;
                    towerOptions[0].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.TutorialPosition;
                }
                break;
            case 8:
                if (Assets.CoinCounter.GetCoinCount() == 4)
                {
                    step += 1;
                    stepHint = 0;
                    TextField.text = "I think this is enough.";
                    Map.gameSpeed = 1;
                    Time.timeScale = 1;
                    Map.paused = false;
                    towerOptions[0].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.None;
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[0].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[1].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[0].transform.position, selectedPositions[1].transform.position, Convert.ToSingle(stepHint) / 250f);
                    }
                }
                break;
            case 9:
                if (Map.Enemies.Count == 6)
                {
                    step += 1;
                    TextField.text = "Hm, they're getting far. Let's make sure the towers focus on the enemy closest to the castle. Selected the highlighed towers, and then press the focus button.";
                    Map.paused = true;
                    Time.timeScale = 0;
                    Map.gameSpeed = 0;
                }
                break;
            case 10:
                if (InfoBox.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>() != null && InfoBox.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().TargetPriority == 0)
                {
                    FocusButton.GetComponent<CanvasGroup>().alpha = 1f;
                    FocusButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    FocusButton.GetComponent<CanvasGroup>().interactable = true;
                }
                else
                {
                    FocusButton.GetComponent<CanvasGroup>().alpha = 0.3f;
                    FocusButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    FocusButton.GetComponent<CanvasGroup>().interactable = false;
                }
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    if (InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[0] && selectedPositions[0].GetComponent<MapLocation>().TargetPriority == 0) selectedPositions[0].transform.localScale = new Vector3(selectedPositions[0].transform.localScale.x + 0.003f, selectedPositions[0].transform.localScale.y + 0.003f, selectedPositions[0].transform.localScale.z);
                    if (InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[1] && selectedPositions[1].GetComponent<MapLocation>().TargetPriority == 0) selectedPositions[1].transform.localScale = new Vector3(selectedPositions[1].transform.localScale.x + 0.003f, selectedPositions[1].transform.localScale.y + 0.003f, selectedPositions[1].transform.localScale.z);
                    if (InfoBox.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>() != null && InfoBox.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().TargetPriority == 0) FocusButton.transform.localScale = new Vector3(FocusButton.transform.localScale.x + 0.003f, FocusButton.transform.localScale.y + 0.003f, FocusButton.transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        selectedPositions[0].transform.localScale = new Vector3(1, 1, 1);
                        selectedPositions[1].transform.localScale = new Vector3(1, 1, 1);
                        FocusButton.transform.localScale = new Vector3(1, 1, 1);
                    }
                    if (InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[0] && selectedPositions[0].GetComponent<MapLocation>().TargetPriority == 0) selectedPositions[0].transform.localScale = new Vector3(selectedPositions[0].transform.localScale.x - 0.003f, selectedPositions[0].transform.localScale.y - 0.003f, selectedPositions[0].transform.localScale.z);
                    if (InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[1] && selectedPositions[1].GetComponent<MapLocation>().TargetPriority == 0) selectedPositions[1].transform.localScale = new Vector3(selectedPositions[1].transform.localScale.x - 0.003f, selectedPositions[1].transform.localScale.y - 0.003f, selectedPositions[1].transform.localScale.z);
                    if (InfoBox.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>() != null && InfoBox.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().TargetPriority == 0) FocusButton.transform.localScale = new Vector3(FocusButton.transform.localScale.x - 0.003f, FocusButton.transform.localScale.y - 0.003f, FocusButton.transform.localScale.z);
                }
                if (selectedPositions[0].GetComponent<MapLocation>().TargetPriority == 1 && selectedPositions[1].GetComponent<MapLocation>().TargetPriority == 1)
                {
                    step += 1;
                    selectedPositions[0].transform.localScale = new Vector3(1, 1, 1);
                    selectedPositions[1].transform.localScale = new Vector3(1, 1, 1);
                    FocusButton.transform.localScale = new Vector3(1, 1, 1);
                    stepHint = 0;
                    TextField.text = "This should help defeat the enemies.";
                    Map.paused = false;
                    Time.timeScale = 1;
                    Map.gameSpeed = 1;
                }
                break;
            case 11:
                if (Map.Enemies.Count == 0)
                {
                    step += 1;
                    TextField.text = "Only one got through, that's fine, after all, we need a toad to tell Mario the princess is in another castle. This was only the beginning though. Let's place something more exiting! Let's take a look at the third tower.";
                }
                break;
            case 12:
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    towerOptions[2].transform.localScale = new Vector3(towerOptions[2].transform.localScale.x + 0.003f, towerOptions[2].transform.localScale.y + 0.003f, towerOptions[2].transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        towerOptions[2].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    }
                    towerOptions[2].transform.localScale = new Vector3(towerOptions[2].transform.localScale.x - 0.003f, towerOptions[2].transform.localScale.y - 0.003f, towerOptions[2].transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().selectedTower == towerOptions[2] && !InfoBox.GetComponent<TowerInfo>().hidden)
                {
                    step += 1;
                    towerOptions[2].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    stepHint = 0;
                    TextField.text = "Hmm, no damage sprite, but has another one instead... <sprite=4>= the time enemies will be frozen. A frozen enemy won't move, even when it gets hit. So this tower won't deal damage, but will stop enemies. Let's place it strategically between the Goombas.";
                    Assets.CoinCounter.ChangeCoinCounter(44, false);
                    draggingTower.GetComponent<draggingTower>().towerType = "FreezieTower";
                    draggingTower.GetComponent<Image>().sprite = towerOptions[2].GetComponent<Image>().sprite;
                    TutorialPosition = selectedPositions[2];
                    towerOptions[2].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.TutorialPosition;
                }
                break;
            case 13:
                if (Assets.CoinCounter.GetCoinCount() == 0)
                {
                    step += 1;
                    stepHint = 0;
                    TextField.text = "And while we're at it, let's look at another tower as well! What do you think about the fourth tower?";
                    towerOptions[2].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.None;
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[2].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[2].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[2].transform.position, selectedPositions[2].transform.position, Convert.ToSingle(stepHint) / 250f);
                    }
                }
                break;
            case 14:
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    towerOptions[3].transform.localScale = new Vector3(towerOptions[3].transform.localScale.x + 0.003f, towerOptions[3].transform.localScale.y + 0.003f, towerOptions[3].transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        towerOptions[3].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    }
                    towerOptions[3].transform.localScale = new Vector3(towerOptions[3].transform.localScale.x - 0.003f, towerOptions[3].transform.localScale.y - 0.003f, towerOptions[3].transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().selectedTower == towerOptions[3] && !InfoBox.GetComponent<TowerInfo>().hidden)
                {
                    step += 1;
                    towerOptions[3].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    stepHint = 0;
                    TextField.text = "Again no damage sprite, but yet another new one... <sprite=5>= the time enemies will be stunned when hit. Like with the freezies, this stops the enemies' movement, but it is a little bit different. A frozen enemy can be stunned, but not frozen again, and vice versa. When stunned by this, the ice will break though, so let's put it a bit further away from the freezie.";
                    Assets.CoinCounter.ChangeCoinCounter(75, false);
                    draggingTower.GetComponent<draggingTower>().towerType = "Thwomp";
                    draggingTower.GetComponent<Image>().sprite = towerOptions[3].GetComponent<Image>().sprite;
                    TutorialPosition = selectedPositions[3];
                    towerOptions[3].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.TutorialPosition;
                }
                break;
            case 15:
                if (Assets.CoinCounter.GetCoinCount() == 0)
                {
                    step += 1;
                    stepHint = 0;
                    TextField.text = "Let the enemies come! We got this!";
                    WaveManager.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessCaptainToad);
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 2;
                    EnemySpawner.GetComponent<SpawnEnemies>().stopSpawning = false;
                    towerOptions[3].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.None;
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[3].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[3].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[3].transform.position, selectedPositions[3].transform.position, Convert.ToSingle(stepHint) / 400f);
                    }
                }
                break;
            case 16:
                if (Map.Enemies.Count == 6)
                {
                    step += 1;
                }
                break;
            case 17:
                if (Map.Enemies.Count == 5)
                {
                    step += 1;
                    TextField.text = "Oh my, the last toad is a bit stronger! Let's get some more reinforcements going!";
                    TutorialPosition = selectedPositions[4];
                    Assets.CoinCounter.ChangeCoinCounter(76, false);
                    draggingTower.GetComponent<draggingTower>().towerType = "KoopaTower";
                    draggingTower.GetComponent<Image>().sprite = towerOptions[1].GetComponent<Image>().sprite;
                    Map.paused = true;
                    Time.timeScale = 0;
                    Map.gameSpeed = 0;
                    towerOptions[1].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.TutorialPosition;
                }
                break;
            case 18:
                if (Assets.CoinCounter.GetCoinCount() == 40)
                {
                    step += 1;
                    stepHint = 0;
                    TutorialPosition = selectedPositions[5];
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[1].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[4].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[1].transform.position, selectedPositions[4].transform.position, Convert.ToSingle(stepHint) / 200f);
                    }
                }
                break;
            case 19:
                if (Assets.CoinCounter.GetCoinCount() == 0)
                {
                    step += 1;
                    stepHint = 0;
                    TextField.text = "Let's hope King Bowser won't fire us if things go a bit wrong here.";
                    Map.gameSpeed = 1;
                    Time.timeScale = 1;
                    Map.paused = false;
                    towerOptions[1].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.None;
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[1].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[5].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[1].transform.position, selectedPositions[5].transform.position, Convert.ToSingle(stepHint) / 200f);
                    }
                }
                break;
            case 20:
                if (Map.Enemies.Count == 0)
                {
                    step += 1;
                    TextField.text = "That was great! Let's move on to upgrading towers. Select the highlighted tower.";
                }
                break;
            case 21:
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    selectedPositions[0].transform.localScale = new Vector3(selectedPositions[0].transform.localScale.x + 0.003f, selectedPositions[0].transform.localScale.y + 0.003f, selectedPositions[0].transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        selectedPositions[0].transform.localScale = new Vector3(1, 1, 1);
                    }
                    selectedPositions[0].transform.localScale = new Vector3(selectedPositions[0].transform.localScale.x - 0.003f, selectedPositions[0].transform.localScale.y - 0.003f, selectedPositions[0].transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().selectedTower == selectedPositions[0] && !InfoBox.GetComponent<TowerInfo>().hidden)
                {
                    step += 1;
                    selectedPositions[0].transform.localScale = new Vector3(1, 1, 1);
                    stepHint = 0;
                    TextField.text = "Select upgrade to improve this tower.";
                }
                break;
            case 22:
                if (InfoBox.GetComponent<TowerInfo>().selectedTower == selectedPositions[0])
                {
                    UpgradeButton.GetComponent<CanvasGroup>().alpha = 1f;
                    UpgradeButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    UpgradeButton.GetComponent<CanvasGroup>().interactable = true;
                }
                else
                {
                    UpgradeButton.GetComponent<CanvasGroup>().alpha = 0.3f;
                    UpgradeButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    UpgradeButton.GetComponent<CanvasGroup>().interactable = false;
                }
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    UpgradeButton.transform.localScale = new Vector3(UpgradeButton.transform.localScale.x + 0.003f, UpgradeButton.transform.localScale.y + 0.003f, UpgradeButton.transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        UpgradeButton.transform.localScale = new Vector3(1, 1, 1);
                    }
                    UpgradeButton.transform.localScale = new Vector3(UpgradeButton.transform.localScale.x - 0.003f, UpgradeButton.transform.localScale.y - 0.003f, UpgradeButton.transform.localScale.z);
                }
                if (selectedPositions[0].GetComponent<MapLocation>().towerLevel == 2)
                {
                    step += 1;
                    Assets.CoinCounter.ChangeCoinCounter(42, false);
                    UpgradeButton.transform.localScale = new Vector3(1, 1, 1);
                    TextField.text = "Go ahead and upgrade all highlighted towers.";
                }
                break;
            case 23:
                if (InfoBox.GetComponent<TowerInfo>().selectedTower == selectedPositions[1] || InfoBox.GetComponent<TowerInfo>().selectedTower == selectedPositions[4] || InfoBox.GetComponent<TowerInfo>().selectedTower == selectedPositions[5])
                {
                    UpgradeButton.GetComponent<CanvasGroup>().alpha = 1f;
                    UpgradeButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    UpgradeButton.GetComponent<CanvasGroup>().interactable = true;
                }
                else
                {
                    UpgradeButton.GetComponent<CanvasGroup>().alpha = 0.3f;
                    UpgradeButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    UpgradeButton.GetComponent<CanvasGroup>().interactable = false;
                }
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    if (selectedPositions[1].GetComponent<MapLocation>().towerLevel == 1 && InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[1]) selectedPositions[1].transform.localScale = new Vector3(selectedPositions[1].transform.localScale.x + 0.003f, selectedPositions[1].transform.localScale.y + 0.003f, selectedPositions[1].transform.localScale.z);
                    if (selectedPositions[4].GetComponent<MapLocation>().towerLevel == 1 && InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[4]) selectedPositions[4].transform.localScale = new Vector3(selectedPositions[4].transform.localScale.x + 0.003f, selectedPositions[4].transform.localScale.y + 0.003f, selectedPositions[4].transform.localScale.z);
                    if (selectedPositions[5].GetComponent<MapLocation>().towerLevel == 1 && InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[5]) selectedPositions[5].transform.localScale = new Vector3(selectedPositions[5].transform.localScale.x + 0.003f, selectedPositions[5].transform.localScale.y + 0.003f, selectedPositions[5].transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        selectedPositions[1].transform.localScale = new Vector3(1, 1, 1);
                        selectedPositions[4].transform.localScale = new Vector3(1, 1, 1);
                        selectedPositions[5].transform.localScale = new Vector3(1, 1, 1);
                    }
                    if (selectedPositions[1].GetComponent<MapLocation>().towerLevel == 1 && InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[1]) selectedPositions[1].transform.localScale = new Vector3(selectedPositions[1].transform.localScale.x - 0.003f, selectedPositions[1].transform.localScale.y - 0.003f, selectedPositions[1].transform.localScale.z);
                    if (selectedPositions[4].GetComponent<MapLocation>().towerLevel == 1 && InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[4]) selectedPositions[4].transform.localScale = new Vector3(selectedPositions[4].transform.localScale.x - 0.003f, selectedPositions[4].transform.localScale.y - 0.003f, selectedPositions[4].transform.localScale.z);
                    if (selectedPositions[5].GetComponent<MapLocation>().towerLevel == 1 && InfoBox.GetComponent<TowerInfo>().selectedTower != selectedPositions[5]) selectedPositions[5].transform.localScale = new Vector3(selectedPositions[5].transform.localScale.x - 0.003f, selectedPositions[5].transform.localScale.y - 0.003f, selectedPositions[5].transform.localScale.z);
                }
                if (selectedPositions[1].GetComponent<MapLocation>().towerLevel == 2 && selectedPositions[4].GetComponent<MapLocation>().towerLevel == 2 && selectedPositions[5].GetComponent<MapLocation>().towerLevel == 2)
                {
                    step += 1;
                    selectedPositions[1].transform.localScale = new Vector3(1, 1, 1);
                    selectedPositions[4].transform.localScale = new Vector3(1, 1, 1);
                    selectedPositions[5].transform.localScale = new Vector3(1, 1, 1);
                    stepHint = 0;
                    TextField.text = "Let's see who's next.";
                    WaveManager.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessCaptainToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessCaptainToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessCaptainToad);
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 2;
                    EnemySpawner.GetComponent<SpawnEnemies>().stopSpawning = false;
                    UpgradeButton.GetComponent<CanvasGroup>().alpha = 0.3f;
                    UpgradeButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    UpgradeButton.GetComponent<CanvasGroup>().interactable = false;
                }
                break;
            case 24:
                if (Map.Enemies.Count == 1)
                {
                    step += 1;
                }
                break;
            case 25:
                if (Map.Enemies.Count == 0)
                {
                    step += 1;
                    TextField.text = "Wow, we didn't need to do anything! Let's see who's next!";
                    WaveManager.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessYoshi);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].music = "Yoshi";
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 4;
                    EnemySpawner.GetComponent<SpawnEnemies>().stopSpawning = false;
                }
                break;
            case 26:
                if (Map.Enemies.Count == 1)
                {
                    step += 1;
                    TextField.text = "Eek! It's Yoshi! He moves a lot faster. Be careful!";
                }
                break;
            case 27:
                if (Map.Enemies.Count == 0)
                {
                    step += 1;
                    TextField.text = "Whew... that was close! King bower would've been angry if he made it. Let's place a bullet blaster just to make sure he won't pass in the future. Bullet blasters can only be placed next to a path, that is not an inner corner. They will fire onto the path next to them.";
                    Assets.CoinCounter.ChangeCoinCounter(8, false);
                    TutorialPosition = selectedPositions[6];
                    towerOptions[4].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.TutorialPosition;
                    draggingTower.GetComponent<draggingTower>().towerType = "BulletBlaster";
                    draggingTower.GetComponent<Image>().sprite = towerOptions[4].GetComponent<Image>().sprite;
                }
                break;
            case 28:
                if (Assets.CoinCounter.GetCoinCount() == 0)
                {
                    step += 1;
                    stepHint = 0;
                    TextField.text = "Let's also get a Magikoopa in here; they won't attack, but will enhance our towers near them.";
                    draggingTower.GetComponent<draggingTower>().towerType = "MagikoopaTower";
                    draggingTower.GetComponent<Image>().sprite = towerOptions[6].GetComponent<Image>().sprite;
                    draggingTower.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0;
                    draggingTower.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
                    Assets.CoinCounter.ChangeCoinCounter(150, false);
                    TutorialPosition = selectedPositions[7];
                    towerOptions[4].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.None;
                    towerOptions[6].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.TutorialPosition;
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    draggingTower.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
                    draggingTower.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[4].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[6].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[4].transform.position, selectedPositions[6].transform.position, Convert.ToSingle(stepHint) / 300f);
                    }
                }
                break;
            case 29:
                if (Assets.CoinCounter.GetCoinCount() == 0)
                {
                    step += 1;
                    stepHint = 0;
                    TextField.text = "Who's next?";
                    towerOptions[6].GetComponent<TowerOption>().validPosition = Assets.ValidPosition.None;
                    WaveManager.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessCaptainToad);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessYoshi);
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 1;
                    EnemySpawner.GetComponent<SpawnEnemies>().stopSpawning = false;
                }
                else if (!draggingTower.GetComponent<draggingTower>().dragging)
                {
                    if (draggingTower.GetComponent<CanvasGroup>().alpha == 0f)
                    {
                        draggingTower.transform.position = towerOptions[6].transform.position;
                        draggingTower.GetComponent<CanvasGroup>().alpha = 0.5f;
                        stepHint = 0;
                    }
                    else if (draggingTower.GetComponent<CanvasGroup>().transform.position == selectedPositions[7].transform.position)
                    {
                        draggingTower.GetComponent<CanvasGroup>().alpha -= 0.01f;
                    }
                    else
                    {
                        stepHint += 1;
                        draggingTower.transform.position = Vector3.Lerp(towerOptions[6].transform.position, selectedPositions[7].transform.position, Convert.ToSingle(stepHint) / 300f);
                    }
                }
                break;
            case 30:
                if (Map.Enemies.Count == 1)
                {
                    step += 1;
                }
                break;
            case 31:
                if (Map.Enemies.Count == 0)
                {
                    step += 1;
                    TextField.text = "Hahahaha! Not this time!";
                    WaveManager.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessLuigi);
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].music = "Luigi";
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 3;
                    EnemySpawner.GetComponent<SpawnEnemies>().stopSpawning = false;
                }
                break;
            case 32:
                if (Map.Enemies.Count == 1)
                {
                    step += 1;
                    TextField.text = "Oh no! It's Luigi! He's a scaredy cat though, so when he'll see the bullet blaster, he'll go running home... wait.. what!? He's evading it! Oh no! Get him Koopas!";
                }
                break;
            case 33:
                if (Map.Enemies.Count == 0)
                {
                    step += 1;
                    TextField.text = "Ok, so when Luigi arrives, Mario is not far behind! I've ran out of coins to give you, and I don't think we're going to make it...";
                    WaveManager.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
                    WaveManager.GetComponent<Waves>().TheWaves[WaveManager.GetComponent<Waves>().TheWaves.Count - 1].wave.Add(WaveManager.GetComponent<Waves>().EndlessMario);
                    WaveManager.GetComponent<Waves>().currentWaveDelay = 8;
                    EnemySpawner.GetComponent<SpawnEnemies>().stopSpawning = false;
                }
                break;
            case 34:
                if (Map.Enemies.Count == 1)
                {
                    step += 1;
                    TextField.text = "Well... we're going to die! Let's take a look at Mario's ability before we abandon the castle though.";
                    Map.paused = true;
                    Time.timeScale = 0;
                    Map.gameSpeed = 1;
                }
                break;
            case 35:
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    Map.Enemies[0].transform.localScale = new Vector3(Map.Enemies[0].transform.localScale.x + 0.003f, Map.Enemies[0].transform.localScale.y + 0.003f, Map.Enemies[0].transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        Map.Enemies[0].transform.localScale = new Vector3(1.5f, 1.5f, 1);
                    }
                    Map.Enemies[0].transform.localScale = new Vector3(Map.Enemies[0].transform.localScale.x - 0.003f, Map.Enemies[0].transform.localScale.y - 0.003f, Map.Enemies[0].transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().selectedTower == Map.Enemies[0])
                {
                    step += 1;
                    Map.Enemies[0].transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                    stepHint = 0;
                    TextField.text = "Looks like he'll destroy Goomba towers when he gets close to them. He can also completely obliterate our castle, no matter how much health he has. Well... I'm out! I'll take the fall for this, go protect the other castles.";
                }
                break;
            case 36:
                if (stepHint <= 60)
                {
                    stepHint += 1;
                    HideButton.transform.localScale = new Vector3(HideButton.transform.localScale.x + 0.003f, HideButton.transform.localScale.y + 0.003f, HideButton.transform.localScale.z);
                }
                else if (stepHint <= 120)
                {
                    stepHint += 1;
                    if (stepHint == 120)
                    {
                        stepHint = 0;
                        HideButton.transform.localScale = new Vector3(1, 1, 1);
                    }
                    HideButton.transform.localScale = new Vector3(HideButton.transform.localScale.x - 0.003f, HideButton.transform.localScale.y - 0.003f, HideButton.transform.localScale.z);
                }
                if (InfoBox.GetComponent<TowerInfo>().slide == 2)
                {
                    SceneManager.LoadScene("Levelselect");
                    Map.LoadedLevel = "Levelselect";
                }
                break;
        }
    }
}
