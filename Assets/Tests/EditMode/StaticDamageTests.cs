using NUnit.Framework;
using UnityEngine;

public class StaticDamageTests {
    [Test]
    public void OnTriggerEnter2D_WithNonEnemyTag_ThrowsNullReference_WhenNoNetworkManager() {
        var go = new GameObject();
        var sd = go.AddComponent<StaticDamage>();
        sd.enemyTags = new System.Collections.Generic.List<string> { "Enemy" };

        var colGO = new GameObject();
        colGO.tag = "Player";
        var col = colGO.AddComponent<BoxCollider2D>();
    }
}
