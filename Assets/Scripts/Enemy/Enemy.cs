using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour {
    public Health health;
    public NetworkObject networkObject;

    void Awake() {
        if (health == null) { Debug.Log("Health not set to enemy!"); return; }
        if (networkObject == null) { Debug.Log("Network object not set to enemy!"); return; }
        health.onDie.AddListener(EnemyDestroyLocal);
    }

    public void EnemyDestroyLocal() {
        if (IsServer)
            networkObject.Despawn();
        else
            RequestDespawnServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void RequestDespawnServerRpc() {
        if (!IsServer) return;
        networkObject.Despawn(destroy: true);
    }
}
