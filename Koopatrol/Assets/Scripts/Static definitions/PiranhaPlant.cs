using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class PiranhaPlant
    {
        public static int GetSellCost(int towerLevel)
        {
            return 50;
        }
        public static string GetDescription(int towerLevel)
        {
            return "<sprite=0>=2 <sprite=5>=4 <sprite=2>=8| Piranha plant. Hit breaks ice. Magic <sprite=5>+2.";
        }
        public static float GetCooldown(int towerLevel)
        {
            return 8;
        }
        public static int GetDamage(int towerLevel)
        {
            return 2;
        }
        public static float GetStaggerTime(int towerLevel)
        {
            if (towerLevel <= 1) return 4;
            if (towerLevel >= 2) return 6;
            return 0;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            return 0;
        }
    }
}