using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class EnemyPatternMovement : NetworkBehaviour {
    [Tooltip("The points where the enemy will move sequentially")]
    public List<Transform> destinationPoints;

    [Header("Characteristics")]
    public float speed = 10;
    public float timeToStayAtPoint = 5;

    [Header("Dash")]
    public bool isDasher = false;
    public float dashSpeed = 20f;
    public float dashTime = 1f;
    private float statDashTime;


    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public ViewZone viewZone;


    public void Init(List<Transform> _destinationPoints) {
        if (!IsServer) return;
        destinationPoints = _destinationPoints;
        if (destinationPoints == null) Debug.LogError("Destination points not given!");
        if (rb == null) Debug.LogError("Rigidbody2D was not added to:" + gameObject.name);
        if (animator == null) Debug.LogError("Animator was not added to:" + gameObject.name);
        if (viewZone == null) Debug.LogError("ViewZone was not added to:" + gameObject.name);
        StartCoroutine(Movement());
    }

    IEnumerator Movement() {
        yield return new WaitForSeconds(1f);
        int currentPoint = 0;
        while (true) {
            Transform dest = destinationPoints[currentPoint];
            while (Vector2.Distance(transform.position, dest.position) > 1f) {
                if (viewZone.HasVisibleTargets) {
                    if (isDasher) {
                        Vector3 targetPos = viewZone.GetClosestVisibleTarget().position;
                        statDashTime = Time.time;
                        yield return StartCoroutine(DashTo(targetPos));
                    }
                }
                Vector2 newPos = Vector2.MoveTowards(transform.position, dest.position, speed * Time.deltaTime);
                float dir = dest.position.x - transform.position.x;
                transform.localScale = new Vector3(Mathf.Sign(dir), 1, 1);

                rb.MovePosition(newPos);
                animator.SetBool("run", true);
                yield return null;
            }
            animator.SetBool("run", false);
            yield return new WaitForSeconds(timeToStayAtPoint);
            currentPoint = (currentPoint + 1) % destinationPoints.Count;
        }
    }

    IEnumerator DashTo(Vector3 targetPos) {
        animator.SetBool("dash", true);
        while (Time.time - statDashTime < dashTime) {
            Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, dashSpeed * Time.deltaTime);
            float dir = targetPos.x - transform.position.x;
            transform.localScale = new Vector3(Mathf.Sign(dir), 1, 1);
            rb.MovePosition(newPos);
            yield return null;
        }
        animator.SetBool("dash", false);

        var melee = GetComponent<EnemyAttackMelee>();
        if (melee != null) {
            melee.StartAttackServerRpc(transform.position);
            yield return new WaitForSeconds(0.25f);
            melee.StopAttackServerRpc();
        }
    }
}
