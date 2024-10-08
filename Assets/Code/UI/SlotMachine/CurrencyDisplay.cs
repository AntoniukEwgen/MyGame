using TMPro;
using UnityEngine;

namespace Vampire
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] CurrencySlotMachine currency;
        [SerializeField] TextMeshProUGUI currencyText;
        private void OnDisable()
        {
            currency.OnCurrencyChange -= ChangeText;
        }

        private void OnEnable()
        {
            currency.OnCurrencyChange += ChangeText;
        }

        private void ChangeText(int value)
        {
            currencyText.text = value.ToString();
        }
    }
}
