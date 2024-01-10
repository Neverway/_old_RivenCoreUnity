//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioVarienceModulator : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private Vector2 pitchVariance;
    [SerializeField] private bool delayedPlayOnAwake;
    [SerializeField] private float playOnAwakeDelay;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private AudioSource audioSource;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (delayedPlayOnAwake) StartCoroutine(PlaySound(playOnAwakeDelay));
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public IEnumerator PlaySound(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.pitch = Random.Range(pitchVariance.x, pitchVariance.y);
        audioSource.Play();
    }
    
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.pitch = Random.Range(pitchVariance.x, pitchVariance.y);
        audioSource.Play();
    }
}
