using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Memo : Puzzle {
    public TMP_Text console;
    public Transform board;

    private NetworkList<int> cardValues;
    private NetworkList<byte> cardStates;
    private NetworkVariable<int> currentPlayerIndex;
    private List<ulong> playerIds = new();
    private int firstIndex = -1, secondIndex = -1, matchesFound = 0;
    private bool isChecking = false;

    void Awake() {
        cardValues = new NetworkList<int>();
        cardStates = new NetworkList<byte>();
        currentPlayerIndex = new NetworkVariable<int>(0);
    }

    public override void PuzzleStart() {
        if (IsServer)
            InitializeGameOnServer();
        cardStates.OnListChanged += OnStatesChanged;
    }

    private void InitializeGameOnServer() {
        playerIds = NetworkManager.Singleton.ConnectedClients.Keys.ToList();

        var items = board.GetComponentsInChildren<PuzzleItem>(false);
        int count = items.Length;

        var vals = Enumerable.Range(0, count / 2).SelectMany(i => new[] { i, i }).OrderBy(_ => Guid.NewGuid()).ToArray();
        var permutation = Enumerable.Range(0, count).OrderBy(_ => Guid.NewGuid()).ToArray();

        cardValues.Clear();
        foreach (var v in vals) cardValues.Add(v);

        cardStates.Clear();
        for (int i = 0; i < count; i++)
            cardStates.Add(0);

        currentPlayerIndex.Value = 0;
        UpdateTurnConsoleClientRpc(playerIds[0]);

        ShuffleBoardClientRpc(permutation);
        ApplyValuesClientRpc(vals);
    }

    [ClientRpc]
    private void ShuffleBoardClientRpc(int[] permutation) {
        var children = new Transform[board.childCount];
        for (int i = 0; i < board.childCount; i++)
            children[i] = board.GetChild(i);

        for (int i = 0; i < permutation.Length; i++) {
            children[permutation[i]].SetSiblingIndex(i);
        }
    }

    [ClientRpc]
    private void ApplyValuesClientRpc(int[] values) {
        var items = board.GetComponentsInChildren<PuzzleItem>(false);
        for (int i = 0; i < values.Length; i++)
            items[i].SetValue(values[i]);
    }

    [ClientRpc]
    private void UpdateTurnConsoleClientRpc(ulong clientId) {
        console.text = $"Player {GetPlayerNameByID(clientId)}'s turn";
    }

    private void OnStatesChanged(NetworkListEvent<byte> change) {
        var item = board.GetChild(change.Index).GetComponent<PuzzleItem>();
        item.ApplyState(change.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestFlipServerRpc(int index, ServerRpcParams prms = default) {
        if (isChecking) return;
        if (playerIds[currentPlayerIndex.Value] != prms.Receive.SenderClientId)
            return;
        if (cardStates[index] != 0)
            return;

        cardStates[index] = 1;

        if (firstIndex < 0)
            firstIndex = index;
        else {
            secondIndex = index;
            isChecking = true;
            StartCoroutine(CheckPairAfterDelay(0.5f));
        }
    }

    private System.Collections.IEnumerator CheckPairAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        if (firstIndex < 0 || secondIndex < 0 || firstIndex >= cardValues.Count || secondIndex >= cardValues.Count) {
            firstIndex = secondIndex = -1;
            isChecking = false;
            yield break;
        }

        if (cardValues[firstIndex] == cardValues[secondIndex]) {
            cardStates[firstIndex] = 2;
            cardStates[secondIndex] = 2;
            matchesFound++;
            if (matchesFound >= cardStates.Count / 2) {
                SubmitFinishServerRpc();
                yield break;
            }
        }
        else {
            cardStates[firstIndex] = 0;
            cardStates[secondIndex] = 0;
        }

        firstIndex = secondIndex = -1;
        currentPlayerIndex.Value = (currentPlayerIndex.Value + 1) % playerIds.Count;
        UpdateTurnConsoleClientRpc(playerIds[currentPlayerIndex.Value]);
        isChecking = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitFinishServerRpc(ServerRpcParams p = default) {
        base.PuzzleFinish();
    }
    
    public string GetPlayerNameByID(ulong clientId) {
        var profiles = FindObjectsOfType<InGameProfile>(true);
        foreach (var profile in profiles) {
            var netObj = profile.GetComponent<NetworkObject>();
            if (netObj != null && netObj.OwnerClientId == clientId) {
                return profile.nickname.text;
            }
        }
        return $"Player {clientId}";
    }
}
