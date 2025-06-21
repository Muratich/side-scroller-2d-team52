using System.Collections;
using UnityEngine;

public class EnemyAttackMelee : EnemyAttack {
    public GameObject hitZone;
    private Coroutine hitCor;

    
    public void Awake() {
        if (animator == null) Debug.LogError("Animator was not added to:" + gameObject.name);
    }


    public override void StartAttack(Transform target) {
        if (hitCor != null) return;
        hitCor = StartCoroutine(Hit());
        animator.SetBool("attack", true);
    }

    public override void StopAttack() {
        if (hitCor != null) {
            StopCoroutine(hitCor);
            hitCor = null;
        }
        animator.SetBool("attack", false);
    }

    IEnumerator Hit() {
        while (true) {
            GameObject hitZoneObj = Instantiate(hitZone, transform.position, Quaternion.identity);
            Destroy(hitZoneObj, 0.5f);
            yield return new WaitForSeconds(attackReload);
        }   
    }
}
