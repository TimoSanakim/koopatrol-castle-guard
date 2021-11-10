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
            if (towerLevel == 1) return "<sprite=0>=2 <sprite=1>=1 <sprite=2>=1 <sprite=3>=path| Bullet blaster. Upgrade <sprite=1>+1.";
            if (towerLevel == 2) return "<sprite=0>=2 <sprite=1>=2 <sprite=2>=1 <sprite=3>=path| Bullet blaster+1. Upgrade <sprite=0>+1.";
            return "<sprite=0>=3 <sprite=1>=2 <sprite=2>=1 <sprite=3>=path| Bullet blaster+2. Magic <sprite=2>-0.3.";
        }
        public static float GetCooldown(int towerLevel)
        {
            if (towerLevel <= 3) return 1;
            if (towerLevel >= 4) return 0.7f;
            return 0;
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
            if (towerLevel >= 2) return 100;
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