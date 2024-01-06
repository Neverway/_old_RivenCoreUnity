//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Smoothly transition from one song to another, or fade a song out
// Notes:
//
//=============================================================================

using System.Collections;
using UnityEngine;

public class System_MusicManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    public float fadeDuration = 1f;

    private float initialVolume;
    private bool isFadingPrimary;
    private bool isFadingSecondary;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private AudioSource primaryChannel;
    [SerializeField] private AudioSource secondaryChannel;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        primaryChannel.Play();
        secondaryChannel.Play();
        primaryChannel.volume=1;
        secondaryChannel.volume=0;
    }
    
    private void Awake()
    {
        primaryChannel.Play();
        secondaryChannel.Play();
        primaryChannel.volume=1;
        secondaryChannel.volume=0;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void ResetVolume()
    {
	    primaryChannel.volume=1;
	    secondaryChannel.volume=0;
    }
    
    private IEnumerator Fade1(float _targetVolume, int _track)
    {
	    if (_track == 0) isFadingPrimary = true;
	    else if (_track == 1) isFadingSecondary = true;
	    float timer = 0f;

	    while (timer < fadeDuration)
	    {
		    timer += Time.deltaTime;
		    float progress = timer / fadeDuration;

		    if (_track == 0) primaryChannel.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    else if (_track == 1) secondaryChannel.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    yield return null;
	    }

	    if (_track == 0) primaryChannel.volume = _targetVolume;
	    else if (_track == 1) secondaryChannel.volume = _targetVolume;
	    if (_track == 0) isFadingPrimary = false;
	    else if (_track == 1)
	    {
		    isFadingSecondary = false;
		    // if this is true, we were fading in the secondary channel, so we should now make that our new primary channel
		    if (_targetVolume == 1)
		    {
			    SetPrimaryChannel(secondaryChannel.clip);
			    SetSecondaryChannel(null);
			    ResetChannels();
			    ResetVolume();
		    }
	    }
    }
    private IEnumerator Fade2(float _targetVolume, int _track)
    {
	    if (_track == 0) isFadingPrimary = true;
	    else if (_track == 1) isFadingSecondary = true;
	    float timer = 0f;

	    while (timer < fadeDuration)
	    {
		    timer += Time.deltaTime;
		    float progress = timer / fadeDuration;

		    if (_track == 0) primaryChannel.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    else if (_track == 1) secondaryChannel.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    yield return null;
	    }

	    if (_track == 0) primaryChannel.volume = _targetVolume;
	    else if (_track == 1) secondaryChannel.volume = _targetVolume;
	    if (_track == 0) isFadingPrimary = false;
	    else if (_track == 1)
	    {
		    isFadingSecondary = false;
		    // if this is true, we were fading in the secondary channel, so we should now make that our new primary channel
		    if (_targetVolume == 1)
		    {
			    SetPrimaryChannel(secondaryChannel.clip);
			    SetSecondaryChannel(null);
			    ResetChannels();
			    ResetVolume();
		    }
	    }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetPrimaryChannel(AudioClip _audioClip)
    {
	    primaryChannel.clip = _audioClip;
    }
    
    public void SetSecondaryChannel(AudioClip _audioClip)
    {
	    secondaryChannel.clip = _audioClip;
    }

    public void FadeIn(int _track)
    {
	    if (_track == 0 && isFadingPrimary || _track == 1 && isFadingSecondary) return;

	    initialVolume = primaryChannel.volume;
	    StartCoroutine(Fade1(1, _track));
    }

    // Insert Fadeout: Underground joke here
    public void Fadeout(int _track)
    {
	    if (_track == 0 && isFadingPrimary || _track == 1 && isFadingSecondary) return;

	    initialVolume = primaryChannel.volume;
	    StartCoroutine(Fade2(0, _track));
    }

    /// <summary>
    /// Fade the music from the primary channel to the secondary channel, after the music has fully switched tracks, swap the secondary channel as the new primary
    /// </summary>
    public void CrossFadeTracks()
    {
	    Fadeout(0);
	    FadeIn(1);
    }

    public void ResetChannels()
    {
	    primaryChannel.Play();
	    secondaryChannel.Play();
    }
}
