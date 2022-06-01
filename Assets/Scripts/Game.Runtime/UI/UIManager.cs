using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private GameObject pauseGamePanel;
        [SerializeField] private GameObject startGamePanel;

        [SerializeField] private GameObject btnPauseGame;
        
        [SerializeField] private Text currentEnemyTxt;
        [SerializeField] private Text currentKilledEnemyTxt;
        [SerializeField] private Text currentTimeCountTxt;

        
        public static UIManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }
        
        
    }
}
