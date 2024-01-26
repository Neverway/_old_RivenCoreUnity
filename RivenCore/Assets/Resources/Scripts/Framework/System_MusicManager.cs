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
    public float trackFadeDuration = 1f;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private float initialVolume;
    private bool isFadingPrimary;
    private bool isFadingSecondary;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private AudioSource primaryTrackAudioSource;
    [SerializeField] private AudioSource secondaryTrackAudioSource;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
	    primaryTrackAudioSource.Play();
	    secondaryTrackAudioSource.Play();
	    primaryTrackAudioSource.volume=1;
	    secondaryTrackAudioSource.volume=0;
    }
    
    private void Awake()
    {
	    primaryTrackAudioSource.Play();
	    secondaryTrackAudioSource.Play();
        primaryTrackAudioSource.volume=1;
        secondaryTrackAudioSource.volume=0;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void ResetVolume()
    {
	    primaryTrackAudioSource.volume=1;
	    secondaryTrackAudioSource.volume=0;
    }
    
    private IEnumerator Fade1(float _targetVolume, int _track)
    {
	    if (_track == 0) isFadingPrimary = true;
	    else if (_track == 1) isFadingSecondary = true;
	    float timer = 0f;

	    while (timer < trackFadeDuration)
	    {
		    timer += Time.deltaTime;
		    float progress = timer / trackFadeDuration;

		    if (_track == 0) primaryTrackAudioSource.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    else if (_track == 1) secondaryTrackAudioSource.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    yield return null;
	    }

	    if (_track == 0) primaryTrackAudioSource.volume = _targetVolume;
	    else if (_track == 1) secondaryTrackAudioSource.volume = _targetVolume;
	    if (_track == 0) isFadingPrimary = false;
	    else if (_track == 1)
	    {
		    isFadingSecondary = false;
		    // if this is true, we were fading in the secondary channel, so we should now make that our new primary channel
		    if (_targetVolume == 1)
		    {
			    SetPrimaryChannel(secondaryTrackAudioSource.clip);
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

	    while (timer < trackFadeDuration)
	    {
		    timer += Time.deltaTime;
		    float progress = timer / trackFadeDuration;

		    if (_track == 0) primaryTrackAudioSource.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    else if (_track == 1) secondaryTrackAudioSource.volume = Mathf.Lerp(initialVolume, _targetVolume, progress);
		    yield return null;
	    }

	    if (_track == 0) primaryTrackAudioSource.volume = _targetVolume;
	    else if (_track == 1) secondaryTrackAudioSource.volume = _targetVolume;
	    if (_track == 0) isFadingPrimary = false;
	    else if (_track == 1)
	    {
		    isFadingSecondary = false;
		    // if this is true, we were fading in the secondary channel, so we should now make that our new primary channel
		    if (_targetVolume == 1)
		    {
			    SetPrimaryChannel(secondaryTrackAudioSource.clip);
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
	    primaryTrackAudioSource.clip = _audioClip;
    }
    
    public void SetSecondaryChannel(AudioClip _audioClip)
    {
	    secondaryTrackAudioSource.clip = _audioClip;
    }

    public void FadeIn(int _track)
    {
	    if (_track == 0 && isFadingPrimary || _track == 1 && isFadingSecondary) return;

	    initialVolume = primaryTrackAudioSource.volume;
	    StartCoroutine(Fade1(1, _track));
    }

    // Insert Fadeout: Underground joke here
    public void Fadeout(int _track)
    {
	    if (_track == 0 && isFadingPrimary || _track == 1 && isFadingSecondary) return;

	    initialVolume = primaryTrackAudioSource.volume;
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
	    primaryTrackAudioSource.Play();
	    secondaryTrackAudioSource.Play();
    }
}
