using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "CurrencyRepository", menuName = "so/currencyRepository")]
    public class CurrencyRepository : ScriptableObject
    {
        private const string CoinPrefKey = "COIN_PREF_KEY";

        private int _coin;
        private int _previousCoin;


        private void OnEnable()
        {
            _coin = PlayerPrefs.GetInt(CoinPrefKey, 0);
            _previousCoin = _coin;
        }

        public void AddCoin(int amount)
        {
            _previousCoin = _coin;
            _coin += amount;
            PlayerPrefs.SetInt(CoinPrefKey, _coin);
        }

        public void RemoveCoin(int amount)
        {
            if (amount > _coin) return;
            _previousCoin = _coin;
            _coin -= amount;
            PlayerPrefs.SetInt(CoinPrefKey, _coin);
        }

        public int GetCoin() => _coin;

        public int PreviousCoin() => _previousCoin;
    }
}