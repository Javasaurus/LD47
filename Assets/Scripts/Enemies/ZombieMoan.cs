using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMoan : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioClip hurt;
    private AudioSource audioSource;

    private float playIntervalMax = 5f;
    private float playIntervalMin = 2f;
    private float timer;
    // Start is called before the first frame update

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        if (Time.time > timer)
        {
            timer = Time.time + Random.Range(playIntervalMin, playIntervalMax);
            PlayVoiceClip();
        }
    }

    public void PlayVoiceClip()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    public void PlayHurt()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = Random.Range(.9f, 1.3f);
            audioSource.PlayOneShot(hurt);
        }
    }
}
