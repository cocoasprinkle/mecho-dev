using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown graphicsDropdown;
    private void Start()
    {
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        Debug.Log(graphicsDropdown.value);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("SavedGraphics"));
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("SavedGraphics", qualityIndex);
        PlayerPrefs.Save();
    }
}
