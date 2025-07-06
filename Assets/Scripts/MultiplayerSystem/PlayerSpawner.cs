using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    public GameObject hostPlayerPrefab;
    public GameObject clientPlayerPrefab;
    public InGameManager inGameManager;

    public void Start() {
        if (inGameManager == null) Debug.Log("In game manager not set!");
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
        bool isHost = NetworkManager.Singleton.IsHost && clientId == NetworkManager.Singleton.LocalClientId;
        GameObject prefabToSpawn = isHost ? hostPlayerPrefab : clientPlayerPrefab;

        if (prefabToSpawn == null) { Debug.Log("Player prefab not set!"); return; }

        Vector3 spawnPoint;
        GameObject spObj = GameObject.FindGameObjectWithTag("Respawn");

        if (spObj == null) { Debug.Log("Spawn point not found on scene!"); spawnPoint = new Vector3(0, 0, 0); }
        else spawnPoint = spObj.transform.position;

        GameObject go = Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity);
        if (!go.TryGetComponent<NetworkObject>(out NetworkObject netObj)) { Debug.Log("Player has not NetworkObject!"); return; }
        netObj.SpawnAsPlayerObject(clientId, true);

        if (!go.TryGetComponent<Health>(out Health health)) { Debug.Log("Player has not Health!"); return; }
        inGameManager.RegisterPlayer(clientId, health);
    }
}
