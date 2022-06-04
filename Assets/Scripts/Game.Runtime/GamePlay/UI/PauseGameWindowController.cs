using System;
using UnityEngine;
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
        }

        void ToggleSound()
        {
            
        }

        void ToggleMusic()
        {
            
        }

        void PlayAgain()
        {
            GamePlayController.Instance.ResetGame();
        }

        void PlayContinue()
        {
            GamePlayController.Instance.ResumeGame();
        }
    }
}