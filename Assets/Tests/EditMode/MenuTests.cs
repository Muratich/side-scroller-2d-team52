using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTests {
    [Test]
    public void Singleplayer_LoadsNextScene() {
        var menu = new GameObject().AddComponent<Menu>();
        int before = SceneManager.GetActiveScene().buildIndex;
    }

    [Test]
    public void Quit_DoesNotThrow() {
        var menu = new GameObject().AddComponent<Menu>();
        Assert.DoesNotThrow(() => menu.Quit());
    }
}
