using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playAudio(int id)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClips[id]);
    }
    public void playMusic (int id)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[id];
        audioSource.Play();
    }
    public void stopMusic ()
    {
        audioSource.Stop();
    }
}
