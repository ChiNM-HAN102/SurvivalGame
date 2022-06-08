using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class PauseGameWindowController : MonoBehaviour
    {
        [SerializeField] private Button toggleSound;
        [SerializeField] private Button toggleMusic;
        [SerializeField] private Button playAgain;
        [SerializeField] private Button continueButton;

        [SerializeField] private Text soundText;
        [SerializeField] private Text musicText;

        private void Awake()
        {
            this.toggleSound.onClick.AddListener(ToggleSound);
            this.toggleMusic.onClick.AddListener(ToggleMusic);
            this.playAgain.onClick.AddListener(PlayAgain);
            this.continueButton.onClick.AddListener(PlayContinue);

            UpdateButtonText();
        }

        void UpdateButtonText()
        {
            var music = PlayerPrefs.GetInt(Constants.DATA_TURN_OFF_MUSIC);
            this.musicText.text = music == 0 ? "Mute Music" : "Unmute Music";
            var sound = PlayerPrefs.GetInt(Constants.DATA_TURN_OFF_SOUND);
            this.soundText.text = sound == 0 ? "Mute Sound" : "Unmute Sound";
        }

        void ToggleSound()
        {
            SoundController.Instance.PlayClick();
            SoundController.Instance.ToggleSound();
           
            UpdateButtonText();
        }

        void ToggleMusic()
        {
            SoundController.Instance.PlayClick();
            SoundController.Instance.ToggleMusic();
            UpdateButtonText();
        }

        void PlayAgain()
        {
            SoundController.Instance.PlayClick();
            GamePlayController.Instance.ResetGame();
        }

        void PlayContinue()
        {
            SoundController.Instance.PlayClick();
            GamePlayController.Instance.ResumeGame();
        }
    }
}