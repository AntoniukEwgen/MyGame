using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vampire
{
    public class WinGameDialog : DialogBox
    {
        [SerializeField] private TextMeshProUGUI coinsGained;
        [SerializeField] private TextMeshProUGUI enemiesRouted;
        [SerializeField] private TextMeshProUGUI damageDealt;
        [SerializeField] private TextMeshProUGUI damageTaken;
        [SerializeField] private GameObject background;

        public void Open(bool levelPassed, StatsManager statsManager)
        {
            coinsGained.text = "+" + statsManager.CoinsGained;
            enemiesRouted.text = statsManager.MonstersKilled.ToString();
            damageDealt.text = statsManager.DamageDealt.ToString();
            damageTaken.text = statsManager.DamageTaken.ToString();
            background.SetActive(true);
            base.Open();
        }

        public override void Close()
        {
            base.Close();
            background.SetActive(false);
        }
    }
}
