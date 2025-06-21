using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;

public class EnemyPatternMovement : MonoBehaviour {
    [Tooltip("The points where the enemy will move sequentially")]
    public List<Transform> destinationPoints;

    [Header("Characteristics")]
    public float speed = 10;
    public float timeToStayAtPoint = 5;


    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public ViewZone viewZone;


    void Awake() {
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
            while (Vector2.Distance(transform.position, dest.position) > 0.6f) {
                if (viewZone.HasVisibleTargets) {
                    yield return new WaitUntil(() => !viewZone.HasVisibleTargets);
                }
                Vector2 newPos = Vector2.MoveTowards(transform.position, dest.position, speed * Time.deltaTime);
                float dir = dest.position.x - transform.position.x;
                if (dir > 0.01f) transform.localScale = new Vector3(1, 1, 1);
                else if (dir < -0.01f) transform.localScale = new Vector3(-1, 1, 1);

                rb.MovePosition(newPos);
                animator.SetBool("run", true);
                yield return null;
            }
            animator.SetBool("run", false);
            yield return new WaitForSeconds(timeToStayAtPoint);
            currentPoint = (currentPoint + 1) % destinationPoints.Count;
        }
    }
}
