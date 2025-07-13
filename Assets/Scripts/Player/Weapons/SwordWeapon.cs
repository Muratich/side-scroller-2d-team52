using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SwordWeapon : Weapon {
    private bool isReloaded = true;

    public override void Attack() {
        if (!isReloaded || scaleRef == null) return;

        isReloaded = false;
        GameObject projObj = Instantiate(proj, firePos.position, Quaternion.identity);

        if (projObj.TryGetComponent<NetworkObject>(out NetworkObject projNet))
            projNet.Spawn(destroyWithScene: true);
        StartCoroutine(DeleteBullet(projNet));
        StartCoroutine(Reload());
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }

    IEnumerator DeleteBullet(NetworkObject projNet) {
        yield return new WaitForSeconds(0.3f);
        projNet.Despawn(proj);
    }
}
