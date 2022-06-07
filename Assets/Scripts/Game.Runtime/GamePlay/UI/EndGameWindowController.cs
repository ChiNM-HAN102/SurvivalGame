using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class EndGameWindowController : MonoBehaviour
    {
        [SerializeField] private Button playAgainBtn;

        [SerializeField] private Button quitBtn;

        [SerializeField] private Text survivalText;

        private void Awake()
        {
            this.playAgainBtn.onClick.AddListener(PlayAgain);
            this.quitBtn.onClick.AddListener(Quit);
        }
        

        private void OnEnable()
        {
            var survivalTime = PlayerPrefs.GetInt(Constants.DATA_CURRENT_SURVIVAL_TIME_KEY);
            this.survivalText.text = $"{survivalTime} seconds";
        }

        void PlayAgain()
        {
            GamePlayController.Instance.ResetGame();
        }


        void Quit()
        {
            Application.Quit();
        }
        
    }
}
