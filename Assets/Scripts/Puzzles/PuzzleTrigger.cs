using Unity.Netcode;
using UnityEngine;

public class PuzzleTrigger : NetworkBehaviour {
    private PuzzleManager puzzleManager;
    public GameObject gates;

    void Start() {
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
        if (puzzleManager == null) Debug.LogError("PuzzleManager not found for the: " + gameObject.name);
        if (gates == null) Debug.Log("Gates object not set to the: " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!IsServer || puzzleManager == null) return;
        if (!collision.CompareTag("Player")) return;

        puzzleManager.Activate(gates, transform.position);
        Destroy(gameObject);
    }
}
