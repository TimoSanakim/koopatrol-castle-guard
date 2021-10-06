using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class KoopaTower
    {
        public static int bulletImage = 1;
        public static float GetRange(int towerLevel)
        {
            if (towerLevel <= 2) return 200;
            if (towerLevel == 3) return 300;
            else return 0;
        }
        public static float GetCooldown(int towerLevel)
        {
            if (towerLevel == 1) return 2;
            if (towerLevel >= 2) return 1;
            else return 0;
        }
        public static int GetDamage(int towerLevel)
        {
            return 1;
        }
        public static float GetSpeed(int towerLevel)
        {
            return 100;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 20;
            if (towerLevel == 2) return 40;
            else return 0;
        }
    }
}