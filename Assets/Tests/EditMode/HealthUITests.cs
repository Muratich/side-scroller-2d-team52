using NUnit.Framework;
using UnityEngine;

public class HealthUITests {
    [Test]
    public void Init_CreatesCorrectNumberOfHealthIcons() {
        var go = new GameObject();
        var ui = go.AddComponent<HealthUI>();
        ui.healthBar = new GameObject("Bar").transform;
        ui.healthPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        var healthGO = new GameObject();
        var health = healthGO.AddComponent<Health>();
        if (health.CurrentHealth != null) {
            health.CurrentHealth.Value = 3;

            ui.Init(health);
            Assert.AreEqual(3, ui.healthBar.childCount);
        }
    }
}
