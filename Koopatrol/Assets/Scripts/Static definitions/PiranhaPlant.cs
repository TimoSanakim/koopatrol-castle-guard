using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class PiranhaPlant
    {
        public static int GetSellCost(int towerLevel)
        {
            return 50;
        }
        public static string GetDescription(int towerLevel)
        {
            return "Piranha plant. A pipe which can be placed on paths, when an enemy walks on top, a Piranha Plant will grab and hold that enemy. When grabbed, breaks ice. Can be enhanced with magic to increase hold time.";
        }
        public static float GetCooldown(int towerLevel)
        {
            return 8;
        }
        public static int GetDamage(int towerLevel)
        {
            return 2;
        }
        public static float GetStaggerTime(int towerLevel)
        {
            if (towerLevel <= 1) return 4;
            if (towerLevel >= 2) return 6;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            return 0;
        }
    }
}