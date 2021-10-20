﻿using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class Bowser
    {
        public static int bulletImage = 4;
        public static int GetSellCost(int towerLevel)
        {
            if (towerLevel == 1) return 250;
            if (towerLevel == 2) return 350;
            if (towerLevel == 3) return 500;
            return 0;
        }
        public static string GetDescription(int towerLevel)
        {
            if (towerLevel == 1) return "All hail the king! Only one can exist in the map. Has high range, deals 5 damage and fire is homing at the target. Can be upgraded twice to increase fire rate.";
            if (towerLevel == 2) return "All hail the king! Only one can exist in the map. Has high range, deals 5 damage and fire is homing at the target. Can be upgraded once more to increase fire rate.";
            return "All hail the king! Only one can exist in the map. Has high range, deals 5 damage and fire is homing at the target. When enhanced by magic, increases damage by 1.";
        }
        public static float GetRange(int towerLevel)
        {
            return 400;
        }
        public static float GetCooldown(int towerLevel)
        {
            if (towerLevel == 1) return 3;
            if (towerLevel == 2) return 2;
            if (towerLevel >= 3) return 1;
            return 0;
        }
        public static int GetDamage(int towerLevel)
        {
            if (towerLevel <= 3) return 5;
            if (towerLevel >= 4) return 6;
            return 0;
        }
        public static float GetSpeed(int towerLevel)
        {
            return 100;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            if (towerLevel == 1) return 200;
            if (towerLevel == 2) return 300;
            return 0;
        }
    }
}