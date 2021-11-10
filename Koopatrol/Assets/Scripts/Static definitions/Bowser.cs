using System.Collections;
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
            if (towerLevel == 1) return "<sprite=0>=5 <sprite=1>=2 <sprite=2>=3 <sprite=3>=8| " + "All hail the king! Only one can exist in the map. " + 
            "Upgrade <sprite=2>-1";
            if (towerLevel == 2) return "<sprite=0>=5 <sprite=1>=2 <sprite=2>=2 <sprite=3>=8| " + "Bowser+1. " + 
            "Upgrade <sprite=2>-1";
            return "<sprite=0>=5 <sprite=1>=2 <sprite=2>=1 <sprite=3>=8| " +"Bowser+2. "+ "Magic gains additional small radial attack.";
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
            return 5;
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