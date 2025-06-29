using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class ViewZone : MonoBehaviour {
    [Header("Characteristics")]
    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask targetMask;
    public LayerMask barrierMask;
    public float updateInterval = 0.1f;
    public Vector2 rayOriginOffset = Vector2.zero;
    public Transform enemyObj;
    public EnemyAttack enemyAttack;

    private List<Transform> targetsInRange = new List<Transform>();
    private List<Transform> visibleTargets = new List<Transform>();
    private CircleCollider2D circleCollider2D;
    private HashSet<Transform> previouslyVisible = new HashSet<Transform>();

    private void Awake() {
        if (enemyAttack == null) Debug.LogError("EnemyAttack script was not founded in " + gameObject.name);
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = viewRadius;
    }

    private void OnEnable() {
        InitializationOfTargets();
        StartCoroutine(CheckWithPeriod());
    }

    private void OnDisable() => StopAllCoroutines();

    public void InitializationOfTargets() {
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position, viewRadius, targetMask);
        foreach (Collider2D hit in hits) {
            if (!targetsInRange.Contains(hit.transform)) {
                targetsInRange.Add(hit.transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (((1 << other.gameObject.layer) & targetMask) != 0) {
            if (!targetsInRange.Contains(other.transform)) {
                targetsInRange.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (((1 << other.gameObject.layer) & targetMask) != 0) {
            Transform i = other.transform;
            if (targetsInRange.Contains(i)) {
                targetsInRange.Remove(i);
                if (previouslyVisible.Contains(i)) {
                    previouslyVisible.Remove(i);
                    OnTargetLost(i);
                }
                visibleTargets.Remove(i);
            }
        }
    }

    IEnumerator CheckWithPeriod() {
        yield return null;
        while (true) {
            yield return new WaitForSeconds(updateInterval);
            CheckVisibleTargets();
        }
    }

    public void CheckVisibleTargets() {
        visibleTargets.Clear();
        Vector2 origin = (Vector2)transform.position + rayOriginOffset;
        for (int i = targetsInRange.Count - 1; i >= 0; i--) {
            Transform t = targetsInRange[i];
            if (t == null) {
                targetsInRange.RemoveAt(i);
                previouslyVisible.Remove(t);
                continue;
            }
            Vector2 dirToTarget = ((Vector2)t.position - origin).normalized;
            float distToTarget = Vector2.Distance(origin, (Vector2)t.position);

            Vector2 facingDir = new Vector2(enemyObj.localScale.x > 0 ? 1f : -1f, 0f);
            float angleToTarget = Vector2.Angle(facingDir, dirToTarget);
            if (angleToTarget <= viewAngle / 2f) {
                int mask = targetMask | barrierMask;
                RaycastHit2D hit = Physics2D.Raycast(origin, dirToTarget, distToTarget, mask);
                if (hit) {
                    if (((1 << hit.collider.gameObject.layer) & targetMask) != 0) {
                        visibleTargets.Add(t);
                        if (!previouslyVisible.Contains(t)) {
                            previouslyVisible.Add(t);
                            OnTargetVisible(t);
                        }
                    }
                    else {
                        if (previouslyVisible.Contains(t)) {
                            previouslyVisible.Remove(t);
                            OnTargetHidden(t);
                        }
                    }
                }
                else {
                    if (previouslyVisible.Contains(t)) {
                        previouslyVisible.Remove(t);
                        OnTargetHidden(t);
                    }
                }
            }
            else {
                if (previouslyVisible.Contains(t)) {
                    previouslyVisible.Remove(t);
                    OnTargetHidden(t);
                }
            }
        }
    }

    public bool HasVisibleTargets => visibleTargets.Count > 0;
    
    protected virtual void OnTargetVisible(Transform target) {
        enemyAttack.StartAttack(target);
    }

    protected virtual void OnTargetHidden(Transform target) {
        enemyAttack.StopAttack();
    }

    protected virtual void OnTargetLost(Transform target) {
        enemyAttack.StopAttack();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 origin = transform.position;
        Vector2 facingDir = new Vector2(enemyObj.localScale.x > 0 ? 1f : -1f, 0f);
        float halfAngle = viewAngle / 2f;

        Quaternion leftRot = Quaternion.Euler(0, 0, -halfAngle);
        Quaternion rightRot = Quaternion.Euler(0, 0, halfAngle);
        Vector2 leftDir = leftRot * facingDir;
        Vector2 rightDir = rightRot * facingDir;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + (Vector3)leftDir.normalized * viewRadius);
        Gizmos.DrawLine(origin, origin + (Vector3)rightDir.normalized * viewRadius);
    }
}
