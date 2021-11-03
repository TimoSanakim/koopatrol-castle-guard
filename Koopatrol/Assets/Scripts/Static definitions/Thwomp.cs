using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class Thwomp
    {
        public static int GetSellCost(int towerLevel)
        {
            return 30;
        }
        public static string GetDescription(int towerLevel)
        {
            return "Thwomp. A Non-firing tower, instead scares nearby enemies for a little bit by slamming the ground. Breaks the ice enemies got frozen by. When enhanced by magic, increases the scare time.";
        }
        public static float GetRange(int towerLevel)
        {
            return 140;
        }
        public static float GetCooldown(int towerLevel)
        {
            return 4;
        }
        public static float GetStaggerTime(int towerLevel)
        {
            if (towerLevel == 1) return 1;
            if (towerLevel >= 2) return 2;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            return 0;
        }
    }
}