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
        if (hitCor != null || hitZone == null) return;
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
            GameObject hitZoneObj = Instantiate(hitZone, attackPos.position, Quaternion.identity);

            Vector3 scaleMul = hitZoneObj.transform.localScale;
            hitZoneObj.transform.localScale = new Vector3(transform.localScale.x > 0 ? scaleMul.x : -scaleMul.x, scaleMul.y, scaleMul.z);
            hitZoneObj.transform.position += new Vector3(1.5f * transform.localScale.x, 0, 0);
            
            if (hitZoneObj.TryGetComponent<NetworkObject>(out NetworkObject hitZoneNet))
                hitZoneNet.Spawn();
            Destroy(hitZoneObj, 0.5f);
            yield return new WaitForSeconds(attackReload);
        }
    }
}
