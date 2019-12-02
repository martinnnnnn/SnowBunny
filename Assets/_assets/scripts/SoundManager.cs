using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private AudioSource musicSource;
    private AudioSource soundSource;


    public AudioClip[] sounds;
    public AudioClip music;

    void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.volume = 0.5f;

        musicSource.PlayOneShot(music);
    }

    public void PlaySnowSound()
    {
        soundSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }
}
