using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSettings : MonoBehaviour {
    public static InGameSettings Instance;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnDisconnectButtonPressed() {
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening) {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            return;
        }

        if (NetworkManager.Singleton.IsHost) {
            NetworkManager.Singleton.Shutdown();
        }
        else if (NetworkManager.Singleton.IsClient) {
            NetworkManager.Singleton.Shutdown();
        }

        if (NetworkManager.Singleton.gameObject != null)
            Destroy(NetworkManager.Singleton.gameObject);

        GameObject profile = FindAnyObjectByType<Profile>().gameObject;
        if (profile != null) Destroy(profile);

        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
