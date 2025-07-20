using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour {
    public Health health;
    public Animator animator;

    void Start() {
        if (health == null) { Debug.Log("Health not set to enemy!"); return; }
        if (NetworkObject == null) { Debug.Log("Network object not set to enemy!"); return; }
        health.onDie.AddListener(ActivateAnim);
    }

    private void ActivateAnim() {
        StartCoroutine(DeathAnim());
    }

    IEnumerator DeathAnim() {
        animator.Play("death");
        yield return null;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        while (!info.IsName("death")) {
            yield return null;
            info = animator.GetCurrentAnimatorStateInfo(0);
        }
        while (info.normalizedTime < 1f) {
            yield return null;
            info = animator.GetCurrentAnimatorStateInfo(0);
        }

        if (IsServer)
            NetworkObject.Despawn();
        else
            RequestDespawnServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void RequestDespawnServerRpc() {
        if (!IsServer) return;
        NetworkObject.Despawn(destroy: true);
    }
}
