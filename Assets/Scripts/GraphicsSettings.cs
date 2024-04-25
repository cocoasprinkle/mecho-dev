using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown graphicsDropdown;
    private void Start()
    {
        // Checks what graphics setting the player is currently using
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("SavedGraphics"));
    }
    public void SetQuality(int qualityIndex)
    {
        // When called by a button, the graphics setting is changed depending on the option chosen in the dropdown, represented by an int
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("SavedGraphics", qualityIndex);
        PlayerPrefs.Save();
    }
}
