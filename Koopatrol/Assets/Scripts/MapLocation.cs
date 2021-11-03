using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapLocation : MonoBehaviour, IDropHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    static GameObject bulletOriginal;
    static GameObject bulletList;
    public int towerLevel = 0;
    GameObject draggingTower;
    GameObject towerInfo;
    GameObject magicEffect;
    GameObject myMagic;
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
    public int TargetPriority = 0;
    GameObject CooldownCounter;
    GameObject MyCooldown = null;
    GameObject lavaFieldSource;
    GameObject lavaField = null;
    public bool rangeIndicating = false;
    Color originalColor;
    Vector3 originalPosition;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if ((gameObject.tag == "Tower" || gameObject.tag == "PathTower") && wasDragging == false)
            {
                towerInfo.GetComponent<TowerInfo>().ShowInfo(gameObject);
            }
            wasDragging = false;
        }
    }
    public void DestroyTower()
    {
        if (towerType == "Bowser") Map.bowserPlaced = false;
        if (tag != "PathTower" && tag != "Path") transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        transform.position = originalPosition;
        gameObject.GetComponent<Image>().sprite = null;
        gameObject.GetComponent<Image>().color = originalColor;
        towerType = "none";
        cooldown = 0;
        towerLevel = 0;
        towerSprites = new List<Sprite>();
        TargetPriority = 0;
        GetComponent<AudioSource>().clip = null;
        if (gameObject.tag == "PathTower")
        {
            gameObject.tag = "Path";
        }
        else
        {
            gameObject.tag = "Ground";
        }
        gameObject.GetComponent<Image>().sprite = originalImage;
        Destroy(MyCooldown);
        MyCooldown = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && draggingTower.GetComponent<draggingTower>().towerType != "none")
        {
            if (draggingTower.GetComponent<draggingTower>().towerCost > Assets.CoinCounter.GetCoinCount())
            {
                Debug.Log("Not enough money to place tower");
            }
            else if ((gameObject.tag == "Ground") && (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.AnyGround || (draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.GroundNextToPathOnOneAxis && isNextToPath != 0)))
            {
                PlaceTower();
                gameObject.tag = "Tower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost, false);
            }
            else if ((gameObject.tag == "Path") && draggingTower.GetComponent<draggingTower>().validPosition == Assets.ValidPosition.Path)
            {
                PlaceTower();
                gameObject.tag = "PathTower";
                Assets.CoinCounter.ChangeCoinCounter(-draggingTower.GetComponent<draggingTower>().towerCost, false);
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
        if (hasCooldownCounter()) MyCooldown = CooldownCounter.GetComponent<Cooldown>().CreateCooldownCounter(gameObject);
    }
    bool hasCooldownCounter()
    {
        switch (towerType)
        {
            case "FreezieTower":
                return true;
            case "Thwomp":
                return true;
            case "PiranhaPlant":
                return true;
        }
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Map.Tiles.Add(gameObject);
        gameObject.GetComponent<AudioSource>().volume = Convert.ToSingle(Map.SoundVolume) / 100;
        draggingTower = GameObject.FindGameObjectWithTag("DraggingTower");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
        bulletOriginal = GameObject.FindGameObjectWithTag("Bullet");
        bulletList = GameObject.FindGameObjectWithTag("BulletList");
        magicEffect = GameObject.FindGameObjectWithTag("MagicEffect");
        CooldownCounter = GameObject.FindGameObjectWithTag("CooldownCounter");
        lavaFieldSource = GameObject.FindGameObjectWithTag("LavaAttack");
        originalImage = gameObject.GetComponent<Image>().sprite;
        originalColor = gameObject.GetComponent<Image>().color;
        originalPosition = gameObject.transform.position;
        if (gameObject.tag == "Ground" || gameObject.tag == "Tower")
        {
            GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
            GameObject[] obstructedPaths = GameObject.FindGameObjectsWithTag("PathTower");
            GameObject castle = GameObject.FindGameObjectWithTag("Castle");
            List<GameObject> field = new List<GameObject>();
            field.Add(castle);
            foreach (GameObject path in paths)
            {
                field.Add(path);
            }
            foreach (GameObject path in obstructedPaths)
            {
                field.Add(path);
            }
            bool xPath = false;
            bool yPath = false;
            foreach (GameObject path in field)
            {
                if (gameObject.transform.position.x - path.transform.position.x >= 30 && gameObject.transform.position.x - path.transform.position.x <= 70 && gameObject.transform.position.y == path.transform.position.y)
                {
                    xPath = true;
                }
                else if (gameObject.transform.position.x - path.transform.position.x >= -70 && gameObject.transform.position.x - path.transform.position.x <= -30 && gameObject.transform.position.y == path.transform.position.y)
                {
                    xPath = true;
                }
                else if (gameObject.transform.position.y - path.transform.position.y >= 30 && gameObject.transform.position.y - path.transform.position.y <= 70 && gameObject.transform.position.x == path.transform.position.x)
                {
                    yPath = true;
                }
                else if (gameObject.transform.position.y - path.transform.position.y >= -70 && gameObject.transform.position.y - path.transform.position.y <= -30 && gameObject.transform.position.x == path.transform.position.x)
                {
                    yPath = true;
                }
            }
            if (xPath || yPath)
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
        if (towerBuffed && towerType != "MagikoopaTower")
        {
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
        if (towerBuffed) towerLevel -= 1;
        towerBuffed = false;
    }
    public void bulletBlasterRangeVisualization()
    {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        int whileLoop = 0;
        bool hasPath = true;
        while (whileLoop != 2)
        {
            foreach (GameObject spot in Map.Tiles)
            {
                if ((spot.tag == "Path" || spot.tag == "PathTower") && y - spot.transform.position.y >= -45 && y - spot.transform.position.y <= 45 && x - spot.transform.position.x >= -45 && x - spot.transform.position.x <= 45)
                {
                    hasPath = true;
                    spot.GetComponent<Image>().color = new Color(1f, 0.3f, 0.3f);
                    spot.GetComponent<MapLocation>().rangeIndicating = true;
                }
            }
            if (!hasPath)
            {
                x = gameObject.transform.position.x;
                y = gameObject.transform.position.y;
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
        GetComponent<Image>().color = originalColor;
        rangeIndicating = false;
    }
    void LookAt(Vector3 targetPosition)
    {
        Vector3 difference = targetPosition - gameObject.transform.position;
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
            distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
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
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
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
                if ((spot.tag == "Path" || spot.tag == "PathTower") && y - spot.transform.position.y >= -45 && y - spot.transform.position.y <= 45 && x - spot.transform.position.x >= -45 && x - spot.transform.position.x <= 45)
                {
                    hasPath = true;
                }
            }
            if (hasPath)
            {
                foreach (GameObject enemy in Map.Enemies)
                {
                    if (enemy.GetComponent<EnemyBehaviour>().isClone && y - enemy.transform.position.y >= -5 && y - enemy.transform.position.y <= 5 && x - enemy.transform.position.x >= -5 && x - enemy.transform.position.x <= 5)
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
                        else if (TargetPriority == 2 && target.GetComponent<EnemyHealth>().Health > enemy.GetComponent<EnemyHealth>().Health)
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 2 && target.GetComponent<EnemyHealth>().Health == enemy.GetComponent<EnemyHealth>().Health)
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                        else if (TargetPriority == 3 && target.GetComponent<EnemyHealth>().GetDamage() < enemy.GetComponent<EnemyHealth>().GetDamage())
                        {
                            target = enemy;
                            if (whileLoop == 0) enemyDistance = first;
                            else enemyDistance = second;
                        }
                        else if (whileLoop == 1 && enemyDistance > second && TargetPriority == 3 && target.GetComponent<EnemyHealth>().GetDamage() == enemy.GetComponent<EnemyHealth>().GetDamage())
                        {
                            target = enemy;
                            enemyDistance = second;
                        }
                    }
                }
            }
            else
            {
                x = gameObject.transform.position.x;
                y = gameObject.transform.position.y;
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
            distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
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
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target.GetComponent<EnemyHealth>().Health > enemy.GetComponent<EnemyHealth>().Health && TargetPriority == 2)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && target.GetComponent<EnemyHealth>().Health == enemy.GetComponent<EnemyHealth>().Health && TargetPriority == 2)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < range && target.GetComponent<EnemyHealth>().GetDamage() < enemy.GetComponent<EnemyHealth>().GetDamage() && TargetPriority == 3)
            {
                target = enemy;
            }
            else if (enemy.GetComponent<EnemyBehaviour>().isClone && distance < lowestDistance && target.GetComponent<EnemyHealth>().GetDamage() == enemy.GetComponent<EnemyHealth>().GetDamage() && TargetPriority == 3)
            {
                target = enemy;
            }
        }
        return target;
    }
    void CreateBullet(int image, int power, float speed, GameObject homingTarget)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().homingTarget = homingTarget;
        bullet.GetComponent<Assets.Bullet>().power = power;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(homingTarget.transform.position);
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void CreateBullet(int image, int power, float speed, Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().targetPosition = targetPosition;
        bullet.GetComponent<Assets.Bullet>().power = power;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(targetPosition);
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void CreateBullet(int image, float freezeTime, float speed, GameObject homingTarget)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().homingTarget = homingTarget;
        bullet.GetComponent<Assets.Bullet>().freezeAmount = freezeTime;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(homingTarget.transform.position);
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void CreateBullet(int image, float freezeTime, float speed, Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletOriginal);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Image>().sprite = bullet.GetComponent<Assets.Bullet>().bulletSprites[image];
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<Assets.Bullet>().targetPosition = targetPosition;
        bullet.GetComponent<Assets.Bullet>().freezeAmount = freezeTime;
        bullet.GetComponent<Assets.Bullet>().speed = speed;
        bullet.GetComponent<Assets.Bullet>().isClone = true;
        bullet.GetComponent<Assets.Bullet>().LookAt(targetPosition);
        bullet.transform.SetParent(bulletList.transform, true);
    }
    void GoombaTower()
    {
        if (cooldown == 0)
        {
            GameObject enemy = GetEnemy(Assets.GoomaTower.GetRange(towerLevel), TargetPriority);
            if (enemy != null)
            {
                cooldown = Assets.GoomaTower.GetCooldown(towerLevel);
                CreateBullet(Assets.GoomaTower.bulletImage, Assets.GoomaTower.GetDamage(towerLevel), Assets.GoomaTower.GetSpeed(towerLevel), enemy);
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
        if (cooldown > 0 && cooldown <= 1) gameObject.transform.position = new Vector3(gameObject.transform.position.x, Convert.ToSingle(gameObject.transform.position.y + (10*Time.deltaTime)), gameObject.transform.position.z);
        if (cooldown == 0)
        {
            foreach (GameObject enemy in Map.Enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().isClone && Vector3.Distance(enemy.transform.position, gameObject.transform.position) < Assets.Thwomp.GetRange(towerLevel))
                {
                    gameObject.transform.position = originalPosition;
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
                    CreateBullet(Assets.BulletBlaster.bulletImage, Assets.BulletBlaster.GetDamage(towerLevel), Assets.BulletBlaster.GetSpeed(towerLevel), new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
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
                    CreateBullet(Assets.BulletBlaster.bulletImage, Assets.BulletBlaster.GetDamage(towerLevel), Assets.BulletBlaster.GetSpeed(towerLevel), new Vector3(transform.position.x, enemy.transform.position.y, enemy.transform.position.z));
                }
            }
        }
        else
        {
            Debug.Log("Bullet blaster placed on non-valid location; destroying.");
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
            if ((tower.tag == "Tower" || tower.tag == "PathTower") && Vector3.Distance(tower.transform.position, gameObject.transform.position) <= Assets.MagikoopaTower.GetRange(towerLevel))
            {
                tower.GetComponent<MapLocation>().towerBuffed = true;
            }
        }
    }
    void Bowser()
    {
        GameObject enemy = GetEnemy(Assets.Bowser.GetRange(towerLevel), TargetPriority);
        if (enemy != null) LookAt(enemy.transform.position);
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
        //wasDragging = true;
        //map.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }
}
