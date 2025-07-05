using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SniperWeapon : Weapon {
    public float bulletSpeed = 3;
    private bool isReloaded = true;

    public override void Attack() {
        if (!isReloaded || scaleRef == null) return;

        GameObject projObj = Instantiate(proj, firePos.position, Quaternion.identity);

        if (projObj.TryGetComponent<NetworkObject>(out NetworkObject projNet))
            projNet.Spawn(destroyWithScene: true);
        if (projObj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            rb.linearVelocity = new Vector2(scaleRef.localScale.x * bulletSpeed, 0);
        StartCoroutine(DeleteBullet(projNet));
        StartCoroutine(Reload());
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }

    IEnumerator DeleteBullet(NetworkObject projNet) {
        yield return new WaitForSeconds(4);
        projNet.Despawn(proj);
    }
}
