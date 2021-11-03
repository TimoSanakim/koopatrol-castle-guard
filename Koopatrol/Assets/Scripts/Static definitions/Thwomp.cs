using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class Thwomp
    {
        public static int GetSellCost(int towerLevel)
        {
            return 30;
        }
        public static string GetDescription(int towerLevel)
        {
            return "<sprite=5>=1 <sprite=2>=4 <sprite=3>=2.8| Thwomp. Hits all enemies in range and breaks ice. Magic <sprite=5>+1";
        }
        public static float GetRange(int towerLevel)
        {
            return 140;
        }
        public static float GetCooldown(int towerLevel)
        {
            return 4;
        }
        public static float GetStaggerTime(int towerLevel)
        {
            if (towerLevel == 1) return 1;
            if (towerLevel >= 2) return 2;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            return 0;
        }
    }
}