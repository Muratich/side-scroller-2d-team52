using NUnit.Framework;
using UnityEngine;

public class HealthUITest : MonoBehaviour
{
    private GameObject uiGO;
    private HealthUI uiComponent;
    private GameObject healthGO;

    [SetUp]
    public void SetUp() {
        healthGO = new GameObject("Player");
        healthGO.tag = "PlayerTag";
        var health = healthGO.AddComponent<Health>();
        health.health = 2;

        uiGO = new GameObject("HealthUI");
        uiComponent = uiGO.AddComponent<HealthUI>();
        uiComponent.targetTag = "PlayerTag";

        var bar = new GameObject("Bar");
        uiComponent.healthBar = bar.transform;
        uiComponent.healthPrefab = new GameObject("Pref");

        uiComponent.Awake();
    }

    [Test]
    public void Awake_InitializesCorrectNumberOfHearts() {
        Assert.AreEqual(2, uiComponent.healthBar.childCount);
    }

    [Test]
    public void OnDamage_UpdatesUI() {
        var health = healthGO.GetComponent<Health>();
        health.health = 1;
        health.onDamage.Invoke();
        Assert.AreEqual(1, uiComponent.healthBar.childCount);
    }
}
