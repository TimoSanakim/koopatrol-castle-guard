using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static int gameSpeed = 1;
    public static bool paused = true;
    public static System.Random randomizer = new System.Random();
    public static bool bowserPlaced = false;
    public static List<GameObject> Tiles = new List<GameObject>();
    public static List<GameObject> Enemies = new List<GameObject>();
}
