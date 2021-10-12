using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class GoomaTower
    {
        public static int bulletImage = 0;
        public static int GetSellCost(int towerLevel)
        {
            if (towerLevel == 1) return 5;
            if (towerLevel == 2) return 10;
            return 0;
        }
        public static string GetDescription(int towerLevel)
        {
            if (towerLevel == 1) return "Goomba tower. Slow shooting tower, can be upgraded to have the Goombas move a little faster";
            return "Goomba tower+1. Slow shooting tower";
        }
        public static float GetRange(int towerLevel)
        {
            return 200;
        }
        public static float GetCooldown(int towerLevel)
        {
            return 2;
        }
        public static int GetDamage(int towerLevel)
        {
            return 1;
        }
        public static float GetSpeed(int towerLevel)
        {
            if (towerLevel == 1) return 50;
            if (towerLevel >= 2) return 75;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 10;
            return 0;
        }
    }
}