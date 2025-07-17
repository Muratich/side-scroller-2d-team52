using Unity.Netcode;
using UnityEngine;

public class PuzzleTrigger : NetworkBehaviour {
    public PuzzleManager puzzleManager;
    public GameObject puzzle;
    public GameObject gates;

    void Start() {
        if (puzzleManager == null) Debug.LogError("PuzzleManager not found for the: " + gameObject.name);
        if (gates == null) Debug.Log("Gates object not set to the: " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!IsServer || puzzleManager == null) return;
        if (!collision.CompareTag("Player")) return;

        puzzleManager.Activate(gates, transform.position, puzzle);
        Destroy(gameObject);
    }
}
