using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class EnemyAttackRange : EnemyAttack {
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private Coroutine fireCor;

    public void Awake() {
        if (animator == null) Debug.LogError("Animator was not added to:" + gameObject.name);
        if (bulletPrefab == null) Debug.LogError("Bullet was not added to:" + gameObject.name);
    }

    protected override void StartAttack(Vector3 targetPosition) {
        if (fireCor != null) return;
        currTargetPos = targetPosition;
        fireCor = StartCoroutine(Fire());
        PlayAttack(true);
    }

    protected override void StopAttack() {
        if (fireCor != null) {
            StopCoroutine(fireCor);
            fireCor = null;
        }
        PlayAttack(false);
    }

    IEnumerator Fire() {
        while (true) {
            Vector2 dir = (currTargetPos - transform.position).normalized;
            GameObject proj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            if (proj.TryGetComponent<NetworkObject>(out NetworkObject projNet))
                projNet.Spawn();
            if (proj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                rb.linearVelocity = dir * bulletSpeed;

            Destroy(proj, 5f);
            yield return new WaitForSeconds(attackReload);
        }
    }
}