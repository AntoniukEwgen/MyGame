using System;
using UnityEngine;

namespace Vampire
{
    public class CurrencySlotMachine : MonoBehaviour
    {
        public event Action<int> OnCurrencyChange;
        public int currency;
        public void AddCurrency(int value)
        {
            currency += value;
            OnCurrencyChange?.Invoke(currency);
        }

        public void TakeCurrency(int value)
        {
            if (currency <= 0)
            {
                return; // Exit the method if currency is zero or less
            }

            currency -= value;
            OnCurrencyChange?.Invoke(currency);
        }

        public void SetCurrency(int value)
        {
            currency = value;
            OnCurrencyChange?.Invoke(currency);
        }
    }
}
