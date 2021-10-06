using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class Thwomp
    {
        public static float GetRange(int towerLevel)
        {
            return 100;
        }
        public static float GetCooldown(int towerLevel)
        {
            return 4;
        }
        public static float GetStaggerTime(int towerLevel)
        {
            if (towerLevel == 1) return 1;
            if (towerLevel >= 2) return 2;
            else return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            return 0;
        }
    }
}