using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class SlotMachineDialog : DialogBox
    {
        [SerializeField] private AbilitySelectionDialog abilitySelectionDialog;

        [SerializeField] private int startCurrency;
        [SerializeField] CurrencySlotMachine currency;
        [SerializeField] private GameObject CloseButton;
        [SerializeField] private GameObject slotMachineObject;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private float closeDelay = 2f; // Delay duration exposed in the Inspector

        public bool win;
        private bool menuOpen = false;
        public bool MenuOpen { get => menuOpen; }

        public void Open()
        {
            base.Open();
            SoundPlayer.Instance.PlaySound("LevelUP");
            Time.timeScale = 0;
            win = false;
            currency.SetCurrency(startCurrency);
            CloseButton.SetActive(true);
            slotMachineObject.SetActive(true);
            menuOpen = true;
            pauseMenu.TimeIsFrozen = true;
        }

        public override void Close()
        {
            Time.timeScale = 1;
            menuOpen = false;
            pauseMenu.TimeIsFrozen = false;
            slotMachineObject.SetActive(false);
            base.Close();
        }

        private IEnumerator CloseCoroutine()
        {
            CloseButton.SetActive(false);
            yield return new WaitForSecondsRealtime(closeDelay);
            Close();
            abilitySelectionDialog.Open();
        }

        public void DelayedClose()
        {
            win = true;
            StartCoroutine(CloseCoroutine());
        }
    }
}
