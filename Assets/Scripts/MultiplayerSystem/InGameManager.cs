using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Linq;

public class InGameManager : NetworkBehaviour {
    private Dictionary<ulong, PlayerData> playerData = new();
    public Dictionary<ulong, bool> playerAlive = new();

    public void RegisterPlayer(ulong clientId, Health health) {
        if (!playerData.ContainsKey(clientId)) {
            playerData.Add(clientId, new PlayerData(health));
            playerAlive.Add(clientId, true);
            health.onDie.AddListener(() => OnPlayerDie(clientId));
        }
    }

    public Health GetPlayerHealth(ulong clientId) {
        playerData.TryGetValue(clientId, out var player);
        return player?.health;
    }

    public IReadOnlyCollection<PlayerData> AllPlayersData => playerData.Values;

    private void OnPlayerDie(ulong clientId) {
        if (!IsServer) return;
        playerAlive[clientId] = false;
        NetworkManager.Singleton.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}

[System.Serializable]
public class PlayerData {
    public Health health { get; private set; }

    public PlayerData(Health health) {
        this.health = health;
    }
}