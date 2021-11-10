using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapLocation : MonoBehaviour, IDropHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Sprite[] PathSprites;
    static GameObject bulletOriginal;
    static GameObject bulletList;
    public int towerLevel = 0;
    GameObject draggingTower;
    GameObject towerInfo;
    GameObject magicEffect;
    GameObject myMagic;
    GameObject map;
    public string towerType = "none";
    public List<Sprite> towerSprites;
    //1 = path left or right, and not above or below
    //2 = path up or down, and not to sides
    //0 = any other situation
    public int isNextToPath = 0;
    Sprite originalImage;
    public float cooldown = 0;
    bool wasDragging = false;
    public bool highlight = false;
    int highlightTime = 0;
    public bool towerBuffed = false;
    public bool towerWasBuffed = false;
    public int TargetPriority = 0;
    GameObject CooldownCounter;
    GameObject MyCooldown = null;
    GameObject lavaFieldSource;
    GameObject lavaField = null;
    public bool rangeIndicating = false;
    public Color originalColor;
    Vector3 originalPosition;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if ((gameObject.tag == "Tower" || gameObject.tag == "PathTower") && !wasDragging)
            {
                towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
            }
            else if (!wasDragging)
            {
                towerInfo.GetComponent<TowerInfo>().HideInfo();
                if (gameObject.transform.parent.transform.parent.GetComponent<levelCreator>() != null)
                {
                    if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject) towerInfo.GetComponent<TowerInfo>().HideInfo();
                    GameObject selectedOption = gameObject.transform.parent.transform.parent.GetComponent<levelCreator>().SelectedOption;
                    GameObject backgroundTile = gameObject.transform.parent.transform.parent.GetComponent<levelCreator>().BackgroundTile;
                    if (selectedOption.name == "PlaceGround")
                    {
                        DestroyTower();
                        gameObject.transform.parent.transform.parent.GetChild(0).GetChild(gameObject.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = selectedOption.GetComponent<Image>().sprite;
                        gameObject.GetComponent<Image>().sprite = null;
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        gameObject.tag = "Ground";
                        RecalculateNeighbors();
                        Recalculate();
                    }
                    else if (selectedOption.name == "PlaceObstruction")
                    {
                        DestroyTower();
                        gameObject.transform.parent.transform.parent.GetChild(0).GetChild(gameObject.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = selectedOption.GetComponent<Image>().sprite;
                        gameObject.GetComponent<Image>().sprite = null;
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        gameObject.tag = "BlockedGround";
                        RecalculateNeighbors();
                        Recalculate();
                    }
                    else if (selectedOption.name == "PlacePath")
                    {
                        DestroyTower();
                        gameObject.transform.parent.transform.parent.GetChild(0).GetChild(gameObject.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = backgroundTile.GetComponent<Image>().sprite;
                        gameObject.GetComponent<Image>().sprite = selectedOption.GetComponent<Image>().sprite;
                        gameObject.GetComponent<Image>().color = selectedOption.GetComponent<Image>().color;
                        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        gameObject.tag = "Path";
                        RecalculateNeighbors();
                        Recalculate();
                    }
                    else if (selectedOption.name == "PlaceCastle")
                    {
                        DestroyTower();
                        gameObject.transform.parent.transform.parent.GetChild(0).GetChild(gameObject.transform.GetSiblingIndex()).gameObject.GetComponent<Image>().sprite = backgroundTile.GetComponent<Image>().sprite;
                        gameObject.GetComponent<Image>().sprite = selectedOption.GetComponent<Image>().sprite;
                        gameObject.GetComponent<Image>().color = selectedOption.GetComponent<Image>().color;
                        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        gameObject.tag = "Castle";
                        RecalculateNeighbors();
                        Recalculate();
                    }
                }
            }
            wasDragging = false;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (gameObject.transform.parent.transform.parent.GetComponent<levelCreator>() != null)
            {
                if (gameObject.transform.parent.transform.childCount != 2)
                {
                    Map.Tiles.Remove(gameObject);
                    gameObject.tag = "Finish";
                    if (map.GetComponent<levelCreator>().Waves.GetComponent<Waves>().StartingPositions.Contains(gameObject)) map.GetComponent<levelCreator>().Waves.GetComponent<Waves>().StartingPositions.Remove(gameObject);
                    if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
                    {
                        towerInfo.GetComponent<TowerInfo>().HideInfo();
                        towerInfo.GetComponent<TowerInfo>().selectedTower = null;
                    }
                    Destroy(gameObject.transform.parent.transform.parent.GetChild(0).GetChild(gameObject.transform.GetSiblingIndex()).gameObject);
                    RecalculateNeighbors();
                    if (Map.Tiles.Contains(gameObject)) Map.Tiles.Remove(gameObject);
                    Destroy(gameObject);
                }
                else
                {
                    Map.WriteToLog("You may not delete the only tile in the map.");
                }
            }
        }
    }
    public void RecalculateNeighbors()
    {
        List<GameObject> tiles = new List<GameObject>();
        tiles.AddRange(Map.Tiles);
        foreach (GameObject t in tiles)
        {
            if (t != gameObject)
            {
                if (t.transform.localPosition.x - transform.localPosition.x > -60 && t.transform.localPosition.x - transform.localPosition.x < 60 && t.transform.localPosition.y - transform.localPosition.y > -10 && t.transform.localPosition.y - transform.localPosition.y < 10)
                {
                    Map.Tiles.Remove(t);
                    t.GetComponent<MapLocation>().Start();
                }
                else if (t.transform.localPosition.x - transform.localPosition.x > -10 && t.transform.localPosition.x - transform.localPosition.x < 10 && t.transform.localPosition.y - transform.localPosition.y > -60 && t.transform.localPosition.y - transform.localPosition.y < 60)
                {
                    Map.Tiles.Remove(t);
                    t.GetComponent<MapLocation>().Start();
                }
            }
        }
    }
    public void Recalculate()
    {
        if (Map.Tiles.Contains(gameObject)) Map.Tiles.Remove(gameObject);
        Start();
    }
    public void DestroyTower()
    {
        if (towerType == "Bowser") Map.bowserPlaced = false;
        if (tag != "PathTower" && tag != "Path") transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        transform.localPosition = originalPosition;
        gameObject.GetComponent<Image>().sprite = originalImage;
        gameObject.GetComponent<Image>().color = originalColor;
        towerType = "none";
        cooldown = 0;
        towerLevel = 0;
        towerSprites = new List<Sprite>();
        TargetPriority = Map.DefaultTargetPriority;
        GetComponent<AudioSource>().clip = null;
        if (gameObject.tag == "PathTower")
        {
            gameObject.tag = "Path";
        }
        else
        {
            gameObject.tag = "Ground";
        }
        Destroy(MyCooldown);
        MyCooldown = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && draggingTower.GetComponent<draggingTower>().towerType != "none")
        {
            if (draggingTower.GetComponent<draggingTower>().towerCost > Assets.CoinCounter.GetCoinCount())
            {
                Map.WriteToLog("Not enough money to place tower");
            }
            else if ((gameObject.tag == "Ground") && (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.AnyGround || (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath != 0)))
            {
                PlaceTower();
                gameObject.tag = "Tower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost, false);
                towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
            }
            else if ((gameObject.tag == "Path") && draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.Path)
            {
                PlaceTower();
                gameObject.tag = "PathTower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost, false);
                towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
            }
            else if ((gameObject == Tutorial.TutorialPosition) && draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.TutorialPosition)
            {
                PlaceTower();
                if (gameObject.tag == "Path") gameObject.tag = "PathTower";
                else gameObject.tag = "Tower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost, false);
                towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
            }
        }
        draggingTower.GetComponent<draggingTower>().towerType = "none";
    }
    void PlaceTower()
    {
        gameObject.GetComponent<Image>().sprite = draggingTower.GetComponent<Image>().sprite;
        gameObject.GetComponent<Image>().color = draggingTower.GetComponent<Image>().color;
        towerType = draggingTower.GetComponent<draggingTower>().towerType;
        towerSprites = draggingTower.GetComponent<draggingTower>().towerSprites;
        GetComponent<AudioSource>().clip = draggingTower.GetComponent<draggingTower>().towerSound;
        towerLevel = 1;
        cooldown = 1;
        if (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath == 2) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        if (towerType == "Bowser") Map.bowserPlaced = true;
        if (map.GetComponent<levelCreator>() == null) CreateCooldown();
    }
    public void CreateCooldown()
    {
        if (CooldownCounter == null) CooldownCounter = GameObject.FindGameObjectWithTag("CooldownCounter");
        if (hasCooldownCounter()) MyCooldown = CooldownCounter.GetComponent<Cooldown>().CreateCooldownCounter(gameObject);
    }
    bool hasCooldownCounter()
    {
        switch (towerType)
        {
            case "GoombaTower":
                if (Assets.GoombaTower.GetCooldown(towerLevel) >= Map.ShowCooldowns) return true;
                break;
            case "KoopaTower":
                if (Assets.KoopaTower.GetCooldown(towerLevel) >= Map.ShowCooldowns) return true;
                break;
            case "FreezieTower":
                if (Assets.FreezieTower.GetCooldown(towerLevel) >= Map.ShowCooldowns) return true;
                break;
            case "Thwomp":
                if (Assets.Thwomp.GetCooldown(towerLevel) >= Map.ShowCooldowns) return true;
                break;
            case "BulletBlaster":
                if (Assets.BulletBlaster.GetCooldown(towerLevel) >= Map.ShowCooldowns) return true;
                break;
            case "PiranhaPlant":
                if (Assets.PiranhaPlant.GetCooldown(towerLevel) >= Map.ShowCooldowns) return true;
                break;
            case "Bowser":
                if (Assets.Bowser.GetCooldown(towerLevel) >= Map.ShowCooldowns) return true;
                break;
        }
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        TargetPriority = Map.DefaultTargetPriority;
        if (!Map.Tiles.Contains(gameObject)) Map.Tiles.Add(gameObject);
        gameObject.GetComponent<AudioSource>().volume = Convert.ToSingle(Map.SoundVolume) / 100;
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        bulletOriginal = GameObject.FindGameObjectWithTag("Bullet");
        bulletList = GameObject.FindGameObjectWithTag("BulletList");
        magicEffect = GameObject.FindGameObjectWithTag("MagicEffect");
        CooldownCounter = GameObject.FindGameObjectWithTag("CooldownCounter");
        lavaFieldSource = GameObject.FindGameObjectWithTag("LavaAttack");
        if (tag != "Tower" && tag != "PathTower") originalImage = gameObject.GetComponent<Image>().sprite;
        if (tag != "Tower" && tag != "PathTower" && !rangeIndicating) originalColor = gameObject.GetComponent<Image>().color;
        originalPosition = gameObject.transform.localPosition;
        map = GameObject.FindGameObjectWithTag("Map");
        isNextToPath = 0;
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        GameObject[] obstructedPaths = GameObject.FindGameObjectsWithTag("PathTower");
        GameObject[] castles = GameObject.FindGameObjectsWithTag("Castle");
        List<GameObject> field = new List<GameObject>();
        foreach (GameObject castle in castles)
        {
            field.Add(castle);
        }
        foreach (GameObject path in paths)
        {
            field.Add(path);
        }
        foreach (GameObject path in obstructedPaths)
        {
            field.Add(path);
        }
        bool xPath = false;
        bool xppath = false;
        bool xnpath = false;
        bool yPath = false;
        bool yppath = false;
        bool ynpath = false;
        byte connectedpaths = 0;
        foreach (GameObject path in field)
        {
            if (gameObject.transform.localPosition.x - path.transform.localPosition.x >= 30 && gameObject.transform.localPosition.x - path.transform.localPosition.x <= 70 && gameObject.transform.localPosition.y == path.transform.localPosition.y)
            {
                xPath = true;
                xppath = true;
                connectedpaths += 1;
            }
            else if (gameObject.transform.localPosition.x - path.transform.localPosition.x >= -70 && gameObject.transform.localPosition.x - path.transform.localPosition.x <= -30 && gameObject.transform.localPosition.y == path.transform.localPosition.y)
            {
                xPath = true;
                xnpath = true;
                connectedpaths += 1;
            }
            else if (gameObject.transform.localPosition.y - path.transform.localPosition.y >= 30 && gameObject.transform.localPosition.y - path.transform.localPosition.y <= 70 && gameObject.transform.localPosition.x == path.transform.localPosition.x)
            {
                yPath = true;
                yppath = true;
                connectedpaths += 1;
            }
            else if (gameObject.transform.localPosition.y - path.transform.localPosition.y >= -70 && gameObject.transform.localPosition.y - path.transform.localPosition.y <= -30 && gameObject.transform.localPosition.x == path.transform.localPosition.x)
            {
                yPath = true;
                ynpath = true;
                connectedpaths += 1;
            }
        }
        if ((tag=="Ground" || tag =="Tower") && (xPath || yPath))
        {
            if (xPath && !yPath)
            {
                isNextToPath = 1;
            }
            else if (yPath && !xPath)
            {
                isNextToPath = 2;
            }
        }
        if (tag == "Path" || tag == "PathTower") 
        {
            if (map.GetComponent<levelCreator>() != null)
            {
                if (map.GetComponent<levelCreator>().Waves.GetComponent<Waves>().StartingPositions.Contains(gameObject)) map.GetComponent<levelCreator>().Waves.GetComponent<Waves>().StartingPositions.Remove(gameObject);
            }
            else if (map.GetComponent<CustomLevel>() != null)
            {
                if (map.GetComponent<CustomLevel>().Waves.GetComponent<Waves>().StartingPositions.Contains(gameObject)) map.GetComponent<levelCreator>().Waves.GetComponent<Waves>().StartingPositions.Remove(gameObject);
            }
            if (connectedpaths == 1)
            {
                originalImage = PathSprites[0];
                if (xppath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, -180.0f);
                else if (xnpath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                else if (yppath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                else if (ynpath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                if (map.GetComponent<levelCreator>() != null)
                {
                    map.GetComponent<levelCreator>().Waves.GetComponent<Waves>().StartingPositions.Add(gameObject);
                }
                else if (map.GetComponent<CustomLevel>() != null)
                {
                    map.GetComponent<CustomLevel>().Waves.GetComponent<Waves>().StartingPositions.Add(gameObject);
                }
            }
            if (connectedpaths == 2)
            {
                if ((xppath && xnpath) || (yppath && ynpath))
                {
                    originalImage = PathSprites[1];
                    if (xppath || xnpath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    else transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                }
                else
                {
                    originalImage = PathSprites[2];
                    if (xppath && yppath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                    else if (xnpath && yppath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, -180.0f);
                    else if (xnpath && ynpath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                    else if (xppath && ynpath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
            }
            if (connectedpaths == 3)
            {
                originalImage = PathSprites[3];
                if (!xppath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                else if (!xnpath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                else if (!yppath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                else if (!ynpath) transform.rotation = Quaternion.Euler(0.0f, 0.0f, -180.0f);
            }
            if (connectedpaths == 4)
            {
                originalImage = PathSprites[4];
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            if (tag == "Path") gameObject.GetComponent<Image>().sprite = originalImage;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (highlight)
        {
            if (highlightTime == 0) highlightTime = 120;
            if (highlightTime >= 91) gameObject.GetComponent<Image>().color = new Color(Convert.ToSingle(gameObject.GetComponent<Image>().color.r - 0.03), Convert.ToSingle(gameObject.GetComponent<Image>().color.g), Convert.ToSingle(gameObject.GetComponent<Image>().color.b - 0.03));
            if (highlightTime >= 61 && highlightTime <= 90) gameObject.GetComponent<Image>().color = new Color(Convert.ToSingle(gameObject.GetComponent<Image>().color.r + 0.03), Convert.ToSingle(gameObject.GetComponent<Image>().color.g), Convert.ToSingle(gameObject.GetComponent<Image>().color.b + 0.03));
            if (highlightTime >= 31 && highlightTime <= 60) gameObject.GetComponent<Image>().color = new Color(Convert.ToSingle(gameObject.GetComponent<Image>().color.r - 0.03), Convert.ToSingle(gameObject.GetComponent<Image>().color.g), Convert.ToSingle(gameObject.GetComponent<Image>().color.b - 0.03));
            if (highlightTime >= 1 && highlightTime <= 30) gameObject.GetComponent<Image>().color = new Color(Convert.ToSingle(gameObject.GetComponent<Image>().color.r + 0.03), Convert.ToSingle(gameObject.GetComponent<Image>().color.g), Convert.ToSingle(gameObject.GetComponent<Image>().color.b + 0.03));
            highlightTime -= 1;
            if (highlightTime == 0)
            {
                highlight = false;
                if (gameObject.tag == "Path") gameObject.GetComponent<Image>().color = originalColor;
                else gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            }
        }
        if (!towerBuffed && towerWasBuffed) towerWasBuffed = false;
        if (towerBuffed && towerType != "MagikoopaTower")
        {
            towerWasBuffed = true;
            towerLevel += 1;
            if (myMagic != null) myMagic.GetComponent<MagicEffect>().killNextTime = false;
            else myMagic = magicEffect.GetComponent<MagicEffect>().CreateMagicEffect(gameObject);
        }
        if (cooldown != 0) cooldown -= 1 * Time.deltaTime;
        if (cooldown < 0) cooldown = 0;
        switch (towerType)
        {
            case "GoombaTower":
                GoombaTower();
                break;
            case "KoopaTower":
                KoopaTower();
                break;
            case "FreezieTower":
                FreezieTower();
                break;
            case "Thwomp":
                Thwomp();
                break;
            case "BulletBlaster":
                BulletBlaster();
                break;
            case "PiranhaPlant":
                PiranhaPlant();
                break;
            case "MagikoopaTower":
                MagikoopaTower();
                break;
            case "Bowser":
                Bowser();
                break;
        }
        if (towerBuffed && towerType != "MagikoopaTower") towerLevel -= 1;
        towerBuffed = false;
    }
    public void bulletBlasterRangeVisualization()
    {
        float x = gameObject.transform.localPosition.x;
        float y = gameObject.transform.localPosition.y;
        int whileLoop = 0;
        bool hasPath = true;
        while (whileLoop != 2)
        {
            foreach (GameObject spot in Map.Tiles)
            {
                if ((spot.tag == "Path" || spot.tag == "PathTower") && y - spot.transform.localPosition.y >= -45 && y - spot.transform.localPosition.y <= 45 && x - spot.transform.localPosition.x >= -45 && x - spot.transform.localPosition.x <= 45)
                {
                    hasPath = true;
                    spot.GetComponent<Image>().color = new Color(1f, 0.3f, 0.3f);
                    spot.GetComponent<MapLocation>().rangeIndicating = true;
                }
            }
            if (!hasPath)
            {
                x = gameObject.transform.localPosition.x;
                y = gameObject.transform.localPosition.y;
                whileLoop += 1;
            }
            if (isNextToPath == 1)
            {
                if (whileLoop == 0) x -= 10;
                else x += 10;
            }
            else if (isNextToPath == 2)
            {
                if (whileLoop == 0) y += 10;
                else y -= 10;
            }
            hasPath = false;
        }
    }
    public void RemoveRangeIndication()
    {
        if (tag=="PathTower" || tag == "Tower" || tag == "Castle") GetComponent<Image>().color = new Color(1, 1, 1, 1);
        else GetComponent<Image>().color = originalColor;
        rangeIndicating = false;
    }
    void LookAt(Vector3 targetPosition)
    {
        Vector3 difference = targetPosition - gameObject.transform.localPosition;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
    //At me
    GameObject GetEnemy()
    {
        GameObject target = null;
        float lowestDistance = 5;
        float distance;
        foreach (GameObject enemy in Map.Enemies)
        {
            distance = Vector3.Distance(enemy.transform.localPosition, gameObject.transform.localPosition);
            if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance)
            {
                target = enemy;
                lowestDistance = distance;
            }
        }
        return target;
    }
    //Get based on isNextToPath
    GameObject GetEnemy(int TargetPriority)
    {
        float x = gameObject.transform.localPosition.x;
        float y = gameObject.transform.localPosition.y;
        int first = 0;
        int second = 0;
        int whileLoop = 0;
        int enemyDistance = 1000000;
        bool hasPath = true;
        GameObject target = null;
        while (whileLoop != 2)
        {
            foreach (GameObject spot in Map.Tiles)
            {
                if ((spot.tag == "Path" || spot.tag == "PathTower") && y - spot.transform.localPosition.y >= -45 && y - spot.transform.localPosition.y <= 45 && x - spot.transform.localPosition.x >= -45 && x - spot.transform.localPosition.x <= 45)
                {
                    hasPath = true;
                }
            }
            if (hasPath)
            {
                foreach (GameObject enemy in Map.Enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone && y - enemy.transform.localPosition.y >= -5 && y - enemy.transform.localPosition.y <= 5 && x - enemy.transform.localPosition.x >= -5 && x - enemy.transform.localPosition.x <= 5)
                    {
                        if (target == null)
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 0)
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                        else if (TargetPriority == 1 && target.GetComponent<EnemyBehaviour>().Paths.Count > enemy.GetComponent<EnemyBehaviour>().Paths.Count)
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 1 && target.GetComponent<EnemyBehaviour>().Paths.Count == enemy.GetComponent<EnemyBehaviour>().Paths.Count)
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                        else if (TargetPriority == 2 && target.GetComponent<EnemyBehaviour>().Paths.Count < enemy.GetComponent<EnemyBehaviour>().Paths.Count)
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 2 && target.GetComponent<EnemyBehaviour>().Paths.Count == enemy.GetComponent<EnemyBehaviour>().Paths.Count)
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                        else if (TargetPriority == 3 && target.GetComponent<EnemyHealth>().Health > enemy.GetComponent<EnemyHealth>().Health)
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 3 && target.GetComponent<EnemyHealth>().Health == enemy.GetComponent<EnemyHealth>().Health)
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                        else if (TargetPriority == 4 && target.GetComponent<EnemyHealth>().Health < enemy.GetComponent<EnemyHealth>().Health)
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 4 && target.GetComponent<EnemyHealth>().Health == enemy.GetComponent<EnemyHealth>().Health)
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                        else if (TargetPriority == 5 && target.GetComponent<EnemyHealth>().GetDamage() < enemy.GetComponent<EnemyHealth>().GetDamage())
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 5 && target.GetComponent<EnemyHealth>().GetDamage() == enemy.GetComponent<EnemyHealth>().GetDamage())
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                    }
                }
            }
            else
            {
                x = gameObject.transform.localPosition.x;
                y = gameObject.transform.localPosition.y;
                whileLoop += 1;
            }
            if (isNextToPath == 1)
            {
                if (whileLoop == 0) x -= 10;
                else x += 10;
            }
            else if (isNextToPath == 2)
            {
                if (whileLoop == 0) y += 10;
                else y -= 10;
            }
            if (whileLoop == 0) first += 1;
            else second += 1;
            hasPath = false;
        }
        return target;
    }
    //Circular range
    GameObject GetEnemy(float range, int TargetPriority)
    {
        GameObject target = null;
        float distance;
        float lowestDistance = range;
        foreach (GameObject enemy in Map.Enemies)
        {
            distance = Vector3.Distance(enemy.transform.localPosition, gameObject.transform.localPosition);
            if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target == null)
            {
                target = enemy;
                if (TargetPriority == 0) lowestDistance = distance;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && TargetPriority == 0)
            {
                target = enemy;
                lowestDistance = distance;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target.GetComponent<EnemyBehaviour>().Paths.Count > enemy.GetComponent<EnemyBehaviour>().Paths.Count && TargetPriority == 1)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && target.GetComponent<EnemyBehaviour>().Paths.Count == enemy.GetComponent<EnemyBehaviour>().Paths.Count && TargetPriority == 1)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target.GetComponent<EnemyBehaviour>().Paths.Count < enemy.GetComponent<EnemyBehaviour>().Paths.Count && TargetPriority == 2)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && target.GetComponent<EnemyBehaviour>().Paths.Count == enemy.GetComponent<EnemyBehaviour>().Paths.Count && TargetPriority == 2)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target.GetComponent<EnemyHealth>().Health > enemy.GetComponent<EnemyHealth>().Health && TargetPriority == 3)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && target.GetComponent<EnemyHealth>().Health == enemy.GetComponent<EnemyHealth>().Health && TargetPriority == 3)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target.GetComponent<EnemyHealth>().Health < enemy.GetComponent<EnemyHealth>().Health && TargetPriority == 4)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && target.GetComponent<EnemyHealth>().Health == enemy.GetComponent<EnemyHealth>().Health && TargetPriority == 4)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target.GetComponent<EnemyHealth>().GetDamage() < enemy.GetComponent<EnemyHealth>().GetDamage() && TargetPriority == 5)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && target.GetComponent<EnemyHealth>().GetDamage() == enemy.GetComponent<EnemyHealth>().GetDamage() && TargetPriority == 5)
            {
                target = enemy;
            }
        }
        return target;
    }
    void CreateBullet(int image, int power, float speed, GameObject homingTarget)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.SetParent(bulletList.transform, true);
        bullet.transform.localPosition = gameObject.transform.localPosition;
        bullet.transform.localScale = new Vector3(1,1,1);
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().homingTarget = homingTarget;
        bullet.GetComponent<Assets.Bullet>().power = power;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(homingTarget.transform.localPosition);
    }
    void CreateBullet(int image, int power, float speed, Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.SetParent(bulletList.transform, true);
        bullet.transform.localPosition = gameObject.transform.localPosition;
        bullet.transform.localScale = new Vector3(1,1,1);
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().targetPosition = targetPosition;
        bullet.GetComponent<Assets.Bullet>().power = power;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(targetPosition);
    }
    void CreateBullet(int image, float freezeTime, float speed, GameObject homingTarget)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.SetParent(bulletList.transform, true);
        bullet.transform.localPosition = gameObject.transform.localPosition;
        bullet.transform.localScale = new Vector3(1,1,1);
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().homingTarget = homingTarget;
        bullet.GetComponent<Assets.Bullet>().freezeAmount = freezeTime;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(homingTarget.transform.localPosition);
    }
    void CreateBullet(int image, float freezeTime, float speed, Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.SetParent(bulletList.transform, true);
        bullet.transform.localPosition = gameObject.transform.localPosition;
        bullet.transform.localScale = new Vector3(1,1,1);
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().targetPosition = targetPosition;
        bullet.GetComponent<Assets.Bullet>().freezeAmount = freezeTime;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(targetPosition);
    }
    void GoombaTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.GoombaTower.GetRange(towerLevel), TargetPriority);
            if (enemy != null)
            {
                cooldown = Assets.GoombaTower.GetCooldown(towerLevel);
                CreateBullet(Assets.GoombaTower.bulletImage, Assets.GoombaTower.GetDamage(towerLevel), Assets.GoombaTower.GetSpeed(towerLevel), enemy);
            }
        }
    }
    void KoopaTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.KoopaTower.GetRange(towerLevel), TargetPriority);
            if (enemy != null)
            {
                cooldown = Assets.KoopaTower.GetCooldown(towerLevel);
                CreateBullet(Assets.KoopaTower.bulletImage, Assets.KoopaTower.GetDamage(towerLevel), Assets.KoopaTower.GetSpeed(towerLevel), enemy);
            }
        }
    }
    void FreezieTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.FreezieTower.GetRange(towerLevel), TargetPriority);
            if (enemy != null)
            {
                cooldown = Assets.FreezieTower.GetCooldown(towerLevel);
                CreateBullet(Assets.FreezieTower.bulletImage, Assets.FreezieTower.GetFreezeTime(towerLevel), Assets.FreezieTower.GetSpeed(towerLevel), enemy);
            }
        }
    }
    void Thwomp()
    {
        if (cooldown > 0 && cooldown <= 1) gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, Convert.ToSingle(gameObject.transform.localPosition.y + (10*Time.deltaTime)), gameObject.transform.localPosition.z);
        if (cooldown == 0)
        {
            foreach (GameObject enemy in Map.Enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().isClone && Vector3.Distance(enemy.transform.localPosition, gameObject.transform.localPosition) < Assets.Thwomp.GetRange(towerLevel))
                {
                    gameObject.transform.localPosition = originalPosition;
                    if (cooldown == 0) GetComponent<AudioSource>().Play();
                    cooldown = Assets.Thwomp.GetCooldown(towerLevel);
                    enemy.GetComponent<EnemyBehaviour>().Stagger(Assets.Thwomp.GetStaggerTime(towerLevel), true);
                    enemy.GetComponent<EnemyBehaviour>().Freeze(-1, false);
                }
            }
        }
    }
    void BulletBlaster()
    {
        if (isNextToPath == 1)
        {
            if (cooldown == 0)
            {
                GameObject enemy = GetEnemy(TargetPriority);
                if (enemy != null)
                {
                    cooldown = Assets.BulletBlaster.GetCooldown(towerLevel);
                    CreateBullet(Assets.BulletBlaster.bulletImage, Assets.BulletBlaster.GetDamage(towerLevel), Assets.BulletBlaster.GetSpeed(towerLevel), new Vector3(enemy.transform.localPosition.x, transform.localPosition.y, enemy.transform.localPosition.z));
                }
            }
        }
        else if (isNextToPath == 2)
        {
            if (cooldown == 0)
            {
                GameObject enemy = GetEnemy(TargetPriority);
                if (enemy != null)
                {
                    cooldown = Assets.BulletBlaster.GetCooldown(towerLevel);
                    CreateBullet(Assets.BulletBlaster.bulletImage, Assets.BulletBlaster.GetDamage(towerLevel), Assets.BulletBlaster.GetSpeed(towerLevel), new Vector3(transform.localPosition.x, enemy.transform.localPosition.y, enemy.transform.localPosition.z));
                }
            }
        }
        else
        {
            Map.WriteToLog("Bullet blaster placed on non-valid location; destroying.");
            if (towerInfo.GetComponent<TowerInfo>().selectedTower == gameObject)
            {
                towerInfo.GetComponent<TowerInfo>().HideInfo();
                towerInfo.GetComponent<TowerInfo>().selectedTower = null;
            }
            DestroyTower();
        }
    }
    void PiranhaPlant()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy();
            if (enemy != null)
            {
                GetComponent<AudioSource>().Play();
                cooldown = Assets.PiranhaPlant.GetCooldown(towerLevel);
                enemy.GetComponent<EnemyBehaviour>().Stagger(Assets.PiranhaPlant.GetStaggerTime(towerLevel), true);
                enemy.GetComponent<EnemyBehaviour>().Freeze(-1, false);
                enemy.GetComponent<EnemyHealth>().Hurt(Assets.PiranhaPlant.GetDamage(towerLevel));
            }
        }
    }
    void MagikoopaTower()
    {
        foreach (GameObject tower in Map.Tiles)
        {
            if ((tower.tag == "Tower" || tower.tag == "PathTower") && Vector3.Distance(tower.transform.localPosition, gameObject.transform.localPosition) <= Assets.MagikoopaTower.GetRange(towerLevel))
            {
                tower.GetComponent<MapLocation>().towerBuffed = true;
            }
        }
    }
    void Bowser()
    {
        GameObject enemy = GetEnemy(Assets.Bowser.GetRange(towerLevel), TargetPriority);
        if (enemy != null) LookAt(enemy.transform.localPosition);
        if (cooldown == 0)
        {
            if (enemy != null)
            {
                cooldown = Assets.Bowser.GetCooldown(towerLevel);
                CreateBullet(Assets.Bowser.bulletImage, Assets.Bowser.GetDamage(towerLevel), Assets.Bowser.GetSpeed(towerLevel), enemy);
            }
        }
        if (towerLevel >= 4)
        {
            if (lavaField != null) lavaField.GetComponent<LavaField>().killNextTime = false;
            else lavaField = lavaFieldSource.GetComponent<LavaField>().CreateLavaField(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (map.GetComponent<levelCreator>() != null || map.GetComponent<CustomLevel>() != null)
        {
            wasDragging = true;
            map.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        }
    }
}
