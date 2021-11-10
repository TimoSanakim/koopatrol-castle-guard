using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public GameObject CastleOption;
    public GameObject Waves;
    public GameObject SelectedWave;
    public GameObject CoinCounter;
    public GameObject WaveSelector;
    public GameObject AddEnemyButton;
    public GameObject AddWaveButton;
    public GameObject GamemodeText;
    public GameObject GamemodePanel;
    public GameObject LoadLevelPanel;
    public GameObject SaveLevelPanel;
    public GameObject LoadLevelOptions;
    public GameObject SaveLevelName;
    public GameObject LoadButton;
    public GameObject TowerInfo;
    public GameObject[] Towers;
    int selectedWave = 0;
    int style = 0;
    int gamemode = 0;
    int coinCount = 30;
    int yoshiInWave = -1;
    int luigiInWave = -1;
    int ID = 0;
    bool previouslySaved = false;
    // Start is called before the first frame update
    void Start()
    {
        GamemodePanel.GetComponent<CanvasGroup>().alpha = 0;
        GamemodePanel.GetComponent<CanvasGroup>().interactable = false;
        GamemodePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        LoadLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
        LoadLevelPanel.GetComponent<CanvasGroup>().interactable = false;
        LoadLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        SaveLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
        SaveLevelPanel.GetComponent<CanvasGroup>().interactable = false;
        SaveLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        LoadLevelOptions.GetComponent<TMP_Dropdown>().value = 0;
        LoadLevelOptions.GetComponent<TMP_Dropdown>().options.Clear();
        while (File.Exists(Application.dataPath + "/customlevel" + ID))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/customlevel" + ID);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            ID++;
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = saveObject.name;
            LoadLevelOptions.GetComponent<TMP_Dropdown>().options.Add(option);
        }
        if (File.Exists(Application.dataPath + "/customlevel" + ID)){
                LoadLevelOptions.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = LoadLevelOptions.GetComponent<TMP_Dropdown>().options[0].text;
            if (LoadLevelOptions.GetComponent<TMP_Dropdown>().options.Count == 0)
            {
                LoadButton.GetComponent<CanvasGroup>().alpha = 0.5f;
                LoadButton.GetComponent<CanvasGroup>().interactable = false;
                LoadButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
        
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
                Waves.GetComponent<Waves>().EndlessBobOmbBuddy.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyBehaviour>().finalEnemy = true;
                Waves.GetComponent<Waves>().endlessMode = false;
                break;
            case 1:
                GamemodeText.GetComponent<TextMeshProUGUI>().text = "Limited-coin map";
                Waves.GetComponent<Waves>().EndlessToad.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessCaptainToad.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessYoshi.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessLuigi.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessBobOmbBuddy.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyBehaviour>().finalEnemy = true;
                Waves.GetComponent<Waves>().endlessMode = false;
                break;
            case 2:
                GamemodeText.GetComponent<TextMeshProUGUI>().text = "Endless map";
                Waves.GetComponent<Waves>().EndlessToad.GetComponent<EnemyHealth>().enemyCoin = 2;
                Waves.GetComponent<Waves>().EndlessCaptainToad.GetComponent<EnemyHealth>().enemyCoin = 10;
                Waves.GetComponent<Waves>().EndlessYoshi.GetComponent<EnemyHealth>().enemyCoin = 50;
                Waves.GetComponent<Waves>().EndlessLuigi.GetComponent<EnemyHealth>().enemyCoin = 300;
                Waves.GetComponent<Waves>().EndlessBobOmbBuddy.GetComponent<EnemyHealth>().enemyCoin = 0;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyHealth>().enemyCoin = 700;
                Waves.GetComponent<Waves>().EndlessMario.GetComponent<EnemyBehaviour>().finalEnemy = false;
                Waves.GetComponent<Waves>().endlessMode = true;
                break;
        }
    }
    public void ChangeCoinCount(GameObject value)
    {
        if (value.GetComponent<TMP_InputField>().text == "" || value.GetComponent<TMP_InputField>().text == null) value.GetComponent<TMP_InputField>().text = "0";
        coinCount = Convert.ToInt32(value.GetComponent<TMP_InputField>().text);
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
                    if (!Waves.GetComponent<Waves>().hasSpawnedLuigi) luigiInWave = selectedWave;
                    Waves.GetComponent<Waves>().hasSpawnedLuigi = true;
                }
                else
                {
                    Map.WriteToLog("You may only add a Luigi if a previous round had a Yoshi.");
                }
                break;
            case 4:
                if (luigiInWave < selectedWave && luigiInWave != -1)
                {
                    Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Add(Waves.GetComponent<Waves>().EndlessBobOmbBuddy);
                }
                else
                {
                    Map.WriteToLog("You may only add a Bob-Omb Buddy if a previous round had a Luigi.");
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
            AddEnemyButton.GetComponent<CanvasGroup>().alpha = 1f;
            AddEnemyButton.GetComponent<CanvasGroup>().interactable = true;
            AddEnemyButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
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
            if (enemynumber == 60)
            {
                AddEnemyButton.GetComponent<CanvasGroup>().alpha = 0.5f;
                AddEnemyButton.GetComponent<CanvasGroup>().interactable = false;
                AddEnemyButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                AddEnemyButton.GetComponent<CanvasGroup>().alpha = 1f;
                AddEnemyButton.GetComponent<CanvasGroup>().interactable = true;
                AddEnemyButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
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
            for (int waveIndex = selectedWave; waveIndex < Waves.GetComponent<Waves>().TheWaves.Count; waveIndex++)
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
                    wave.wave.RemoveAll(item => item == Waves.GetComponent<Waves>().EndlessBobOmbBuddy);
                    wave.music = "";
                }
            }
        }
        else if (selectedEnemy.GetComponent<TextMeshProUGUI>().text == "Luigi" && Waves.GetComponent<Waves>().TheWaves[selectedWave].music == "Luigi" && !Waves.GetComponent<Waves>().TheWaves[selectedWave].wave.Contains(Waves.GetComponent<Waves>().EndlessLuigi))
        {
            Waves.GetComponent<Waves>().TheWaves[selectedWave].music = "";
            luigiInWave = -1;
            for (int waveIndex = selectedWave; waveIndex < Waves.GetComponent<Waves>().TheWaves.Count; waveIndex++)
            {
                if (Waves.GetComponent<Waves>().TheWaves[waveIndex].wave.Contains(Waves.GetComponent<Waves>().EndlessLuigi))
                {
                    Waves.GetComponent<Waves>().TheWaves[waveIndex].music = "Luigi";
                    luigiInWave = waveIndex;
                    break;
                }
            }
            if (luigiInWave == -1)
            {
                Waves.GetComponent<Waves>().hasSpawnedLuigi = false;
                foreach (Waves.serializableClass wave in Waves.GetComponent<Waves>().TheWaves)
                {
                    wave.wave.RemoveAll(item => item == Waves.GetComponent<Waves>().EndlessBobOmbBuddy);
                }
            }
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
    [Serializable]
    public class SaveTile
    {
        public Vector3 position;
        public Quaternion rotation;
        public string type;
        public string towerType;
        public int towerLevel;
        public int towerFocus;
    }
    [Serializable]
    public class Wave
    {
        public string music;
        public List<string> enemies;
    }
    [Serializable]
    public class SaveObject
    {
        public List<SaveTile> tiles;
        public List<Wave> waves;
        public string name;
        public int gamemode;
        public int style;
        public int coinCount;
    }

    public void SaveScreen()
    {
        if (!previouslySaved)
        {
            if (SaveLevelPanel.GetComponent<CanvasGroup>().alpha == 1)
            {
                SaveLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
                SaveLevelPanel.GetComponent<CanvasGroup>().interactable = false;
                SaveLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                LoadLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
                LoadLevelPanel.GetComponent<CanvasGroup>().interactable = false;
                LoadLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                SaveLevelPanel.GetComponent<CanvasGroup>().alpha = 1;
                SaveLevelPanel.GetComponent<CanvasGroup>().interactable = true;
                SaveLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else SaveLevel();
    }

    public void SaveLevel()
    {
        Waves.GetComponent<Waves>().SearchForPaths();
        if (Map.PossiblePaths.Count != 0 && SaveLevelName.GetComponent<TMP_InputField>().text != "")
        {
            LoadLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
            LoadLevelPanel.GetComponent<CanvasGroup>().interactable = false;
            LoadLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            SaveLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
            SaveLevelPanel.GetComponent<CanvasGroup>().interactable = false;
            SaveLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            SaveLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
            SaveLevelPanel.GetComponent<CanvasGroup>().interactable = false;
            SaveLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            SaveObject saveObject = new SaveObject();
            saveObject.name = SaveLevelName.GetComponent<TMP_InputField>().text;
            saveObject.gamemode = gamemode;
            saveObject.style = style;
            saveObject.coinCount = coinCount;
            saveObject.tiles = new List<SaveTile>();
            foreach (GameObject t in Map.Tiles)
            {
                if (t.name != "Image")
                {
                    SaveTile tile = new SaveTile();
                    tile.position = t.transform.localPosition;
                    tile.rotation = t.transform.rotation;
                    tile.type = t.tag;
                    tile.towerType = t.GetComponent<MapLocation>().towerType;
                    tile.towerLevel = t.GetComponent<MapLocation>().towerLevel;
                    tile.towerFocus = t.GetComponent<MapLocation>().TargetPriority;
                    saveObject.tiles.Add(tile);
                }
            }
            saveObject.waves = new List<Wave>();
            foreach (Waves.serializableClass wave in Waves.GetComponent<Waves>().TheWaves)
            {
                Wave savewave = new Wave();
                savewave.music = wave.music;
                savewave.enemies = new List<string>();
                foreach (GameObject enemy in wave.wave)
                {
                    savewave.enemies.Add(enemy.GetComponent<EnemyBehaviour>().enemyType);
                }
                saveObject.waves.Add(savewave);
            }
            if (gamemode != 2)
            {
                Wave savewave = new Wave();
                savewave.music = "Mario";
                savewave.enemies = new List<string>();
                savewave.enemies.Add("Mario");
                saveObject.waves.Add(savewave);
            }
            string json = JsonUtility.ToJson(saveObject);

            File.WriteAllText(Application.dataPath + "/customlevel" + ID, json);
            if (!previouslySaved)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = saveObject.name;
                LoadLevelOptions.GetComponent<TMP_Dropdown>().options.Add(option);
                LoadButton.GetComponent<CanvasGroup>().alpha = 1;
                LoadButton.GetComponent<CanvasGroup>().interactable = true;
                LoadButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            previouslySaved = true;
            Map.WriteToLog("Saved map as \"" + saveObject.name + "\".");
        }
        else if (Map.PossiblePaths.Count != 0 && SaveLevelName.GetComponent<TMP_InputField>().text == "")
        {
            Map.WriteToLog("You have to enter a name!");
        }
        else
        {
            Map.WriteToLog("You can only save if there's a valid path to a castle!");
        }
    }

    public void LoadScreen()
    {
        if (LoadLevelPanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            LoadLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
            LoadLevelPanel.GetComponent<CanvasGroup>().interactable = false;
            LoadLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            SaveLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
            SaveLevelPanel.GetComponent<CanvasGroup>().interactable = false;
            SaveLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            LoadLevelPanel.GetComponent<CanvasGroup>().alpha = 1;
            LoadLevelPanel.GetComponent<CanvasGroup>().interactable = true;
            LoadLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void LoadLevel()
    {
        int index = LoadLevelOptions.GetComponent<TMP_Dropdown>().value;
        TowerInfo.GetComponent<TowerInfo>().HideInfo();
        TowerInfo.GetComponent<TowerInfo>().selectedTower = null;
        GamemodePanel.GetComponent<CanvasGroup>().alpha = 0;
        GamemodePanel.GetComponent<CanvasGroup>().interactable = false;
        GamemodePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        LoadLevelPanel.GetComponent<CanvasGroup>().alpha = 0;
        LoadLevelPanel.GetComponent<CanvasGroup>().interactable = false;
        LoadLevelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        ID = index;
        List<GameObject> oldTiles = new List<GameObject>();
        oldTiles.AddRange(Map.Tiles);
        foreach (GameObject t in oldTiles)
        {
            if (t.name != "Image")
            {
                t.tag = "Finish";
                if (Waves.GetComponent<Waves>().StartingPositions.Contains(t)) Waves.GetComponent<Waves>().StartingPositions.Remove(t);
                Map.Tiles.Remove(t);
                Destroy(t.transform.parent.transform.parent.GetChild(0).GetChild(t.transform.GetSiblingIndex()).gameObject);
                Destroy(t);
            }
        }
        previouslySaved = true;
        string saveString = File.ReadAllText(Application.dataPath + "/customlevel" + index);
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
        SaveLevelName.GetComponent<TMP_InputField>().text = saveObject.name;
        if (style != saveObject.style) ChangeStyle();
        if (style != saveObject.style) ChangeStyle();
        if (gamemode != saveObject.gamemode) ChangeGamemode();
        if (gamemode != saveObject.gamemode) ChangeGamemode();
        coinCount = saveObject.coinCount;
        CoinCounter.GetComponent<TMP_InputField>().text = Convert.ToString(coinCount);
        foreach (SaveTile t in saveObject.tiles)
        {
            GameObject background = Instantiate(BackgroundTile);
            background.transform.SetParent(gameObject.transform.GetChild(0), true);
            background.transform.localPosition = new Vector3(t.position.x, t.position.y, t.position.z);
            background.transform.localScale = new Vector3(1, 1, 1);
            if (t.type == "BlockedGround") background.GetComponent<Image>().sprite = BlockedOption.GetComponent<Image>().sprite;

            GameObject tile = Instantiate(Tile);
            tile.transform.SetParent(gameObject.transform.GetChild(1), true);
            tile.transform.localPosition = new Vector3(t.position.x, t.position.y, t.position.z);
            tile.transform.localScale = new Vector3(1, 1, 1);
            if (t.type != "BlockedGround" && t.type != "Ground")
            {
                tile.GetComponent<Image>().color = new Color(1, 1, 1);
                if (t.type == "Path")
                {
                    tile.GetComponent<Image>().color = PathOption.GetComponent<Image>().color;
                    tile.GetComponent<Image>().sprite = PathOption.GetComponent<Image>().sprite;
                }
                if (t.type == "PathTower") tile.GetComponent<MapLocation>().originalColor = PathOption.GetComponent<Image>().color;
                if (t.type == "Castle")
                {
                    tile.GetComponent<Image>().sprite = CastleOption.GetComponent<Image>().sprite;
                }
            }
            tile.transform.rotation = t.rotation;
            tile.tag = t.type;
            switch (t.towerType)
            {
                case "GoombaTower":
                    tile.GetComponent<Image>().sprite = Towers[0].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[0].GetComponent<TowerOption>().towerSprites);
                    break;
                case "KoopaTower":
                    tile.GetComponent<Image>().sprite = Towers[1].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[1].GetComponent<TowerOption>().towerSprites);
                    break;
                case "FreezieTower":
                    tile.GetComponent<Image>().sprite = Towers[2].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[2].GetComponent<TowerOption>().towerSprites);
                    break;
                case "Thwomp":
                    tile.GetComponent<Image>().sprite = Towers[3].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[3].GetComponent<TowerOption>().towerSprites);
                    break;
                case "BulletBlaster":
                    tile.GetComponent<Image>().sprite = Towers[4].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[4].GetComponent<TowerOption>().towerSprites);
                    break;
                case "PiranhaPlant":
                    tile.GetComponent<Image>().sprite = Towers[5].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[5].GetComponent<TowerOption>().towerSprites);
                    break;
                case "MagikoopaTower":
                    tile.GetComponent<Image>().sprite = Towers[6].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[6].GetComponent<TowerOption>().towerSprites);
                    break;
                case "Bowser":
                    tile.GetComponent<Image>().sprite = Towers[7].GetComponent<TowerOption>().towerSprites[t.towerLevel - 1];
                    tile.GetComponent<MapLocation>().towerSprites.AddRange(Towers[7].GetComponent<TowerOption>().towerSprites);
                    Map.bowserPlaced = true;
                    break;
            }
            tile.GetComponent<MapLocation>().towerType = t.towerType;
            tile.GetComponent<MapLocation>().towerLevel = t.towerLevel;
            tile.GetComponent<MapLocation>().TargetPriority = t.towerFocus;
        }
        Waves.GetComponent<Waves>().TheWaves.Clear();
        if (gamemode != 2) saveObject.waves.RemoveAt(saveObject.waves.Count - 1);
        int round = 0;
        WaveSelector.GetComponent<TMP_Dropdown>().options.Clear();
        foreach (Wave wave in saveObject.waves)
        {
            round += 1;
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = "Round " + Convert.ToString(round);
            WaveSelector.GetComponent<TMP_Dropdown>().options.Add(option);
            Waves.serializableClass newwave = new Waves.serializableClass();
            newwave.music = wave.music;
            newwave.wave = new List<GameObject>();
            foreach (string enemy in wave.enemies)
            {
                switch (enemy)
                {
                    case "Toad":
                        newwave.wave.Add(Waves.GetComponent<Waves>().EndlessToad);
                        break;
                    case "Captain Toad":
                        newwave.wave.Add(Waves.GetComponent<Waves>().EndlessCaptainToad);
                        break;
                    case "Yoshi":
                        newwave.wave.Add(Waves.GetComponent<Waves>().EndlessYoshi);
                        break;
                    case "Luigi":
                        newwave.wave.Add(Waves.GetComponent<Waves>().EndlessLuigi);
                        break;
                    case "Bob-Omb Buddy":
                        newwave.wave.Add(Waves.GetComponent<Waves>().EndlessBobOmbBuddy);
                        break;
                }
            }
            Waves.GetComponent<Waves>().TheWaves.Add(newwave);
        }
        WaveSelector.GetComponent<TMP_Dropdown>().value = 0;
        selectedWave = 0;
        SelectWave();
        if (Waves.GetComponent<Waves>().TheWaves.Count == 99)
        {
            AddWaveButton.GetComponent<CanvasGroup>().alpha = 0.5f;
            AddWaveButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            AddWaveButton.GetComponent<CanvasGroup>().interactable = false;
        }
        Map.WriteToLog("Loaded map \"" + saveObject.name + "\".");
    }
}
