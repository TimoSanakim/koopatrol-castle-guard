using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class BulletBlaster
    {
        public static int bulletImage = 3;
        public static int GetSellCost(int towerLevel)
        {
            if (towerLevel == 1) return 40;
            if (towerLevel == 2) return 60;
            if (towerLevel == 3) return 90;
            return 0;
        }
        public static string GetDescription(int towerLevel)
        {
            if (towerLevel == 1) return "Bullet blaster. A high-fire rate tower which can only be placed next to a path on 1 axis, so not corners. Deals 2 damage, rather than 1. Can be upgraded to increase Bullet Bill's movement speed and once again to increase damage further.";
            if (towerLevel == 2) return "Bullet blaster+1. A high-fire rate tower which can only be placed next to a path on 1 axis, so not corners. Deals 2 damage, rather than 1. Can be upgraded once again to increase damage further.";
            return "Bullet blaster+2. A high-fire rate tower which can only be placed next to a path on 1 axis, so not corners. Deals 3 damage, rather than 1.";
        }
        public static float GetCooldown(int towerLevel)
        {
            return 1;
        }
        public static int GetDamage(int towerLevel)
        {
            if (towerLevel <= 2) return 2;
            if (towerLevel >= 3) return 3;
            return 0;
        }
        public static float GetSpeed(int towerLevel)
        {
            if (towerLevel == 1) return 50;
            if (towerLevel == 2) return 100;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 40;
            if (towerLevel == 2) return 60;
            return 0;
        }
    }
}