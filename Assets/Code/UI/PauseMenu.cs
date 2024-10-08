using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        private bool paused = false;
        private bool timeIsFrozen = false;

        public bool TimeIsFrozen { set => timeIsFrozen = value; }

        public void PlayPause()
        {
            if (paused = !paused)
            {
                ShowPauseMenu();
            }
            else
            {
                HidePauseMenu();
            }
        }

        public void ShowPauseMenu()
        {
            SoundPlayer.Instance.PlaySound("Click");
            paused = true;
            if (!timeIsFrozen)
                Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }

        public void HidePauseMenu()
        {
            SoundPlayer.Instance.PlaySound("Click");
            paused = false;
            if (!timeIsFrozen)
                Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
}
