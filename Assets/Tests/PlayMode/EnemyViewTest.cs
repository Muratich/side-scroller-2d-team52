using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools;

public class EnemyViewTest
{
    private GameObject enemyObj;
    private GameObject playerObj;
    private ViewZone viewZone;
    private TestEnemyAttack testAttack;

    [SetUp]
    public void Setup()
    {
        // Create a mock enemy with ViewZone and EnemyAttack
        enemyObj = new GameObject("Enemy");
        viewZone = enemyObj.AddComponent<ViewZone>();
        testAttack = enemyObj.AddComponent<TestEnemyAttack>();
        viewZone.enemyAttack = testAttack;
        viewZone.enemyObj = enemyObj.transform;
        viewZone.viewRadius = 10f;
        viewZone.viewAngle = 180f;
        viewZone.targetMask = LayerMask.GetMask("Player");

        // Create a mock player
        playerObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        playerObj.name = "Player";
        playerObj.layer = LayerMask.NameToLayer("Player");
        playerObj.transform.position = enemyObj.transform.position + Vector3.right * 5f;
    }

    [Test]
    public void EnemyStartsAttackWhenPlayerInView()
    {
        // Simulate player entering view
        viewZone.InitializationOfTargets();
        viewZone.CheckVisibleTargets();

        // Assert that attack was triggered
        Assert.IsTrue(testAttack.WasStartAttackCalled);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyObj);
        Object.DestroyImmediate(playerObj);
    }

    // Test double for EnemyAttack
    private class TestEnemyAttack : EnemyAttack
    {
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
