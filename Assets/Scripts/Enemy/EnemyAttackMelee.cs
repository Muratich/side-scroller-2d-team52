using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class EnemyAttackMelee : EnemyAttack {
    public GameObject hitZone;
    private Coroutine hitCor;


    public void Awake() {
        if (animator == null) Debug.LogError("Animator was not added to:" + gameObject.name);
    }

    protected override void StartAttack(Vector3 targetPos) {
        if (hitCor != null) return;
        hitCor = StartCoroutine(HitLoop());
        PlayAttack(true);
    }

    protected override void StopAttack() {
        if (hitCor != null) {
            StopCoroutine(hitCor);
            hitCor = null;
        }
        PlayAttack(false);
    }

    IEnumerator HitLoop() {
        while (true) {
            GameObject hitZoneObj = Instantiate(hitZone, transform.position, Quaternion.identity);
            if (hitZoneObj.TryGetComponent<NetworkObject>(out NetworkObject hitZoneNet))
                hitZoneNet.Spawn();
            Destroy(hitZoneObj, 0.5f);
            yield return new WaitForSeconds(attackReload);
        }
    }
}
