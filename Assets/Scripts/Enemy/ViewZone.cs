using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class ViewZone : NetworkBehaviour {
    [Header("Vision Settings")]
    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask targetMask, barrierMask;
    public float updateInterval = 0.1f;
    public Vector2 rayOriginOffset = Vector2.zero;

    [Header("References")]
    public Transform enemyObj;
    public EnemyAttack enemyAttack;

    private List<Transform> targetsInRange = new ();
    private HashSet<Transform> previouslyVisible = new HashSet<Transform>();
    public bool HasVisibleTargets => previouslyVisible.Count > 0;

    private void Awake() {
        if (enemyAttack == null) Debug.LogError("EnemyAttack script was not founded in " + gameObject.name);
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        circleCollider2D.radius = viewRadius;
    }

    public override void OnNetworkSpawn() {
        if (!IsServer) return;
        StartCoroutine(CheckWithPeriod());
    }

    IEnumerator CheckWithPeriod() {
        yield return null;
        while (true) {
            yield return new WaitForSeconds(updateInterval);
            CheckVisibleTargets();
        }
    }

    public void CheckVisibleTargets() {
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

            bool hitBarrier = Physics2D.Raycast(origin, dirToTarget, distToTarget, barrierMask);
            bool inSight = angleToTarget <= viewAngle / 2f && !hitBarrier;

            if (inSight) {
                if (!previouslyVisible.Contains(t)) {
                    previouslyVisible.Add(t);
                    enemyAttack.StartAttackServerRpc(t.position);
                }
                else {
                    enemyAttack.UpdateTargetPositionServerRpc(t.position);
                }
            }
            else if (!inSight && previouslyVisible.Contains(t)) {
                previouslyVisible.Remove(t);
                enemyAttack.StopAttackServerRpc();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!IsServer) return;
        if (((1 << other.gameObject.layer) & targetMask) != 0) {
            if (!targetsInRange.Contains(other.transform))
                targetsInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!IsServer) return;
        if (((1 << other.gameObject.layer) & targetMask) != 0) {
            if (targetsInRange.Remove(other.transform)) {
                if (previouslyVisible.Remove(other.transform)) {
                    enemyAttack.StopAttackServerRpc();
                }
            }
        }
    }

   public Transform GetClosestVisibleTarget() {
        if (previouslyVisible.Count == 0)
            return null;

        Transform closest = null;
        float minDist = float.MaxValue;
        Vector3 origin = enemyObj.position;

        foreach (var t in previouslyVisible) {
            if (t == null) 
                continue;

            float dist = Vector3.Distance(origin, t.position);
            if (dist < minDist) {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }
}
