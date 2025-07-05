using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private List<EnemySpawnInfo> enemiesInfo;

    void Start() {
        if (NetworkManager.Singleton.IsServer)
            SpawnAll();
    }

    public void SpawnAll() {
        foreach (EnemySpawnInfo info in enemiesInfo) {
            var enemy = Instantiate(info.enemyPrefab, info.spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn(destroyWithScene: true);
            if (enemy.TryGetComponent<EnemyPatternMovement>(out EnemyPatternMovement patternMovement))
                patternMovement.Init(info.destinationPoints);
        }
    }
}

[System.Serializable]
public class EnemySpawnInfo {
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public List<Transform> destinationPoints;
}