using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fifteens : Puzzle {
    [SerializeField] private List<Button> tileButtons;
    [SerializeField] private TMP_Text currentPlayerNameText;

    private NetworkList<int> tiles = new NetworkList<int>();
    private List<ulong> playerIds = new List<ulong>();
    private NetworkVariable<int> currentPlayer = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public Color origColor;
    public Color moveColor;

    public override void PuzzleStart() {
        playerIds.Clear();
        foreach (var kv in NetworkManager.Singleton.ConnectedClients)
            playerIds.Add(kv.Key);

        for (int i = 0; i < tileButtons.Count; i++) {
            int idx = i;
            tileButtons[i].onClick.RemoveAllListeners();
            tileButtons[i].onClick.AddListener(() => OnTileClicked(idx));
        }

        tiles.OnListChanged += change => { RefreshUI(); UpdateTurnUI(); };
        currentPlayer.OnValueChanged += (_, __) => UpdateTurnUI();

        if (IsServer) InitializePuzzle();
        RefreshUI();
        UpdateTurnUI();
    }

    private void InitializePuzzle() {
        tiles.Clear();
        List<int> init = new List<int>();
        for (int i = 1; i <= 8; i++) init.Add(i);
        init.Add(0);

        while (true) {
            for (int i = init.Count - 1; i > 0; i--) {
                int j = Random.Range(0, i + 1);
                int tmp = init[i]; init[i] = init[j]; init[j] = tmp;
            }
            if (IsSolvable(init)) break;
        }

        foreach (var v in init) tiles.Add(v);
        currentPlayer.Value = 0;
    }

    private bool IsSolvable(List<int> list) {
        int inv = 0;
        for (int i = 0; i < list.Count; i++) {
            for (int j = i + 1; j < list.Count; j++) {
                if (list[i] > 0 && list[j] > 0 && list[i] > list[j]) inv++;
            }
        }
        return inv % 2 == 0;
    }

    private void RefreshUI() {
        for (int i = 0; i < tileButtons.Count; i++) {
            int val = (i < tiles.Count) ? tiles[i] : 0;
            var btn = tileButtons[i];
            var txt = btn.GetComponentInChildren<TMP_Text>();
            if (val == 0) {
                txt.text = val.ToString();
                btn.image.color = moveColor;
            } else {
                txt.text = val.ToString();
                btn.image.color = origColor;
            }
            btn.interactable = false;
        }
    }

    private void UpdateTurnUI() {
        int idx = currentPlayer.Value;
        ulong clientId = playerIds.Count > 0 ? playerIds[idx] : NetworkManager.Singleton.LocalClientId;
        bool isMyTurn = NetworkManager.Singleton.LocalClientId == clientId;

        if (currentPlayerNameText)
            currentPlayerNameText.text = isMyTurn ? "Your turn" : GetPlayerName(clientId) + "'s turn";

        for (int i = 0; i < tileButtons.Count; i++) {
            tileButtons[i].interactable = isMyTurn;
        }
    }

    private void OnTileClicked(int index) => SubmitMoveServerRpc(index);

    [ServerRpc(RequireOwnership = false)]
    private void SubmitMoveServerRpc(int index, ServerRpcParams rpcParams = default) {
        if (playerIds[currentPlayer.Value] != rpcParams.Receive.SenderClientId) return;

        int blank = tiles.IndexOf(0);
        if (!GetNeighbours(blank).Contains(index)) return;

        tiles[blank] = tiles[index];
        tiles[index] = 0;

        bool solved = true;
        for (int i = 0; i < 8; i++) {
            if (tiles[i] != i + 1) { solved = false; break; }
        }
        if (solved && tiles[8] == 0) {
            FinishServerRpc();
            return;
        }
        currentPlayer.Value = (currentPlayer.Value + 1) % playerIds.Count;
    }

    private List<int> GetNeighbours(int index) {
        var nbrs = new List<int>();
        int r = index / 3, c = index % 3;
        if (r > 0) nbrs.Add(index - 3);
        if (r < 2) nbrs.Add(index + 3);
        if (c > 0) nbrs.Add(index - 1);
        if (c < 2) nbrs.Add(index + 1);
        return nbrs;
    }

    [ServerRpc(RequireOwnership = false)] private void FinishServerRpc(ServerRpcParams _ = default) => base.PuzzleFinish();
    private string GetPlayerName(ulong clientId) {
        var client = NetworkManager.Singleton.ConnectedClients[clientId];
        return client.PlayerObject.GetComponent<InGameProfile>().nickname.text;
    }
}
