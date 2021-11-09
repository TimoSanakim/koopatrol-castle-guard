using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class levelCreator : MonoBehaviour, IPointerClickHandler
{
    public GameObject SelectedOption;
    public GameObject SelectedOptionHighlight;
    public GameObject BackgroundTile;
    public GameObject Tile;
    public Sprite[] backgroundTiles;
    public Sprite[] blockedTiles;
    public Color[] PathColors;
    public Sprite[] backgrounds;
    public Sprite[] foregrounds;
    public GameObject Background;
    public GameObject Foreground;
    public GameObject GroundOption;
    public GameObject PathOption;
    public GameObject BlockedOption;
    public GameObject Waves;
    public GameObject SelectedWave;
    public GameObject WaveSelector;
    public GameObject AddWaveButton;
    public GameObject GamemodeText;
    public GameObject GamemodePanel;
    int selectedWave = 0;
    int style = 0;
    int gamemode = 0;
    int coinCount = 30;
    int yoshiInWave = -1;
    // Start is called before the first frame update
    void Start()
    {
        GamemodePanel.GetComponent<CanvasGroup>().alpha = 0;
        GamemodePanel.GetComponent<CanvasGroup>().interactable = false;
        GamemodePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ChangeSelectedOption(GameObject option)
    {
        SelectedOption = option;
        SelectedOptionHighlight.transform.position = option.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Assets.CoinCounter.CoinCount = 300;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject background = Instantiate(BackgroundTile);
            background.transform.SetParent(gameObject.transform.GetChild(0), true);
            background.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            float x = background.transform.localPosition.x;
            float y = background.transform.localPosition.y;
            background.transform.localPosition = new Vector2(Convert.ToInt32(x / 50) * 50, Convert.ToInt32(y / 50) * 50);
            background.transform.localScale = new Vector3(1, 1, 1);
            if (SelectedOption.name == "PlaceObstruction") background.GetComponent<Image>().sprite = SelectedOption.GetComponent<Image>().sprite;

            GameObject tile = Instantiate(Tile);
            tile.transform.SetParent(gameObject.transform.GetChild(1), true);
            tile.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            tile.transform.localPosition = new Vector2(Convert.ToInt32(x / 50) * 50, Convert.ToInt32(y / 50) * 50);
            tile.transform.localScale = new Vector3(1, 1, 1);
            if (SelectedOption.name != "PlaceObstruction" && SelectedOption.name != "PlaceGround")
            {
                tile.GetComponent<Image>().sprite = SelectedOption.GetComponent<Image>().sprite;
                tile.GetComponent<Image>().color = SelectedOption.GetComponent<Image>().color;
                if (SelectedOption.name == "PlacePath") tile.tag = "Path";
                else tile.tag = "Castle";
            }
            else if (SelectedOption.name == "PlaceObstruction") tile.tag = "BlockedGround";
            else tile.tag = "Ground";
            tile.GetComponent<MapLocation>().RecalculateNeighbors();
            tile.GetComponent<MapLocation>().Recalculate();
        }
    }
    public void ChangeStyle()
    {
        style += 1;
        if (style == 3) style = 0;
        GroundOption.GetComponent<Image>().sprite = backgroundTiles[style];
        PathOption.GetComponent<Image>().color = PathColors[style];
        BlockedOption.GetComponent<Image>().sprite = blockedTiles[style];
        foreach (GameObject tile in Map.Tiles)
        {
            if (tile.tag == "Path") tile.GetComponent<Image>().color = PathColors[style];
            tile.transform.parent.transform.parent.GetChild(0).GetChild(tile.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = backgroundTiles[style];
            if (tile.tag == "BlockedGround") tile.transform.parent.transform.parent.GetChild(0).GetChild(tile.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = blockedTiles[style];
        }
        if (style == 0)
        {
            Background.GetComponent<Image>().sprite = backgrounds[0];
            Background.GetComponent<MovingBackOrForeground>().moveRight = 0.1f;
            Background.GetComponent<MovingBackOrForeground>().moveUp = 0f;
            Foreground.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            Foreground.GetComponent<MovingBackOrForeground>().moveRight = 0f;
            Foreground.GetComponent<MovingBackOrForeground>().moveUp = 0f;
        }
        if (style == 1)
        {
            Background.GetComponent<Image>().sprite = backgrounds[1];
            Background.GetComponent<MovingBackOrForeground>().moveRight = 0f;
            Background.GetComponent<MovingBackOrForeground>().moveUp = 0f;
            Foreground.GetComponent<Image>().sprite = foregrounds[0];
            Foreground.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            Foreground.GetComponent<MovingBackOrForeground>().moveRight = 0.5f;
            Foreground.GetComponent<MovingBackOrForeground>().moveUp = -1f;
        }
        if (style == 2)
        {
            Background.GetComponent<Image>().sprite = backgrounds[2];
            Background.GetComponent<MovingBackOrForeground>().moveRight = 0f;
            Background.GetComponent<MovingBackOrForeground>().moveUp = 0f;
            Foreground.GetComponent<Image>().sprite = foregrounds[1];
            Foreground.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            Foreground.GetComponent<MovingBackOrForeground>().moveRight = 2f;
            Foreground.GetComponent<MovingBackOrForeground>().moveUp = -0.5f;
        }
        Background.transform.localPosition = new Vector3(0, 0, 0);
        Foreground.transform.localPosition = new Vector3(0, 0, 0);
    }
    public void ModifyGamemode()
    {
        if (GamemodePanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            GamemodePanel.GetComponent<CanvasGroup>().alpha = 0;
            GamemodePanel.GetComponent<CanvasGroup>().interactable = false;
            GamemodePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            GamemodePanel.GetComponent<CanvasGroup>().alpha = 1;
            GamemodePanel.GetComponent<CanvasGroup>().interactable = true;
            GamemodePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    public void ChangeGamemode()
    {
        gamemode += 1;
        if (gamemode == 3) gamemode = 0;
        switch (gamemode)
        {
            case 0:
                GamemodeText.GetComponent<TextMeshProUGUI>().text = "Normal map";
                Waves.GetComponent<Waves>().EndlessToad.GetComponent<EnemyHealth>().enemyCoin = 2;
                Waves.GetComponent<Waves>().EndlessCaptainToad.GetComponent<EnemyHealth>().enemyCoin = 10;
                Waves.GetComponent<Waves>().EndlessYoshi.GetComponent<EnemyHealth>().enemyCoin = 50;
                Waves.GetComponent<Waves>().EndlessLuigi.GetComponent<EnemyHealth>().enemyCoin = 300;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().endlessMode = false;
                break;
            case 1:
                GamemodeText.GetComponent<TextMeshProUGUI>().text = "Limited-coin map";
                Waves.GetComponent<Waves>().EndlessToad.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessCaptainToad.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessYoshi.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessLuigi.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().endlessMode = false;
                break;
            case 2:
                GamemodeText.GetComponent<TextMeshProUGUI>().text = "Endless map";
                Waves.GetComponent<Waves>().EndlessToad.GetComponent<EnemyHealth>().enemyCoin = 2;
                Waves.GetComponent<Waves>().EndlessCaptainToad.GetComponent<EnemyHealth>().enemyCoin = 10;
                Waves.GetComponent<Waves>().EndlessYoshi.GetComponent<EnemyHealth>().enemyCoin = 50;
                Waves.GetComponent<Waves>().EndlessLuigi.GetComponent<EnemyHealth>().enemyCoin = 300;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyHealth>().enemyCoin = 700;
                Waves.GetComponent<Waves>().endlessMode = true;
                break;
        }
    }
    public void ChangeCoinCount(GameObject value)
    {
        coinCount = Convert.ToInt32(value.GetComponent<TextMeshProUGUI>().text);
    }
    public void AddWave()
    {
        if (Waves.GetComponent<Waves>().TheWaves.Count <= 98)
        {
            Waves.GetComponent<Waves>().TheWaves.Add(new Waves.serializableClass());
            Waves.GetComponent<Waves>().TheWaves[Waves.GetComponent<Waves>().TheWaves.Count - 1].wave = new List<GameObject>();
            int round = 0;
            WaveSelector.GetComponent<TMP_Dropdown>().options.Clear();
            foreach (Waves.serializableClass wave in Waves.GetComponent<Waves>().TheWaves)
            {
                round += 1;
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = "Round " + Convert.ToString(round);
                WaveSelector.GetComponent<TMP_Dropdown>().options.Add(option);
            }
            WaveSelector.GetComponent<TMP_Dropdown>().value = round - 1;
            SelectWave();
            if (Waves.GetComponent<Waves>().TheWaves.Count == 99)
            {
                AddWaveButton.GetComponent<CanvasGroup>().alpha = 0.5f;
                AddWaveButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
                AddWaveButton.GetComponent<CanvasGroup>().interactable = false;
            }
        }
    }
    public void AddEnemyToWave(GameObject selector)
    {
        int enemyindex = selector.GetComponent<TMP_Dropdown>().value;
        switch (enemyindex)
        {
            case 0:
                Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Add(Waves.GetComponent<Waves>().EndlessToad);
                break;
            case 1:
                Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Add(Waves.GetComponent<Waves>().EndlessCaptainToad);
                break;
            case 2:
                Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Add(Waves.GetComponent<Waves>().EndlessYoshi);
                if (!Waves.GetComponent<Waves>().hasSpawnedYoshi) Waves.GetComponent<Waves>().TheWaves[selectedWave].music = "Yoshi";
                if (!Waves.GetComponent<Waves>().hasSpawnedYoshi) yoshiInWave = selectedWave;
                Waves.GetComponent<Waves>().hasSpawnedYoshi = true;
                break;
            case 3:
                if (yoshiInWave < selectedWave && yoshiInWave != -1) 
                { 
                    Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Add(Waves.GetComponent<Waves>().EndlessLuigi);
                    if (!Waves.GetComponent<Waves>().hasSpawnedLuigi) Waves.GetComponent<Waves>().TheWaves[selectedWave].music = "Luigi";
                    Waves.GetComponent<Waves>().hasSpawnedLuigi = true;
                }
                else
                {
                    Map.WriteToLog("You may only add a Luigi if a previous round had a yoshi.");
                }
                break;
        }
        SelectWave();
    }
    public void SelectWave()
    {
        selectedWave = WaveSelector.GetComponent<TMP_Dropdown>().value;
        int childCount = SelectedWave.transform.GetChild(0).GetChild(0).childCount;
        while (childCount != 1)
        {
            Destroy(SelectedWave.transform.GetChild(0).GetChild(0).GetChild(childCount - 1).gameObject);
            childCount -= 1;
        }
        if (Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Count == 0)
        {
            SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<CanvasGroup>().interactable = true;
            SelectedWave.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(SelectedWave.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.x, 50);
        }
        else
        {
            SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<CanvasGroup>().interactable = false;
            int enemynumber = 0;
            foreach (GameObject enemy in Waves.GetComponent<Waves>().TheWaves[selectedWave].wave)
            {
                GameObject entry = Instantiate(SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
                entry.transform.SetParent(SelectedWave.transform.GetChild(0).GetChild(0));
                entry.transform.localScale = new Vector3(1, 1, 1);
                entry.transform.localPosition = new Vector3(SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).localPosition.x, SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).localPosition.y - (enemynumber * 50), SelectedWave.transform.GetChild(0).GetChild(0).GetChild(0).localPosition.z);
                entry.GetComponent<TextMeshProUGUI>().text = enemy.GetComponent<EnemyBehaviour>().enemyType;
                entry.GetComponent<CanvasGroup>().alpha = 1f;
                entry.GetComponent<CanvasGroup>().blocksRaycasts = true;
                entry.GetComponent<CanvasGroup>().interactable = true;
                enemynumber += 1;
            }
            SelectedWave.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(SelectedWave.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.x, 50 * enemynumber);
        }
    }

    public void RemoveEntry(GameObject selectedEnemy)
    {
        if (selectedEnemy.GetComponent<TextMeshProUGUI>().text == "Empty") RemoveWave();
        else RemoveEnemyFromWave(selectedEnemy);
    }
    public void RemoveEnemyFromWave(GameObject selectedEnemy)
    {
        Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.RemoveAt(selectedEnemy.transform.GetSiblingIndex() - 1);
        if (selectedEnemy.GetComponent<TextMeshProUGUI>().text == "Yoshi" && Waves.GetComponent<Waves>().TheWaves[selectedWave].music == "Yoshi" && !Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Contains(Waves.GetComponent<Waves>().EndlessYoshi))
        {
            Waves.GetComponent<Waves>().TheWaves[selectedWave].music = "";
            yoshiInWave = -1;
            for (int waveIndex = selectedWave; waveIndex < Waves.GetComponent<Waves>().TheWaves.Count; waveIndex ++)
            {
                if (Waves.GetComponent<Waves>().TheWaves[waveIndex].wave.Contains(Waves.GetComponent<Waves>().EndlessYoshi))
                {
                    Waves.GetComponent<Waves>().TheWaves[waveIndex].music = "Yoshi";
                    yoshiInWave = waveIndex;
                    break;
                }
            }
            if (yoshiInWave == -1)
            {
                Waves.GetComponent<Waves>().hasSpawnedYoshi = false;
                Waves.GetComponent<Waves>().hasSpawnedLuigi = false;
                foreach (Waves.serializableClass wave in Waves.GetComponent<Waves>().TheWaves)
                {
                    wave.wave.RemoveAll(item => item == Waves.GetComponent<Waves>().EndlessLuigi);
                    wave.music = "";
                }
            }
        }
        else if (selectedEnemy.GetComponent<TextMeshProUGUI>().text == "Luigi" && Waves.GetComponent<Waves>().TheWaves[selectedWave].music == "Luigi" && !Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Contains(Waves.GetComponent<Waves>().EndlessLuigi))
        {
            Waves.GetComponent<Waves>().TheWaves[selectedWave].music = "";
            bool exists = false;
            for (int waveIndex = selectedWave; waveIndex < Waves.GetComponent<Waves>().TheWaves.Count; waveIndex++)
            {
                if (Waves.GetComponent<Waves>().TheWaves[waveIndex].wave.Contains(Waves.GetComponent<Waves>().EndlessLuigi))
                {
                    Waves.GetComponent<Waves>().TheWaves[waveIndex].music = "Luigi";
                    exists = true;
                    break;
                }
            }
            if (!exists) Waves.GetComponent<Waves>().hasSpawnedLuigi = false;
        }
        SelectWave();
    }
    public void RemoveWave()
    {
        if (Waves.GetComponent<Waves>().TheWaves.Count == 99)
        {
            AddWaveButton.GetComponent<CanvasGroup>().alpha = 1f;
            AddWaveButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            AddWaveButton.GetComponent<CanvasGroup>().interactable = true;
        }
        if (Waves.GetComponent<Waves>().TheWaves.Count != 1)
        {
            Waves.GetComponent<Waves>().TheWaves.RemoveAt(selectedWave);
            int round = 0;
            WaveSelector.GetComponent<TMP_Dropdown>().options.Clear();
            foreach (Waves.serializableClass wave in Waves.GetComponent<Waves>().TheWaves)
            {
                round += 1;
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = "Round " + Convert.ToString(round);
                WaveSelector.GetComponent<TMP_Dropdown>().options.Add(option);
            }
            if (WaveSelector.GetComponent<TMP_Dropdown>().value != 0) WaveSelector.GetComponent<TMP_Dropdown>().value -= 1;
            SelectWave();
        }
    }
    //Only allow saving if there's a valid path to the castle, if gamemode is not endless, add a wave at the end containing only mario
}
