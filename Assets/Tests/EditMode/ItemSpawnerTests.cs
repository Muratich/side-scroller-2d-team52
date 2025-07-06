using NUnit.Framework;
using UnityEngine;

public class ItemsSpawnerTests {
    [Test]
    public void Spawn_WithEmptyList_DoesNotThrow() {
        var go = new GameObject();
        var sp = go.AddComponent<ItemsSpawner>();
        sp.items = new System.Collections.Generic.List<ItemInfo>();
        Assert.DoesNotThrow(() => sp.Spawn());
    }
}
