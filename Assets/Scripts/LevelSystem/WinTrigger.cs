using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour {
    public string nextScene;
    public void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;
        
        Profile sceneProfile = FindAnyObjectByType<Profile>();
        if (sceneProfile == null) { Debug.LogError("Profile in scene not founded!"); return; }
        sceneProfile.SetMaxReachedLevel(SceneManager.GetActiveScene().buildIndex);
        
        if (nextScene != null && nextScene != "Menu") {
            NetworkManager.Singleton.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        else if (nextScene == "Menu") {
            InGameSettings sett = FindFirstObjectByType<InGameSettings>();
            if (sett != null) sett.OnDisconnectButtonPressed();
        }
        else Debug.LogError("Setted scene does not exist!");
    }
}
