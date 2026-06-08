using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Sahne ve Pause Ayarları")]
    public GameObject optionsPanel; 
    public bool canPauseWithESC = false;

    [Header("Audio Mixer Ana Bağlantısı")]
    public AudioMixer mainMixer;

    [Header("3 Farklı Ses Slider'ı")]
    public Slider ambienceSlider;
    public Slider minigameSlider;
    public Slider uiSlider;

    [Header("Çözünürlük Elemanları")]
    public TMP_Text resolutionText; 
    
    private List<Resolution> filteredResolutions; 
    private int currentResolutionIndex = 0;

    void Awake()
    {
        float savedAmb = PlayerPrefs.GetFloat("AmbienceVolume", 0.5f);
        float savedMini = PlayerPrefs.GetFloat("MinigameVolume", 0.5f);
        float savedUI = PlayerPrefs.GetFloat("UiVolume", 0.5f);

        if (ambienceSlider != null) { ambienceSlider.value = savedAmb; ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume); }
        if (minigameSlider != null) { minigameSlider.value = savedMini; minigameSlider.onValueChanged.AddListener(SetMinigameVolume); }
        if (uiSlider != null) { uiSlider.value = savedUI; uiSlider.onValueChanged.AddListener(SetUiVolume); }
    }

    void Start()
    {
        SetAmbienceVolume(PlayerPrefs.GetFloat("AmbienceVolume", 0.5f));
        SetMinigameVolume(PlayerPrefs.GetFloat("MinigameVolume", 0.5f));
        SetUiVolume(PlayerPrefs.GetFloat("UiVolume", 0.5f));

        InitResolutions();
    }

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log($"<color=yellow>ESC TUŞUNA BASILDI!</color> canPauseWithESC Durumu: {canPauseWithESC}");
        }

        if (canPauseWithESC && Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsPanel.activeSelf)
                CloseSettings(); 
            else
                OpenSettings(); 
        }
    }

    public void OpenSettings()
    {
        if (optionsPanel != null) optionsPanel.SetActive(true);
        
        
        if (canPauseWithESC) Time.timeScale = 0f; 
    }

    public void CloseSettings()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        
        
        if (canPauseWithESC) Time.timeScale = 1f; 
    }

    
    public void SetAmbienceVolume(float value)
    {
        ApplyLogVolume("AmbienceVol", value);
        PlayerPrefs.SetFloat("AmbienceVolume", value);
        PlayerPrefs.Save();
    }

    public void SetMinigameVolume(float value)
    {
        ApplyLogVolume("MinigameVol", value);
        PlayerPrefs.SetFloat("MinigameVolume", value);
        PlayerPrefs.Save();
    }

    public void SetUiVolume(float value)
    {
        ApplyLogVolume("UiVol", value);
        PlayerPrefs.SetFloat("UiVolume", value);
        PlayerPrefs.Save();
    }

    void ApplyLogVolume(string exposedParam, float sliderValue)
    {
        if (mainMixer == null) return;
        if (sliderValue <= 0.0001f) mainMixer.SetFloat(exposedParam, -80f);
        else mainMixer.SetFloat(exposedParam, Mathf.Log10(sliderValue / 0.5f) * 20f);
    }

    void InitResolutions()
    {
        Resolution[] allResolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            bool isDuplicate = false;
            for (int j = 0; j < filteredResolutions.Count; j++)
            {
                if (allResolutions[i].width == filteredResolutions[j].width && allResolutions[i].height == filteredResolutions[j].height)
                {
                    isDuplicate = true; break;
                }
            }
            if (!isDuplicate) filteredResolutions.Add(allResolutions[i]);
        }

        int savedResIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
        if (savedResIndex != -1 && savedResIndex < filteredResolutions.Count) currentResolutionIndex = savedResIndex;
        else
        {
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                if (filteredResolutions[i].width == Screen.currentResolution.width && filteredResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i; break;
                }
            }
        }
        UpdateResolutionUI();
    }

    public void NextResolution()
    {
        if (filteredResolutions == null || filteredResolutions.Count <= 1) return;
        currentResolutionIndex++;
        if (currentResolutionIndex >= filteredResolutions.Count) currentResolutionIndex = 0; 
        ApplyAndSaveResolution();
    }

    public void PreviousResolution()
    {
        if (filteredResolutions == null || filteredResolutions.Count <= 1) return;
        currentResolutionIndex--;
        if (currentResolutionIndex < 0) currentResolutionIndex = filteredResolutions.Count - 1; 
        ApplyAndSaveResolution();
    }

    void ApplyAndSaveResolution()
    {
        if (filteredResolutions == null || filteredResolutions.Count == 0) return;
        Resolution selectedRes = filteredResolutions[currentResolutionIndex];
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);
        PlayerPrefs.Save();
        UpdateResolutionUI();
    }

    void UpdateResolutionUI()
    {
        if (resolutionText != null && filteredResolutions != null && filteredResolutions.Count > 0)
        {
            Resolution res = filteredResolutions[currentResolutionIndex];
            resolutionText.text = res.width + " x " + res.height;
        }
    }
}