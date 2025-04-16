using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    [Header("Painel de Opções (apenas ele)")]
    public GameObject optionsPanel;

    [Header("Componentes de UI")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();

    void Start()
    {
        // Carrega resoluções disponíveis e remove duplicadas
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> added = new HashSet<string>();

        int defaultIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string res = resolutions[i].width + " x " + resolutions[i].height;

            if (!added.Contains(res))
            {
                added.Add(res);
                options.Add(res);
                filteredResolutions.Add(resolutions[i]);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    defaultIndex = filteredResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        // Carrega configurações salvas
        int savedIndex = PlayerPrefs.GetInt("resolutionIndex", defaultIndex);
        bool isFullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = isFullscreen;

        // Garante que o painel comece desativado
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void ApplySettings()
    {
        int index = resolutionDropdown.value;
        Resolution selected = filteredResolutions[index];
        bool isFullscreen = fullscreenToggle.isOn;

        Screen.SetResolution(selected.width, selected.height, isFullscreen);

        PlayerPrefs.SetInt("resolutionIndex", index);
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OpenOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }
}
