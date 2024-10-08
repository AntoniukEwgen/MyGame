using UnityEngine;

namespace Vampire
{
    public class SlotMachineSpinSetting : MonoBehaviour
    {
        [SerializeField] CurrencySlotMachine currency;
        [SerializeField] SlotMachineDialog slotMachineDialog;

        public void CloseMenu()
        {
            if(!slotMachineDialog.win)
            {
                if (currency.currency <= 0)
                {
                    slotMachineDialog.Close();
                }
            }
        }
    }
}
