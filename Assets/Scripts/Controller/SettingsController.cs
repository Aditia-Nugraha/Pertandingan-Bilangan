using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void OnEnable()
    {
        _musicSlider.SetValueWithoutNotify(AudioManager.Instance.MusicVolume);
        _sfxSlider.SetValueWithoutNotify(AudioManager.Instance.SfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        AudioManager.Instance.SetSfxVolume(volume);
    }
}