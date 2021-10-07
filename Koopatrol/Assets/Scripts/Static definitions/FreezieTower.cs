using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class FreezieTower
    {
        public static int bulletImage = 2;
        public static float GetRange(int towerLevel)
        {
            return 100;
        }
        public static float GetCooldown(int towerLevel)
        {
            return 7;
        }
        public static float GetFreezeTime(int towerLevel)
        {
            if (towerLevel <= 2) return 4;
            if (towerLevel >= 3) return 6;
            else return 0;
        }
        public static float GetSpeed(int towerLevel)
        {
            if (towerLevel == 1) return 25;
            if (towerLevel >= 2) return 50;
            else return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 30;
            if (towerLevel == 2) return 50;
            else return 0;
        }
    }
}