using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthTest : MonoBehaviour
{
    private GameObject go;
        private Health health;
        private bool damageCalled;
        private bool healCalled;
        private bool dieCalled;

        [SetUp]
        public void SetUp() {
            go = new GameObject("HealthGO");
            health = go.AddComponent<Health>();
            health.health = 3;
            health.invisibilityTime = 0.01f;
            health.onDamage = new UnityEngine.Events.UnityEvent();
            health.onHeal = new UnityEngine.Events.UnityEvent();
            health.onDie = new UnityEngine.Events.UnityEvent();

            health.onDamage.AddListener(() => damageCalled = true);
            health.onHeal.AddListener(() => healCalled = true);
            health.onDie.AddListener(() => dieCalled = true);
        }

        [UnityTest]
        public IEnumerator TakeDamage_InvokesEventsAndRespectsInvisibility() {
            health.TakeDamage(1);
            Assert.IsTrue(damageCalled, "onDamage should be invoked");
            Assert.AreEqual(2, health.health);

            damageCalled = false;
            health.TakeDamage(1);
            Assert.IsFalse(damageCalled, "Should be invisible immediately");

            yield return new WaitForSeconds(0.02f);
            health.TakeDamage(2);
            Assert.IsTrue(damageCalled, "onDamage after invisibility");
            Assert.IsTrue(dieCalled, "onDie should fire when health <= 0");
        }

        [Test]
        public void Heal_DoesNotExceedMax_InvokesEvent() {
            health.health = 1;
            health.Heal(5);
            Assert.IsTrue(healCalled);
            Assert.AreEqual(3, health.health, "Health should not exceed maxHealth");
        }
}
