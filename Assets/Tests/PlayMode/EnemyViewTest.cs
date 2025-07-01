using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyViewTest {
    private GameObject enemyObj;
    private GameObject playerObj;
    private ViewZone viewZone;
    private TestEnemyAttack testAttack;

    [SetUp]
    public void SetUp() {
        enemyObj = new GameObject("Enemy");
        var collider = enemyObj.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;

        viewZone = enemyObj.AddComponent<ViewZone>();
        testAttack = enemyObj.AddComponent<TestEnemyAttack>();
        viewZone.enemyAttack = testAttack;
        viewZone.enemyObj = enemyObj.transform;
        viewZone.viewRadius = 10f;
        viewZone.viewAngle = 180f;
        viewZone.targetMask = LayerMask.GetMask("Player");
        viewZone.barrierMask = 0;
        viewZone.rayOriginOffset = Vector2.zero;

        // Player setup
        playerObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        playerObj.name = "Player";
        playerObj.layer = LayerMask.NameToLayer("Player");
        playerObj.transform.position = enemyObj.transform.position + Vector3.right * 5f;
    }

    [UnityTest]
    public IEnumerator EnemyStartsAttackWhenPlayerInView() {
        yield return null;

        viewZone.InitializationOfTargets();
        viewZone.CheckVisibleTargets();

        Assert.IsTrue(testAttack.WasStartAttackCalled, "Enemy should start attack when player is in view");

        yield return null;
    }

    [UnityTest]
    public IEnumerator EnemyStopsAttackWhenPlayerLeavesView() {
        yield return null;
        viewZone.InitializationOfTargets();
        viewZone.CheckVisibleTargets();
        Assert.IsTrue(testAttack.WasStartAttackCalled);

        playerObj.transform.position = enemyObj.transform.position + Vector3.right * 20f;
        viewZone.InitializationOfTargets();
        viewZone.CheckVisibleTargets();

        Assert.IsFalse(testAttack.WasStartAttackCalled, "Enemy should stop attack when player leaves view");

        yield return null;
    }

    [TearDown]
    public void TearDown() {
        Object.DestroyImmediate(enemyObj);
        Object.DestroyImmediate(playerObj);
    }

    private class TestEnemyAttack : EnemyAttack {
        public bool WasStartAttackCalled { get; private set; }

        public override void StartAttack(Transform target)
        {
            WasStartAttackCalled = true;
        }

        public override void StopAttack()
        {
            WasStartAttackCalled = false;
        }
    }
}
