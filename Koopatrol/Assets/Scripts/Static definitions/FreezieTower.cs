using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class FreezieTower
    {
        public static int bulletImage = 2;
        public static int GetSellCost(int towerLevel)
        {
            if (towerLevel == 1) return 25;
            if (towerLevel == 2) return 30;
            if (towerLevel == 3) return 40;
            return 0;
        }
        public static string GetDescription(int towerLevel)
        {
            if (towerLevel == 1) return "Freezie tower. A slow shooting, non-damaging tower. Instead it freezes the enemy hit in place for a little while. Can be upgraded to increase the Freezies movement speed and once more to increase the freeze time.";
            if (towerLevel == 2) return "Freezie tower+1. A slow shooting, non-damaging tower. Instead it freezes the enemy hit in place for a little while. Can be upgraded once more to increase the freeze time.";
            return "Freezie tower+2. A slow shooting, non-damaging tower. Instead it freezes the enemy hit in place for a little while.";
        }
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
            return 0;
        }
        public static float GetSpeed(int towerLevel)
        {
            if (towerLevel == 1) return 25;
            if (towerLevel >= 2) return 50;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 30;
            if (towerLevel == 2) return 50;
            return 0;
        }
    }
}