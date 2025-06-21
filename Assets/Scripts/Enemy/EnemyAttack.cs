using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour {
    public Animator animator;
    public float attackReload = 3;
    public abstract void StartAttack(Transform target);
    public abstract void StopAttack();
}
