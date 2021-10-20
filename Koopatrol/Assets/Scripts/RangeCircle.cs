using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCircle : MonoBehaviour
{
    bool isClone = false;
    public bool killNextTime = false;
    GameObject map;
    GameObject towerInfo;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        towerInfo = GameObject.FindGameObjectWithTag("TowerInfo");
    }

    // Update is called once per frame
    void Update()
    {
       gameObject.transform.Rotate(0, 0, 0.07f, Space.Self);
        if (isClone)
        {
            if (killNextTime) Destroy(gameObject);
            else 
            {
                killNextTime = true;
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(2*GetRangeTower(), 2*GetRangeTower());
            }
        }
        else gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(2*GetRangeDraggedTower(), 2*GetRangeDraggedTower());
    }
float GetRangeTower()
    {
        if (towerInfo.GetComponent<TowerInfo>().selectedTower.transform.childCount == 1)
        {
            switch (towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerType)
            {
                case "GoombaTower":
                    return Assets.GoomaTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel + 1);
                case "KoopaTower":
                    return Assets.KoopaTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel + 1);
                case "FreezieTower":
                    return Assets.FreezieTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel + 1);
                case "Thwomp":
                    return Assets.Thwomp.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel + 1);
                case "MagikoopaTower":
                    return Assets.MagikoopaTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel + 1);
                case "Bowser":
                    return Assets.Bowser.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel + 1);
            }
        }
        switch (towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerType)
        {
            case "GoombaTower":
                return Assets.GoomaTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel);
            case "KoopaTower":
                return Assets.KoopaTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel);
            case "FreezieTower":
                return Assets.FreezieTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Thwomp":
                return Assets.Thwomp.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel);
            case "MagikoopaTower":
                return Assets.MagikoopaTower.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel);
            case "Bowser":
                return Assets.Bowser.GetRange(towerInfo.GetComponent<TowerInfo>().selectedTower.GetComponent<MapLocation>().towerLevel);
        }
        return 0;
    }
float GetRangeDraggedTower()
    {
        switch (gameObject.GetComponentInParent<draggingTower>().towerType)
        {
            case "GoombaTower":
                return Assets.GoomaTower.GetRange(1);
            case "KoopaTower":
                return Assets.KoopaTower.GetRange(1);
            case "FreezieTower":
                return Assets.FreezieTower.GetRange(1);
            case "Thwomp":
                return Assets.Thwomp.GetRange(1);
            case "MagikoopaTower":
                return Assets.MagikoopaTower.GetRange(1);
            case "Bowser":
                return Assets.Bowser.GetRange(1);
        }
        return 0;
    }

    public GameObject CreateRangeCircle(GameObject parent)
    {
        GameObject RangeCircle = Instantiate(gameObject);
        RangeCircle.transform.position = parent.transform.position;
        RangeCircle.transform.SetParent(map.transform, true);
        RangeCircle.GetComponent<RangeCircle>().isClone = true;
        RangeCircle.GetComponent<CanvasGroup>().alpha = 1f;
        RangeCircle.tag = "RangeCircle";
        return RangeCircle;
    }
}
