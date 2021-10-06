using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class PiranhaPlant 
    {
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
            else return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            return 0;
        }
    }
}