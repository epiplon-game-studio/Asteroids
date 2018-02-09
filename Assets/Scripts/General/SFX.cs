using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.General
{
    [RequireComponent(typeof(AudioSource))]
    public class SFX : MonoBehaviour
    {
        public AudioClip[] sounds;

        AudioSource audioSource;
        AudioClip currentClip;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play()
        {
            gameObject.SetActive(true);
            if (sounds.Length > 0)
            {
                currentClip = sounds[Random.Range(0, sounds.Length)];
                audioSource.PlayOneShot(currentClip);
                Invoke("Disable", currentClip.length + 1);
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
