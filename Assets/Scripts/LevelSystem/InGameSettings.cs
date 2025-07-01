using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSettings : MonoBehaviour {
    public void DisconnectToMenu() {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
