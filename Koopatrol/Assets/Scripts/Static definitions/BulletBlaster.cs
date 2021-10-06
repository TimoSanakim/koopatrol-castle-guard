using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class BulletBlaster
    {
        public static int bulletImage = 3;
        public static float GetCooldown(int towerLevel)
        {
            return 1;
        }
        public static int GetDamage(int towerLevel)
        {
            return 3;
        }
        public static float GetSpeed(int towerLevel)
        {
            if (towerLevel == 1) return 50;
            if (towerLevel == 2) return 100;
            if (towerLevel == 3) return 150;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 40;
            if (towerLevel == 2) return 60;
            else return 0;
        }
    }
}