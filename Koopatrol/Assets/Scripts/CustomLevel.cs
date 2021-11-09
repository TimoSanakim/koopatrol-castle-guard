using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomLevel : MonoBehaviour
{
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
    public GameObject CoinCounter;
    public GameObject[] Towers;
    int style = 0;
    int gamemode = 0;
    byte tickPassed = 0;
    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (tickPassed == 1)
        {
            Waves.GetComponent<Waves>().SearchForPaths();
            tickPassed = 2;
        }
        else if (tickPassed == 0) tickPassed = 1;
    }
    public void ChangeStyle()
    {
        style += 1;
        if (style == 3) style = 0;
        GroundOption.GetComponent<Image>().sprite = backgroundTiles[style];
        PathOption.GetComponent<Image>().color = PathColors[style];
        BlockedOption.GetComponent<Image>().sprite = blockedTiles[style];
        BackgroundTile.GetComponent<Image>().sprite = backgroundTiles[style];
        foreach (GameObject tile in Map.Tiles)
        {
            if (tile.tag == "Path") tile.GetComponent<Image>().color = PathColors[style];
            tile.transform.parent.transform.parent.GetChild(0).GetChild(tile.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = backgroundTiles[style];
            if (tile.tag == "BlockedGround") tile.transform.parent.transform.parent.GetChild(0).GetChild(tile.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = blockedTiles[style];
        }
        if (style == 0)
        {
            GameObject.FindGameObjectWithTag("LastResortAttack").GetComponent<Image>().sprite = Towers[8].GetComponent<LastResortAttack>().attackSprites[0];
            Towers[8].GetComponent<Image>().sprite = Towers[8].GetComponent<LastResortAttack>().sprites[0];
            Towers[8].GetComponent<LastResortAttack>().description = "Deals damage equal to 30% of max health to all enemies on the map. Can only be used once!";
            Towers[8].GetComponent<LastResortAttack>().damage = 0;
            Towers[8].GetComponent<LastResortAttack>().damageperc = 30;
            Towers[8].GetComponent<LastResortAttack>().freezetime = 0;
            Towers[8].GetComponent<LastResortAttack>().isTornado = false;
            Background.GetComponent<Image>().sprite = backgrounds[0];
            Background.GetComponent<MovingBackOrForeground>().moveRight = 0.1f;
            Background.GetComponent<MovingBackOrForeground>().moveUp = 0f;
            Foreground.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            Foreground.GetComponent<MovingBackOrForeground>().moveRight = 0f;
            Foreground.GetComponent<MovingBackOrForeground>().moveUp = 0f;
        }
        if (style == 1)
        {
            GameObject.FindGameObjectWithTag("LastResortAttack").GetComponent<Image>().sprite = Towers[8].GetComponent<LastResortAttack>().attackSprites[1];
            Towers[8].GetComponent<Image>().sprite = Towers[8].GetComponent<LastResortAttack>().sprites[1];
            Towers[8].GetComponent<LastResortAttack>().description = "Deals 5 damage to all enemies on the map and freezes them for 20 seconds. Can only be used once!";
            Towers[8].GetComponent<LastResortAttack>().damage = 5;
            Towers[8].GetComponent<LastResortAttack>().damageperc = 0;
            Towers[8].GetComponent<LastResortAttack>().freezetime = 20;
            Towers[8].GetComponent<LastResortAttack>().isTornado = false;
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
            GameObject.FindGameObjectWithTag("LastResortAttack").GetComponent<Image>().sprite = Towers[8].GetComponent<LastResortAttack>().attackSprites[2];
            Towers[8].GetComponent<Image>().sprite = Towers[8].GetComponent<LastResortAttack>().sprites[2];
            Towers[8].GetComponent<LastResortAttack>().description = "Deals 5 damage to all enemies on the map and pushes them back. Can only be used once!";
            Towers[8].GetComponent<LastResortAttack>().damage = 5;
            Towers[8].GetComponent<LastResortAttack>().damageperc = 0;
            Towers[8].GetComponent<LastResortAttack>().freezetime = 0;
            Towers[8].GetComponent<LastResortAttack>().isTornado = true;
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
    public void ChangeGamemode()
    {
        gamemode += 1;
        if (gamemode == 3) gamemode = 0;
        switch (gamemode)
        {
            case 0:
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

    public void LoadLevel()
    {
        string saveString = File.ReadAllText(Application.dataPath + "/customlevel" + Map.LoadedCustomMap);
        levelCreator.SaveObject saveObject = JsonUtility.FromJson<levelCreator.SaveObject>(saveString);
        if (style != saveObject.style) ChangeStyle();
        if (style != saveObject.style) ChangeStyle();
        if (gamemode != saveObject.gamemode) ChangeGamemode();
        if (gamemode != saveObject.gamemode) ChangeGamemode();
        CoinCounter.GetComponent<Text>().text = Convert.ToString(saveObject.coinCount);
        foreach (levelCreator.SaveTile t in saveObject.tiles)
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
        foreach (levelCreator.Wave wave in saveObject.waves)
        {
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
                    case "Mario":
                        newwave.wave.Add(Waves.GetComponent<Waves>().EndlessMario);
                        break;
                }
            }
            Waves.GetComponent<Waves>().TheWaves.Add(newwave);
        }
        Map.WriteToLog("Loaded map \"" + saveObject.name + "\".");
        Waves.GetComponent<Waves>().SpawnEnemies.GetComponent<SpawnEnemies>().stopSpawning = false;
    }
}
