using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [Header("Settings UI")]
    public AudioMixer masterMixer;
    public Slider slider;

    [Header("Profile")]
    public TMP_Text playerName;
    public TMP_Text maxReachedLevel;
    public Profile profile;
    public TMP_InputField newPlayerNameField;

    void Start() {
        Screen.fullScreen = false;
        float sliderValue = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float dB = Mathf.Lerp(-80f, 0f, sliderValue);

        masterMixer.SetFloat("MusicVolume", dB);

        slider.onValueChanged.RemoveAllListeners();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = sliderValue;

        slider.onValueChanged.AddListener(SetMasterVolume);
        profile.UpdateValues();
        playerName.text = profile.GetPlayerName();
        maxReachedLevel.text = profile.GetMaxReachedLevel().ToString();
    }

    public void ClearProgress() {
        profile.DeleteData();
        playerName.text = profile.GetPlayerName();
        maxReachedLevel.text = profile.GetMaxReachedLevel().ToString();
    }
    public void SetMasterVolume(float sliderValue) {
        float dB = Mathf.Lerp(-80f, 0f, sliderValue);
        Debug.Log($"[Audio] Slider={sliderValue:F2} → dB={dB:F1}");  // вот это
        masterMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        PlayerPrefs.Save();
    }


    public void SetNewPlayerName() {
        profile.SetPlayerName(newPlayerNameField.text);
        playerName.text = newPlayerNameField.text;
    }

    public void Quit() => Application.Quit();
}
