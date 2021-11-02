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
            return "Magikoopa. A tower which doesn't affect enemies, instead enhances nearby towers with magic, treating them as a tower of one upgrade higher.";
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