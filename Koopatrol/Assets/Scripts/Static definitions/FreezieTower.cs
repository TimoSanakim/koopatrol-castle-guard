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
            if (towerLevel == 1) return "<sprite=4>=4 <sprite=1>=0.5 <sprite=2>=7 <sprite=3>=2| Freezie tower. Upgrade <sprite=1>+0.5";
            if (towerLevel == 2) return "<sprite=4>=4 <sprite=1>=1 <sprite=2>=7 <sprite=3>=2| Freezie tower+1. Upgrade <sprite=4>+2";
            return "<sprite=4>=6 <sprite=1>=1 <sprite=2>=7 <sprite=3>=2| Freezie tower+1. Magic <sprite=3>+1";
        }
        public static float GetRange(int towerLevel)
        {
            if (towerLevel <= 3) return 100;
            if (towerLevel >= 4) return 150;
            return 0;
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