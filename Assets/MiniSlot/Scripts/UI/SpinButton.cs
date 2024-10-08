using JGM.Game.Audio;
using JGM.Game.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.Game.UI
{
    public class SpinButton : MonoBehaviour
    {
        [Inject] private IAudioService _audioService;
        [Inject] private IEventTriggerService _eventTriggerService;

        [SerializeField] private Button _spinButton;

        public void TriggerStartSpinEvent()
        {
            _audioService.Play("Press Button");
            StartCoroutine(SendStartSpinEventAfterAudioFinishedPlaying());
        }

        private IEnumerator SendStartSpinEventAfterAudioFinishedPlaying()
        {
            yield return new WaitWhile(() => _audioService.IsPlaying("Press Button"));
            _eventTriggerService.Trigger("Start Spin");
        }

        public void SetButtonInteraction(bool makeInteractable)
        {
            _spinButton.interactable = makeInteractable;
        }
    }
}