using NUnit.Framework;
using UnityEngine;

public class CameraFollowTests {
    [Test]
    public void LateUpdate_WithoutTarget_DoesNotMove() {
        var camGO = new GameObject();
        var cf = camGO.AddComponent<CameraFollow>();

        camGO.transform.position = Vector3.zero;
        cf.isMoving = false;
        cf.LateUpdate();
        Assert.AreEqual(Vector3.zero, camGO.transform.position);
    }
}
