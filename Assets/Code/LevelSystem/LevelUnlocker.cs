using UnityEngine;

namespace Vampire
{
    public class LevelUnlocker : MonoBehaviour
    {
        [SerializeField] private int levelIndex;
        private const string LevelKeyPrefix = "Level_";
        public void UnlockLevel()
        {
            PlayerPrefs.SetInt(LevelKeyPrefix + levelIndex, 1);
            PlayerPrefs.Save();
        }
    }
}
