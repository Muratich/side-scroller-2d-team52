using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PuzzleManager : NetworkBehaviour {
    public List<GameObject> puzzlesPrefabs = new();
    [HideInInspector] public List<int> alreadyTakenPuzzlesIndexes = new();
    private bool puzzleStarted = false;
    private GameObject gates;

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void Activate(GameObject gates, Vector3 teleportPos) {
        if (gates == null) { Debug.Log("Gates not given!"); return; }
        if (!IsServer || puzzleStarted) return;
        puzzleStarted = true;

        this.gates = gates;
        gates.SetActive(true);

        var gateRef = new NetworkObjectReference(gates.GetComponent<NetworkObject>());
        ActivateClientRpc(gateRef, teleportPos);

        int randPuzzle = PickUpRandomPuzzle();
        if (randPuzzle == -1) return;

        GameObject puzzleWindow = Instantiate(puzzlesPrefabs[randPuzzle]);
        var netObj = puzzleWindow.GetComponent<NetworkObject>();
        netObj.Spawn(destroyWithScene: true);

        StartPuzzleClientRpc(teleportPos);
    }
    
    [ClientRpc]
    private void ActivateClientRpc(NetworkObjectReference gateRef, Vector3 teleportPos) {
        if (gateRef.TryGet(out var netObj)) {
            gates = netObj.gameObject;
            gates.SetActive(true);
        } else Debug.LogError("Gates not set to the Client!");

        foreach (var mover in FindObjectsOfType<Movement>()) {
            if (mover.IsOwner) {
                mover.control = false;
                mover.transform.position = teleportPos;
                if (mover.rb != null) mover.rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public int PickUpRandomPuzzle() {
        if (puzzlesPrefabs == null || puzzlesPrefabs.Count == 0 || puzzlesPrefabs.Count <= alreadyTakenPuzzlesIndexes.Count) { Debug.LogError("Puzzle not set in PuzzleManager!"); return -1; }

        int randInd = Random.Range(0, puzzlesPrefabs.Count);
        if (alreadyTakenPuzzlesIndexes.Contains(randInd)) return PickUpRandomPuzzle();
        else {
            alreadyTakenPuzzlesIndexes.Add(randInd);
            return randInd;
        }
    }

    [ClientRpc]
    private void StartPuzzleClientRpc(Vector3 teleportPos) {
        foreach (var mover in FindObjectsOfType<Movement>()) {
            if (mover.IsOwner) {
                mover.control = false;
                mover.transform.position = teleportPos;
                if (mover.rb != null) mover.rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void PuzzleFinish() {
        FinishOnInstance();
        PuzzleFinishClientRpc();
    }

    private void FinishOnInstance() {
        puzzleStarted = false;
        if (gates != null) gates.SetActive(false);
        foreach (var mover in FindObjectsOfType<Movement>()) {
            if (mover.IsOwner) {
                mover.control = true;
            }
        }
    }

    [ClientRpc]
    private void PuzzleFinishClientRpc() {
        if (IsServer) return;
        FinishOnInstance();
    }
}