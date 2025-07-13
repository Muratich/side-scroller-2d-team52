using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public abstract class EnemyAttack : NetworkBehaviour {
    public Animator animator;
    public float attackReload = 3;
    public Transform attackPos;
    protected Vector3 currTargetPos;

    [ServerRpc(RequireOwnership = false)] public void StartAttackServerRpc(Vector3 targetPosition) => StartAttack(targetPosition);
    [ServerRpc(RequireOwnership = false)] public void StopAttackServerRpc() => StopAttack();
    [ServerRpc(RequireOwnership = false)] public void UpdateTargetPositionServerRpc(Vector3 position) => currTargetPos = position;

    protected abstract void StartAttack(Vector3 targetPosition);
    protected abstract void StopAttack();
    protected void PlayAttack(bool on) {
        animator.SetBool("attack", on);
    }
}
