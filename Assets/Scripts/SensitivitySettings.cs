using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedSensitivity", 100));
    }

    public void SetVolume(float value)
    {
        if (value < 1)
        {
            value = 0.001f;
        }
        RefreshSlider(value);
        PlayerPrefs.SetFloat("SavedSensitivity", value);
    }

    public void SetSensitivityFromSlider()
    {
        SetVolume(sensitivitySlider.value);
    }

    public void RefreshSlider(float value)
    {
        sensitivitySlider.value = value;
    }
}
