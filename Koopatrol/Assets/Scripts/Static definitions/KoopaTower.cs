using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class KoopaTower
    {
        public static int bulletImage = 1;
        public static int GetSellCost(int towerLevel)
        {
            if (towerLevel == 1) return 15;
            if (towerLevel == 2) return 20;
            if (towerLevel == 3) return 30;
            return 0;
        }
        public static string GetDescription(int towerLevel)
        {
            if (towerLevel == 1) return "Koopa tower. A tower which launches Koopas at the enemies, faster than the Goomba tower. Can be upgraded to increase the fire rate, and once more to increase the detection range.";
            if (towerLevel == 2) return "Koopa tower+1. A tower which launches Koopas at the enemies, faster than the Goomba tower. Can be upgraded once more to increase the detection range.";
            return "Koopa tower+2. A tower which launches Koopas at the enemies, faster than the Goomba tower. When enhanced with magic, increases damage by 1.";
        }
        public static float GetRange(int towerLevel)
        {
            if (towerLevel <= 2) return 200;
            if (towerLevel >= 3) return 300;
            return 0;
        }
        public static float GetCooldown(int towerLevel)
        {
            if (towerLevel == 1) return 2;
            if (towerLevel >= 2) return 1;
            return 0;
        }
        public static int GetDamage(int towerLevel)
        {
            if (towerLevel <= 3) return 1;
            if (towerLevel >= 4) return 2;
            return 0;
        }
        public static float GetSpeed(int towerLevel)
        {
            return 100;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 20;
            if (towerLevel == 2) return 40;
            return 0;
        }
    }
}