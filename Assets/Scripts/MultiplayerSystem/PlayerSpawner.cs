using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour {
    public GameObject playerPrefab;

    private void Start() {
        if (NetworkManager.Singleton.IsServer) {
            SpawnAllExistingClients();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnDestroy() {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void SpawnAllExistingClients() {
        foreach (var kvp in NetworkManager.Singleton.ConnectedClients) {
            SpawnForClient(kvp.Key);
        }
    }

    private void OnClientConnected(ulong clientId) {
        SpawnForClient(clientId);
    }

    private void SpawnForClient(ulong clientId) {
        if (playerPrefab == null) { Debug.Log("Player prefab not set!"); return; }
        
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        GameObject go = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        var netObj = go.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId, true);
    }
}
