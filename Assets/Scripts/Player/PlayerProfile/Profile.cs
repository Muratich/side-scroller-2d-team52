using UnityEngine;
using UnityEngine.SceneManagement;

public class Profile : MonoBehaviour {
    private string playerName;
    private int maxReachedLevel = 1;

    void Start() {
        DontDestroyOnLoad(gameObject);
        UpdateValues();
    }

    public void SetPlayerName(string newName) {
        if (newName.Length > 15) return;

        playerName = newName;
        PlayerPrefs.SetString("name", playerName);
    }

    public void SetMaxReachedLevel(int levelIndex) {
        if (SceneManager.sceneCount >= levelIndex || levelIndex < 0) return;

        maxReachedLevel = levelIndex;
        PlayerPrefs.SetInt("level", levelIndex);
    }

    public string GetPlayerName() {
        UpdateValues();
        return playerName;
    }

    public int GetMaxReachedLevel() {
        UpdateValues();
        return maxReachedLevel;
    }

    public void UpdateValues() {
        if (PlayerPrefs.HasKey("name")) playerName = PlayerPrefs.GetString("name");
        else playerName = "";

        if (PlayerPrefs.HasKey("level")) maxReachedLevel = PlayerPrefs.GetInt("level");
        else maxReachedLevel = 1;
    }

    public void DeleteData() {
        if (PlayerPrefs.HasKey("name")) PlayerPrefs.DeleteKey("name");
        if (PlayerPrefs.HasKey("level")) PlayerPrefs.DeleteKey("level");
        UpdateValues();
    }
}
