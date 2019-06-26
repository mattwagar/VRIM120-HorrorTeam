using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ambientAudioSource;
    public AudioSource cycleAudioSource;
    public AudioSource cycleWindDownAudioSource;
    public AudioSource cycleIdleAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip windDownAudio;
    public AudioClip idleCycleAudio;
    public AudioClip drivingCycleAudio;
    public AudioClip goodEndMusic;
    public AudioClip badEndMusic;
    public AudioClip gameMusic;
    public AudioClip spookyLaugh;
    public AudioClip cricketsAudio;

    public float animSpeed = 1;

    [Range(0,0.8f)]
    public float motorcycleVolume = 0f;

    private IEnumerator RaiseVolumeRoutine()
    {
        while(motorcycleVolume < 0.8f)
        {
            motorcycleVolume += Time.deltaTime * animSpeed;
            cycleAudioSource.volume = motorcycleVolume;
            yield return null;
        }
    }

    private IEnumerator FadeMusicRoutine()
    {
        float musicVolume = 1;
        while(musicVolume > 0f)
        {
            musicVolume -= Time.deltaTime * animSpeed;
            musicAudioSource.volume = musicVolume;
            yield return null;
        }
    }

    private void Start() 
    {
        SetAudioDefaults();
    }

    public void SetAudioDefaults()
    {
        cycleAudioSource.clip = drivingCycleAudio;
        cycleAudioSource.loop = true;
        cycleWindDownAudioSource.clip = windDownAudio;
        cycleIdleAudioSource.clip = idleCycleAudio;
        cycleIdleAudioSource.loop = true;
        musicAudioSource.clip = gameMusic;
        sfxAudioSource.clip = spookyLaugh;
    }

    public void VolumeRise()
    {
        StartCoroutine(RaiseVolumeRoutine());
    }

    public void FadeMusic()
    {
        StartCoroutine(FadeMusicRoutine());
    }

    public void GoodEnding()
    {
        musicAudioSource.Stop();
        musicAudioSource.volume = 1;
        musicAudioSource.clip = goodEndMusic;
        musicAudioSource.Play();
        ambientAudioSource.Stop();
        ambientAudioSource.clip = cricketsAudio;
        ambientAudioSource.Play();

    }

    public void PlaySpookyLaugh()
    {
        sfxAudioSource.loop = false;
        sfxAudioSource.clip = spookyLaugh;
        sfxAudioSource.Play();
    }

    public void SlowMotorcycle()
    {
        cycleWindDownAudioSource.loop = false;
        cycleWindDownAudioSource.Play();
        cycleAudioSource.Stop();
        cycleIdleAudioSource.PlayDelayed(6f);
    }

    public void StopMotorcycle()
    {
        cycleIdleAudioSource.Stop();
        // cycleWindDownAudioSource.Play();
    }

    public void SpeedUpMotorcycle()
    {
        cycleAudioSource.Play();
        cycleIdleAudioSource.Stop();
    }
}