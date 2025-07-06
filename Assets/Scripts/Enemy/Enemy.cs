using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour {
    public Health health;

    void Start() {
        if (health == null) { Debug.Log("Health not set to enemy!"); return; }
        if (NetworkObject == null) { Debug.Log("Network object not set to enemy!"); return; }
        health.onDie.AddListener(EnemyDestroyLocal);
    }

    public void EnemyDestroyLocal() {
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
