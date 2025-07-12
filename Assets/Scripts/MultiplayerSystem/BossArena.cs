using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArena : NetworkBehaviour {
    public Transform bossFightPosition;
    public MusicSwitcher musicSwitcher;
    public GameObject gates;

    private bool fightStarted = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (!IsServer || fightStarted) return;
        if (other.CompareTag("Player")) {
            fightStarted = true;

            StartBossFightClientRpc(bossFightPosition.position);

            var finds = FindObjectsByNameSubstring("boss");
            if (finds != null) {
                GameObject boss = finds[0];
                if (boss != null) boss.GetComponent<Health>().onDie.AddListener(EndBossFightClientRpc);
            }
        }
    }

    [ClientRpc]
    private void StartBossFightClientRpc(Vector3 targetPosition) {
        musicSwitcher.SwitchToBoss(true);
        gates.SetActive(true);

        foreach (var mover in FindObjectsOfType<Movement>()) {
            if (mover.IsOwner) {
                mover.transform.position = targetPosition;
                if (mover.rb != null)
                    mover.rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void BossDeath() {
        EndBossFightClientRpc();
    }

    [ClientRpc]
    private void EndBossFightClientRpc() {
        gates.SetActive(false);
        musicSwitcher.SwitchToBoss(false);
    }
    
    public static List<GameObject> FindObjectsByNameSubstring(string substring) {
        var results = new List<GameObject>();
        var scene = SceneManager.GetActiveScene();
        foreach (var root in scene.GetRootGameObjects()) {
            SearchInHierarchy(root, substring, results);
        }
        return results;
    }

    private static void SearchInHierarchy(GameObject go, string substring, List<GameObject> results) {
        if (go.name.Contains(substring))
            results.Add(go);

        foreach (Transform child in go.transform)
            SearchInHierarchy(child.gameObject, substring, results);
    }
}
