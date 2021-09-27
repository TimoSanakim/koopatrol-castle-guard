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
        public static bool ChangeCoinCounter(int increaseAmount)
        {
            int coinCounter = Convert.ToInt32(GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text);
            coinCounter += increaseAmount;
            if (coinCounter < 0 )
            {
                Debug.Log("Action resulted in negative coin counter; action cancled.");
                return false;
            }
            else
            {
                GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>().text = Convert.ToString(coinCounter);
                return true;
            }
        }
    }
}