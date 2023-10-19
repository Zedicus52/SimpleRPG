using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SimpleRPG.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private AudioMixer _mixer;

        private AudioSource _source;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _source = GetComponent<AudioSource>();
            _source.clip = _backgroundMusic;
        }
    }
}