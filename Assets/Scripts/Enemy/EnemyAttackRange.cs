using System.Collections;
using UnityEngine;

public class EnemyAttackRange : EnemyAttack {
    [Header("Characteristics")]
    public int fireRatePerMinute = 30;
    public GameObject bullet;
    public float bulletSpeed;
    private Coroutine fireCor;
    private Transform currTarget;

    public void Awake() {
        if (animator == null) Debug.LogError("Animator was not added to:" + gameObject.name);
        if (bullet == null) Debug.LogError("Bullet was not added to:" + gameObject.name);
    }

    public override void StartAttack(Transform target) {
        if (fireCor != null) return;
        currTarget = target;
        fireCor = StartCoroutine(Fire());
        animator.SetBool("attack", true);
    }

    public override void StopAttack() {
        if (fireCor != null) {
            StopCoroutine(fireCor);
            fireCor = null;
        }
        animator.SetBool("attack", false);
        currTarget = null;
    }

    IEnumerator Fire() {
        while (currTarget != null) {
            Vector2 dir = (currTarget.position - transform.position).normalized;
            GameObject proj = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            Destroy(proj, 5f);
            if (rb != null) {
                rb.linearVelocity = dir * bulletSpeed;
            }
            yield return new WaitForSeconds(attackReload);
        }
    }
}