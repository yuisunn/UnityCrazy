using SLCGame.Tools;
using SLCGame.Tools.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SLCGame.Unity
{
    public class AudioPreferences
    {
        public float MusicVolume;
        public float SoundVolume;

        public bool MuteMusic;
        public bool MuteSound;
    }
    class AudioMgr : UnitySingleton<AudioMgr>
    {
        private AudioPreferences audioPreferences;

        public float MusicVolume { get { return audioPreferences.MusicVolume; } set { audioPreferences.MusicVolume = Mathf.Clamp(value, 0.0f, 1.0f); ApplySettings(); } }
        public float SoundVolume { get { return audioPreferences.SoundVolume; } set { audioPreferences.SoundVolume = Mathf.Clamp(value, 0.0f, 1.0f); ApplySettings(); } }

        public bool MuteMusic { get { return audioPreferences.MuteMusic; } set { audioPreferences.MuteMusic = value; ApplySettings(); } }
        public bool MuteSound { get { return audioPreferences.MuteSound; } set { audioPreferences.MuteSound = value; ApplySettings(); } }

        public AudioSource musicSource;
        public AudioSource soundSource;

        public AudioClip buttonClickSound;

        // Use this for initialization
        void Awake()
        {
            AudioSource[] audioSources = this.gameObject.GetComponents<AudioSource>();
            if (audioSources.Length == 2)
            {
                musicSource = audioSources[0];
                soundSource = audioSources[1];
            }
            else
            {
                Debug.LogWarning("AudioManager does not contain the exact number of AudioSource components. Removing existing.");
                foreach (AudioSource source in audioSources)
                {
                    Destroy(source);
                }

                musicSource = this.gameObject.AddComponent<AudioSource>();
                soundSource = this.gameObject.AddComponent<AudioSource>();
            }

            LoadSettings();

            buttonClickSound = Resources.Load<AudioClip>("Sounds/buttonClick");

            ApplySettings();
        }  

        public void PlayMusic(AudioClip music, bool loop = false)
        {
            if (music != null)
            {
                // Play the given music.
                musicSource.Stop();
                musicSource.clip = music;
                musicSource.loop = loop;
                musicSource.Play(); 
            }
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void PlaySound(AudioClip sound, bool loop = false)
        {
            if (sound != null)
            {
                //Debug.Log ("Do play!");

                // Play the given music.
                soundSource.PlayOneShot(sound);
            }
        }

        public void PlayButtonClickSound()
        {
            Debug.Log("lalalalalalala PlayButtonClickSound!");

            PlaySound(buttonClickSound, false);
        }

        public void LoadSettings()
        {
            this.audioPreferences = LoadAudioPreferences();
        }

        public void SaveSettings()
        {
            SaveAudioPreferences(this.audioPreferences);
        }

        public void ApplySettings()
        {
            if (MuteMusic)
            {
                musicSource.volume = 0.0f;
            }
            else
            {
                musicSource.volume = MusicVolume;
            }

            if (MuteSound)
            {
                soundSource.volume = 0.0f;
            }
            else
            {
                soundSource.volume = SoundVolume;
            }

            SaveAudioPreferences(audioPreferences);
        }


        public AudioPreferences LoadAudioPreferences()
        {
            AudioPreferences preferences = new AudioPreferences();
            preferences.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            preferences.SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
            preferences.MuteMusic = (PlayerPrefs.GetInt("MuteMusic", 0) == 1);
            preferences.MuteSound = (PlayerPrefs.GetInt("MuteSound", 0) == 1);

            return preferences;
        }

        public void SaveAudioPreferences(AudioPreferences audioPreferences)
        {
            PlayerPrefs.SetFloat("MusicVolume", audioPreferences.MusicVolume);
            PlayerPrefs.SetFloat("SoundVolume", audioPreferences.SoundVolume);
            PlayerPrefs.SetInt("MuteMusic", audioPreferences.MuteMusic ? 1 : 0);
            PlayerPrefs.SetInt("MuteSound", audioPreferences.MuteSound ? 1 : 0);
            PlayerPrefs.Save();

        }
        /// <summary>
        /// 卸载 asset
        /// </summary>
        public void UnloadSoundBundle()
        {

        }
    }
}
