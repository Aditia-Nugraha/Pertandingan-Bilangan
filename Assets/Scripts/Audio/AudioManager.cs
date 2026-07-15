using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    public float MusicVolume => _musicSource.volume;
    public float SfxVolume => _sfxSource.volume;

    [Header("Audio Source")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    public AudioSource MusicSource => _musicSource;
    public AudioSource SfxSource => _sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip _backgroundMusic;

    [Header("Sound Effects")]
    [SerializeField] private List<SfxData> _sfxLibrary = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void Initialize()
    {
        LoadVolume();
        _musicSource.clip = _backgroundMusic;
        _musicSource.Play();
    }

    public void PlayButtonClick()
    {
        PlaySfx(GameSfx.ButtonClick);
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        _musicSource.volume = musicVolume;
        _sfxSource.volume = sfxVolume;
    }

    private AudioClip GetSfxClip(GameSfx type)
    {
        foreach (SfxData data in _sfxLibrary)
        {
            if (data.Type == type)
            {
                return data.Clip;
            }
        }

        return null;
    }

    public void PlaySfx(GameSfx type)
    {
        AudioClip clip = GetSfxClip(type);

        if (clip == null)
        {
            return;
        }

        _sfxSource.PlayOneShot(clip);
    }

    public void PauseMusic()
    {
        _musicSource.Pause();
    }

    public void ResumeMusic()
    {
        _musicSource.UnPause();
    }
}