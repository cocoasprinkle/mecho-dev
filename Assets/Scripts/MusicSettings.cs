using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer musicMixer;

    private void Start()
    {
        // Checks what music volume setting the player is currently using
        SetVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 100));
    }

    public void SetVolume(float value)
    {
        // Sets the music volume setting to the value of the slider, refreshing the slider's value and updating it when called
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
