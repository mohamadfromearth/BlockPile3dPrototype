using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "CurrencyRepository", menuName = "so/currencyRepository")]
    public class CurrencyRepository : ScriptableObject
    {
        private const string CoinPrefKey = "COIN_PREF_KEY";

        private int _coin;


        private void OnEnable()
        {
            _coin = PlayerPrefs.GetInt(CoinPrefKey, 0);
        }

        public void AddCoin(int amount)
        {
            _coin += amount;
            PlayerPrefs.SetInt(CoinPrefKey, _coin);
        }

        public void RemoveCoin(int amount)
        {
            if (amount > _coin) return;
            _coin -= amount;
            PlayerPrefs.SetInt(CoinPrefKey, _coin);
        }

        public int GetCoin() => _coin;
    }
}