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

        Vector3 spawnPoint;
        GameObject spObj = GameObject.FindGameObjectWithTag("Respawn");

        if (spObj == null) { Debug.Log("Spawn point not found on scene!");  spawnPoint = new Vector3(0, 0, 0); }
        else spawnPoint = spObj.transform.position;

        GameObject go = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        if (!go.TryGetComponent<NetworkObject>(out NetworkObject netObj)) { Debug.Log("Player has not NetworkObject!");  return; }
        netObj.SpawnAsPlayerObject(clientId, true);
    }
}
