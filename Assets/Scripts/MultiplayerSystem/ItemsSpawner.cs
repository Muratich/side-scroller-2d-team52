using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour {
    public List<ItemInfo> items = new ();

    void Start() {
        if (NetworkManager.Singleton.IsServer) {
            Spawn();
        }
    }

    public void Spawn() {
        foreach (ItemInfo item in items) {
            var pref = Instantiate(item.prefab, item.spawnPoint.position, Quaternion.identity);
            pref.GetComponent<NetworkObject>().Spawn(destroyWithScene: true);
        }
    }
}

[System.Serializable]
public class ItemInfo {
    public GameObject prefab;
    public Transform spawnPoint;
}
