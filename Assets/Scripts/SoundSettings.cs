using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer musicMixer;

    private void Start()
    {
        // Checks what sound volume setting the player is currently using
        SetVolume(PlayerPrefs.GetFloat("SavedSoundVolume", 100));
    }

    public void SetVolume(float value)
    {
        // Sets the sound volume setting to the value of the slider, refreshing the slider's value and updating it when called
        if (value < 1)
        {
            value = 0.001f;
        }
        RefreshSlider(value);
        PlayerPrefs.SetFloat("SavedSoundVolume", value);
        musicMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
        PlayerPrefs.Save();
    }

    public void RefreshSlider(float value)
    {
        soundSlider.value = value;
    }
}