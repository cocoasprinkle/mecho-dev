using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer musicMixer;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 100));
    }

    public void SetVolume(float value)
    {
        if (value < 1)
        {
            value = 0.001f;
        }
        RefreshSlider(value);
        PlayerPrefs.SetFloat("SavedMusicVolume", value);
        musicMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(musicSlider.value);
        PlayerPrefs.Save();
    }

    public void RefreshSlider(float value)
    {
        musicSlider.value = value;
    }
}
