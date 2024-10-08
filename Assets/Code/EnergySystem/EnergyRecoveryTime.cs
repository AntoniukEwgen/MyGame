using UnityEngine;
using System;
using TMPro;

namespace Vampire
{
    public class EnergyRecoveryTime : MonoBehaviour
    {
        [Header("Energy Setting")]
        [SerializeField] int maxEnergy = 100;
        [SerializeField] public int currentEnergy;

        [Header("Time Setting")]
        [SerializeField] int hours = 0; // Hours for replenish interval
        [SerializeField] int minutes = 1; // Minutes for replenish interval
        [SerializeField] int seconds = 0; // Seconds for replenish interval

        [Header("Visual Setting")]
        [SerializeField] TextMeshProUGUI energyTimerText; // Reference to TextMeshPro object for displaying the timer
        [SerializeField] TextMeshProUGUI energyText; // Reference to TextMeshPro object for displaying the energy
        [SerializeField] GameObject timerPanel; // Reference to the panel to be shown or hidden
        [SerializeField] GameObject EnergyCountPanel;

        public Action<int> OnEnergyChanged; // Event for tracking energy changes

        private const string LastEnergyUpdateKey = "LastEnergyUpdate";
        private const string CurrentEnergyKey = "CurrentEnergy";
        private const string RemainingTimeKey = "RemainingTime";

        private DateTime nextReplenishTime;
        private TimeSpan remainingTime;
        private int replenishIntervalSeconds;
        private bool timerRunning = false;

        void Start()
        {
            // Calculate total seconds based on hours, minutes, and seconds
            replenishIntervalSeconds = hours * 3600 + minutes * 60 + seconds;
            LoadEnergy();
            InvokeRepeating(nameof(UpdateTimerText), 0, 1);
            UpdateEnergyText();
            UpdateTimerPanelVisibility();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveEnergy();
            }
        }

        void OnApplicationQuit()
        {
            SaveEnergy();
        }

        void ReplenishEnergy()
        {
            currentEnergy = maxEnergy;
            timerRunning = false;
            Debug.Log("Energy fully replenished. Current energy: " + currentEnergy);
            UpdateEnergyText();
            UpdateTimerPanelVisibility();
            SaveEnergy();
            OnEnergyChanged?.Invoke(currentEnergy); // Invoke event on energy change
        }

        void SaveEnergy()
        {
            PlayerPrefs.SetString(LastEnergyUpdateKey, DateTime.Now.ToBinary().ToString());
            PlayerPrefs.SetInt(CurrentEnergyKey, currentEnergy);
            PlayerPrefs.SetFloat(RemainingTimeKey, (float)remainingTime.TotalSeconds);
            PlayerPrefs.Save();
        }

        void LoadEnergy()
        {
            if (PlayerPrefs.HasKey(LastEnergyUpdateKey))
            {
                long temp = Convert.ToInt64(PlayerPrefs.GetString(LastEnergyUpdateKey));
                DateTime lastUpdate = DateTime.FromBinary(temp);

                TimeSpan timePassed = DateTime.Now - lastUpdate;
                int secondsPassed = (int)timePassed.TotalSeconds;

                currentEnergy = PlayerPrefs.GetInt(CurrentEnergyKey, maxEnergy);
                float remainingSeconds = PlayerPrefs.GetFloat(RemainingTimeKey, replenishIntervalSeconds);
                remainingTime = TimeSpan.FromSeconds(remainingSeconds);

                if (currentEnergy <= 0)
                {
                    remainingTime -= timePassed;
                    if (remainingTime.TotalSeconds <= 0)
                    {
                        ReplenishEnergy();
                    }
                    else
                    {
                        nextReplenishTime = DateTime.Now.Add(remainingTime);
                        timerRunning = true;
                    }
                }
            }
            else
            {
                currentEnergy = maxEnergy;
                remainingTime = TimeSpan.FromSeconds(replenishIntervalSeconds);
                timerRunning = false;
            }

            Debug.Log("Energy loaded. Current energy: " + currentEnergy);
            UpdateEnergyText();
            UpdateTimerPanelVisibility();
            OnEnergyChanged?.Invoke(currentEnergy); // Invoke event on energy load
        }

        public void UseEnergy(int price)
        {
            if (price > currentEnergy)
            {
                price = currentEnergy;
            }
            currentEnergy -= price;
            Debug.Log("Energy used. Amount: " + price + ", Current energy: " + currentEnergy);
            UpdateEnergyText();

            if (currentEnergy <= 0 && !timerRunning)
            {
                remainingTime = TimeSpan.FromSeconds(replenishIntervalSeconds);
                nextReplenishTime = DateTime.Now.Add(remainingTime);
                timerRunning = true;
            }

            SaveEnergy();
            UpdateTimerPanelVisibility();
            OnEnergyChanged?.Invoke(currentEnergy); // Invoke event on energy change
        }

        void UpdateTimerText()
        {
            if (!timerRunning)
            {
                energyTimerText.text = "";
                return;
            }

            TimeSpan timeRemaining = nextReplenishTime - DateTime.Now;
            if (timeRemaining.TotalSeconds > 0)
            {
                string timerText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
                energyTimerText.text = timerText;
            }
            else
            {
                ReplenishEnergy();
            }
        }

        void UpdateTimerPanelVisibility()
        {
            if (timerPanel != null)
            {
                bool shouldShowTimer = currentEnergy <= 0 && timerRunning;
                timerPanel.SetActive(shouldShowTimer);
                EnergyCountPanel.SetActive(!shouldShowTimer);
            }
        }

        void UpdateEnergyText()
        {
            energyText.text = $"{currentEnergy}/{maxEnergy}";
        }
    }
}
