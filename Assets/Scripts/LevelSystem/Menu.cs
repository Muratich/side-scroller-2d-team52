using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Menu : MonoBehaviour {
    public void Quit() => Application.Quit();
    public void Singleplayer() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
