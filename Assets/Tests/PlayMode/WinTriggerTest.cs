using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class WinTriggerTests {
        private GameObject triggerObj;
        private WinTrigger winTrigger;

        [SetUp]
        public void SetUp()
        {
            triggerObj = new GameObject("WinTrigger");
            var col = triggerObj.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            winTrigger = triggerObj.AddComponent<WinTrigger>();
        }

        [UnityTest]
        public IEnumerator OnDeath_ReloadsCurrentScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            winTrigger.OnDeath();
            yield return null;
            Assert.AreEqual(currentIndex, SceneManager.GetActiveScene().buildIndex);
        }

        [UnityTest]
        public IEnumerator OnTriggerEnter2D_PlayerLoadsScene0() {
            int initialIndex = SceneManager.GetActiveScene().buildIndex;
            Assert.AreNotEqual(0, initialIndex, "Test scene should not be 0");

            var player = new GameObject("Player");
            player.tag = "Player";
            var playerCol = player.AddComponent<BoxCollider2D>();
            player.AddComponent<Rigidbody2D>();
            player.transform.position = triggerObj.transform.position;

            winTrigger.OnTriggerEnter2D(playerCol);
            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(0, SceneManager.GetActiveScene().buildIndex);
        }
    }
