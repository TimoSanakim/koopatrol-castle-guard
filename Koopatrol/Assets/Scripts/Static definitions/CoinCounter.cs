using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public static class CoinCounter
    {
        public static int CoinCount = 0;
        //Returns true if coin count doesn't become negative
        public static bool ChangeCoinCounter(int increaseAmount, bool forceSound)
        {
            if (CoinCount == 0) CoinCount = Convert.ToInt32(GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text);
            CoinCount += increaseAmount;
            if (CoinCount < 0)
            {
                Debug.Log("Action resulted in negative coin counter; action should be cancled.");
                CoinCount -= increaseAmount;
                return false;
            }
            else
            {
                if (CoinCount <= 9999) GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text = Convert.ToString(CoinCount);
                else GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text = "????";
                if (increaseAmount < 0 || forceSound) GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<AudioSource>().Play(0);
                return true;
            }
        }
        //Returns coint counter value
        public static int GetCoinCount()
        {
            if (CoinCount != 0) return CoinCount;
            else return Convert.ToInt32(GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text);
        }
    }
}