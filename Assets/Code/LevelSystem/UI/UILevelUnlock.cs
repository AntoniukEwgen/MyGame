#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    public class UILevelUnlock : MonoBehaviour
    {
        [SerializeField] private Button[] levelButtons;
        [SerializeField] private EnergyRecoveryTime energyRecoveryTime;

        private const string LevelKeyPrefix = "Level_";

        private void OnEnable()
        {
            energyRecoveryTime.OnEnergyChanged += HandleEnergyChanged; // Subscribe to the event
        }

        private void OnDisable()
        {
            energyRecoveryTime.OnEnergyChanged -= HandleEnergyChanged; // Unsubscribe from the event
        }

        private void Start()
        {
            EnsureFirstButtonUnlocked();
            UpdateButtonInteractivity();
        }

        private void EnsureFirstButtonUnlocked()
        {
            // Always unlock the first button (index 0)
            if (PlayerPrefs.GetInt(LevelKeyPrefix + 0, 0) == 0)
            {
                PlayerPrefs.SetInt(LevelKeyPrefix + 0, 1);
                PlayerPrefs.Save();
            }
        }

        private void UpdateButtonInteractivity()
        {
            bool hasEnergy = energyRecoveryTime.currentEnergy > 0;
            for (int i = 0; i < levelButtons.Length; i++)
            {
                bool isLevelUnlocked = PlayerPrefs.GetInt(LevelKeyPrefix + i, 0) == 1;
                levelButtons[i].interactable = isLevelUnlocked && hasEnergy;
            }
        }

        private void HandleEnergyChanged(int currentEnergy)
        {
            UpdateButtonInteractivity();
        }

        public void LoadLevel(string LevelName)
        {
            ShowLoadingPanel.LoadLevel(LevelName);
        }

        public static void UnlockAllLevels()
        {
            for (int i = 0; i < PlayerPrefs.GetInt("TotalLevels", 10); i++) // Assume there are 10 levels
            {
                PlayerPrefs.SetInt(LevelKeyPrefix + i, 1);
            }
            PlayerPrefs.Save();
        }
    }
}

#if UNITY_EDITOR
public class UnlockAllLevelsMenu
{
    [MenuItem("Tools/Unlock All Levels")]
    public static void UnlockAllLevels()
    {
        Vampire.UILevelUnlock.UnlockAllLevels();
        Debug.Log("All levels unlocked!");
    }
}
#endif
