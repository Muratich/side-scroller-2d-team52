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
        float value = 1f;
        if (PlayerPrefs.HasKey("MusicVolume")) value = PlayerPrefs.GetFloat("MusicVolume");
        masterMixer.SetFloat("MusicVolume", value);
        slider.onValueChanged.RemoveAllListeners();
        slider.value = value;

        slider.onValueChanged.AddListener(SetMasterVolume);

        profile.UpdateValues();
        playerName.text = profile.GetPlayerName();
        maxReachedLevel.text = profile.GetMaxReachedLevel().ToString();
    }

    public void MakeFullscreen() {
        Screen.fullScreen = true;
        Screen.SetResolution(1920, 1080, true);
    }

    public void ClearProgress() {
        profile.DeleteData();
        playerName.text = profile.GetPlayerName();
        maxReachedLevel.text = profile.GetMaxReachedLevel().ToString();
    }

    public void SetMasterVolume(float value) {
        masterMixer.SetFloat("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetNewPlayerName() {
        profile.SetPlayerName(newPlayerNameField.text);
        playerName.text = newPlayerNameField.text;
    }

    public void Quit() => Application.Quit();
}
