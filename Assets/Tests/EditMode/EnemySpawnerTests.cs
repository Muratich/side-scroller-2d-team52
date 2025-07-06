// EnemySpawnerTests.cs
using NUnit.Framework;
using UnityEngine;

public class EnemySpawnerTests {
    [Test]
    public void SpawnAll_WithEmptyList_DoesNotThrow() {
        var go = new GameObject();
        var sp = go.AddComponent<EnemySpawner>();
        sp.enemiesInfo = new System.Collections.Generic.List<EnemySpawnInfo>();
        Assert.DoesNotThrow(() => sp.SpawnAll());
    }
}
