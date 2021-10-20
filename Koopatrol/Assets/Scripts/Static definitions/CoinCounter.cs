using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public static class CoinCounter
    {
        //Returns true if coin count doesn't become negative
        public static bool ChangeCoinCounter(int increaseAmount, bool forceSound)
        {
            int coinCounter = Convert.ToInt32(GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text);
            coinCounter += increaseAmount;
            if (coinCounter < 0)
            {
                Debug.Log("Action resulted in negative coin counter; action should be cancled.");
                return false;
            }
            else
            {
                GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text = Convert.ToString(coinCounter);
                if (increaseAmount < 0 || forceSound) GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<AudioSource>().Play(0);
                return true;
            }
        }
        //Returns coint counter value
        public static int GetCoinCount()
        {
            return Convert.ToInt32(GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text);
        }
    }
}