using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class SoundController : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundSource;

        [Header("Music clip")] 
        [SerializeField] private AudioClip gamePlayClip;
        [SerializeField] private AudioClip waitingClip;

        [Header("Sound Clip")]
        [SerializeField] private AudioClip clickSound;

        [SerializeField] private AudioClip startSound;
        [SerializeField] private AudioClip endSound;
        [SerializeField] private AudioClip waitingSound;
        [SerializeField] private AudioClip pickItemSound;
        [SerializeField] private AudioClip heroHurtSound;
        [SerializeField] private AudioClip enemyHurtSound;
        [SerializeField] private AudioClip enemyCallSound;
        
        
        
        public static SoundController Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Init()
        {
            var music = PlayerPrefs.GetInt(Constants.DATA_TURN_OFF_MUSIC);
            var sound = PlayerPrefs.GetInt(Constants.DATA_TURN_OFF_SOUND);
            this.musicSource.mute = music != 0;
            this.soundSource.mute = sound != 0;
        }
        
        

        public void PlayMusic(AudioClip musicSound)
        {
            this.musicSource.clip = musicSound;
            this.musicSource.Play();
        }

        public void PlaySound(AudioClip sound)
        {
            this.soundSource.PlayOneShot(sound);
        }

        public void StopMusic()
        {
            this.musicSource.Stop();    
        }
        

        public void PlayClick()
        {
            PlaySound(this.clickSound);
        }

        public void PlayHeroHurt()
        {
            PlaySound(this.heroHurtSound);
        }

        public void PlayEnemyHurt()
        {
            PlaySound(this.enemyHurtSound);
        }

        public void PlayCallEnemy()
        {
            PlaySound(this.enemyCallSound);
        }

        public void PlayPickItem()
        {
            PlaySound(this.pickItemSound);
        }

        public void PlayStartSound()
        {
            PlaySound(this.startSound);
        }

        public void PlayWaitingSound()
        {
            PlaySound(this.waitingSound);
        }
        

        public void PlayEndSound()
        {
            PlaySound(this.endSound);
        }


        public void PlayWaitingMusic()
        {
            PlayMusic(this.waitingClip);
        }

        public void PlayGamePlayMusic()
        {
            PlayMusic(this.gamePlayClip);
        }


        public void ToggleMusic()
        {
            var music = PlayerPrefs.GetInt(Constants.DATA_TURN_OFF_MUSIC);
            if (music == 0)
            {
                PlayerPrefs.SetInt(Constants.DATA_TURN_OFF_MUSIC, 1);
                this.musicSource.mute = true;
            }
            else
            {
                PlayerPrefs.SetInt(Constants.DATA_TURN_OFF_MUSIC, 0);
                this.musicSource.mute = false;
            }
        }

        public void ToggleSound()
        {
            var sound = PlayerPrefs.GetInt(Constants.DATA_TURN_OFF_SOUND);
            if (sound == 0)
            {
                PlayerPrefs.SetInt(Constants.DATA_TURN_OFF_SOUND, 1);
                this.soundSource.mute = true;
            }
            else
            {
                PlayerPrefs.SetInt(Constants.DATA_TURN_OFF_SOUND, 0);
                this.soundSource.mute = false;
            }
        }
    }
}
