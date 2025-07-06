using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SpawnEffectTests {
    [Test]
    public void Spawn_WithNullEffect_ThrowsArgumentException_AfterLoggingError() {
        var go = new GameObject();
        var se = go.AddComponent<SpawnEffect>();
        se.effect = null;

        LogAssert.Expect(LogType.Error, "Effect prefab does not set!");
        Assert.Throws<ArgumentException>(() => se.Spawn());
    }
}
