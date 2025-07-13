using System;
using NUnit.Framework;
using UnityEngine;

public class WinTriggerTests {
    [Test]
    public void OnTriggerEnter2D_WithPlayerTag_ThrowsInvalidOperationException() {
        var trigger = new GameObject().AddComponent<WinTrigger>();
        var col = new GameObject();
        col.tag = "Player";
        var collider = col.AddComponent<BoxCollider2D>();
        Assert.Throws<InvalidOperationException>(() => trigger.OnTriggerEnter2D(collider));
    }
}
