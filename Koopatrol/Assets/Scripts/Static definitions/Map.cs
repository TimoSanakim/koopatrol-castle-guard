using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static int MusicVolume = 100;
    public static int SoundVolume = 100;
    public static int ShowCooldowns = 4;
    public static int gameSpeed = 1;
    public static bool paused = true;
    public static bool Victory = true;
    public static System.Random randomizer = new System.Random();
    public static bool bowserPlaced = false;
    public static List<GameObject> Tiles = new List<GameObject>();
    public static List<GameObject> Enemies = new List<GameObject>();
    public static string LoadedLevel = "MainMenu";
}
