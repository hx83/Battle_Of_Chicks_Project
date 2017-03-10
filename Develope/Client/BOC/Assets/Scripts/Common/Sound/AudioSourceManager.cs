using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AudioSourceManager
/// </summary>
public class AudioSourceManager : SingletonBase<AudioSourceManager>
{
    private AudioSource musicAudioSource;
    private AudioSource soundsAudioSource;
    private bool isMusicOn = true;
    private bool soundfx;
    private float musicVolume = 1f;
    private float soundfxVolume = 1f;

    protected static readonly string MusicEnabledKey = "MusicEnabled";
    protected static readonly string MusicVolumeKey = "MusicVolume";
    protected static readonly string SoundsEnabledKey = "SoundsEnabled";
    protected static readonly string SoundsVolumeKey = "SoundsVolume";

    public bool IsMusicOn()
    {
        return isMusicOn;
    }

    public AudioClip GetMusicClip()
    {
        return musicAudioSource.clip;
    }

    public bool GetSoundFX()
    {
        return soundfx;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSoundFXVolume()
    {
        return soundfxVolume;
    }

    public void Setup(GameObject obj)
    {
        musicAudioSource = obj.AddComponent<AudioSource>();

        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = false;
        musicAudioSource.volume = musicVolume;

        soundsAudioSource = obj.AddComponent<AudioSource>();
        soundsAudioSource.loop = false;
        soundsAudioSource.playOnAwake = false;
        soundsAudioSource.volume = 1f;

        if(PlayerPrefs.HasKey(MusicEnabledKey))
            isMusicOn = (PlayerPrefs.GetInt(MusicEnabledKey) == 1);
    }

    public bool IsPlayingMusic()
    {
        if (musicAudioSource.clip != null)
        {
            return musicAudioSource.isPlaying;
        }
        return false;
    }

    public bool IsMusicLooped()
    {
        return musicAudioSource.loop;
    }

    public bool IsPlayingSoundFX()
    {
        return false;
    }

    public void LoopMusic(bool loop)
    {
        musicAudioSource.loop = loop;
    }

    public void PlayMusic()
    {
        if (isMusicOn && !IsPlayingMusic() && musicAudioSource.clip != null)
        {
            musicAudioSource.Play();
        }
    }

    public void PlayMusic(uint id)
    {
        ResourceManager.GetResource(AssetPathUtil.GetPrefab("Music/" + id), OnMusicLoaded);
    }

    private void OnMusicLoaded(object prefab)
    {
        AudioClip audioClip = (prefab as GameObject).GetComponent<AudioSource>().clip;
        PlayMusic(audioClip);
    }

    public void PlayMusic(AudioClip music)
    {
        if (music != null)
        {
            AudioClip oldMusic = GetMusicClip();
            if (music != oldMusic)
            {
                musicAudioSource.clip = music;
            }
            if (isMusicOn && music && (music != oldMusic || !IsPlayingMusic()))
            {
                musicAudioSource.Play();
            }
        }
    }

    public void PlaySound(IList<AudioClip> sounds)
    {
        if (sounds.Count > 0)
        {
            PlaySound(sounds[UnityEngine.Random.Range(0, sounds.Count)]);
        }
    }

    public void PlaySound(AudioClip soundFX)
    {
        PlaySound(soundFX, GetSoundFXVolume());
    }

    public void PlaySound(AudioClip soundFX, float volume)
    {
        if (soundfx && soundFX != null && soundsAudioSource != null)
        {
            soundsAudioSource.PlayOneShot(soundFX, volume);
        }
    }

    public void SetMusic(bool on)
    {
        bool isPlayingMusic = IsPlayingMusic();
        isMusicOn = on;
        if (on && !isPlayingMusic)
        {
            PlayMusic();
        }
        else if (!on && isPlayingMusic)
        {
            StopMusic();
        }

        PlayerPrefs.SetInt(MusicEnabledKey, on ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetSoundFX(bool on)
    {
        soundfx = on;
        PlayerPrefs.SetInt(SoundsEnabledKey, on ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = volume;
        }
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSoundFXVolume(float volume)
    {
        soundfxVolume = volume;
        PlayerPrefs.SetFloat(SoundsVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void StopMusic()
    {
        if (musicAudioSource.clip != null)
        {
            musicAudioSource.Stop();
        }
    }

    public void StopSounds()
    {
        soundsAudioSource.Stop();
    }
}
