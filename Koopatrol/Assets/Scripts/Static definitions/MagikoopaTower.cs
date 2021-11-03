using System.Collections;
using UnityEngine;

namespace Assets
{
    public static class MagikoopaTower
    {
        public static int GetSellCost(int towerLevel)
        {
            return 60;
        }
        public static string GetDescription(int towerLevel)
        {
            return "<sprite=3>=2.2| Magikoopa. Uses magic on towers, making them 1 upgrade level higher.";
        }
        public static float GetRange(int towerLevel)
        {
            return 111;
        }
        public static int GetUpgradeCost(int towerLevel)
        {
            return 0;
        }
    }
}