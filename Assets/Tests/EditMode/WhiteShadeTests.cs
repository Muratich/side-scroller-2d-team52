using NUnit.Framework;
using UnityEngine;

public class WhiteShadeTests {
    [Test]
    public void Shade_ImmediatelyChangesColorToShade() {
        var go = new GameObject();
        WhiteShade ws = go.AddComponent<WhiteShade>();
        var srGO = new GameObject();
        var sr = srGO.AddComponent<SpriteRenderer>();
        sr.color = Color.green;
        ws.spriteRenderer = sr;

        ws.Awake();
        ws.Shade();
        Assert.AreEqual(Color.red, sr.color);
    }
}
