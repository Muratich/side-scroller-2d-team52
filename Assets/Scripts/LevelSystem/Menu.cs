using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public void Quit() => Application.Quit();
    public void Singleplayer() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
