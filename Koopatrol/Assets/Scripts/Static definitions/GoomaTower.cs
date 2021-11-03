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
            if (towerLevel == 1) return "<sprite=0>=1 <sprite=1>=1 <sprite=2>=2 <sprite=3>=4| Goomba tower. Upgrade <sprite=1>+1";
            return "<sprite=0>=1 <sprite=1>=2 <sprite=2>=2| Goomba tower+1. Magic <sprite=2>-1";
        }
        public static float GetRange(int towerLevel)
        {
            return 200;
        }
        public static float GetCooldown(int towerLevel)
        {
            if (towerLevel <= 2) return 2;
            if (towerLevel >= 3) return 1;
            return 0;
        }
        public static int GetDamage(int towerLevel)
        {
            return 1;
        }
        public static float GetSpeed(int towerLevel)
        {
            if (towerLevel == 1) return 50;
            if (towerLevel >= 2) return 100;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 10;
            return 0;
        }
    }
}