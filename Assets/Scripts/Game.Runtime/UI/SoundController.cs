using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundSource;
        
        public SoundController Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }
        
        
    }
}
