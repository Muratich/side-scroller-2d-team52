using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Anagrams : Puzzle {
    [SerializeField] private List<string> wordList;
    [SerializeField] private List<Button> letterButtons;
    [SerializeField] private TMP_Text currentPlayerNameText;

    private NetworkVariable<NetworkString> scrambledWord = new NetworkVariable<NetworkString>(
        default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> currentPlayer = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> firstSelection = new NetworkVariable<int>(-1,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private List<ulong> playerIds = new List<ulong>();
    private string originalWord;

    public override void PuzzleStart() {
        wordList.RemoveAll(w => w.Length != letterButtons.Count);

        playerIds = new List<ulong>(NetworkManager.Singleton.ConnectedClients.Keys);
        scrambledWord.OnValueChanged += (_, __) => {RefreshUI(); UpdateTurnUI(currentPlayer.Value);};
        currentPlayer.OnValueChanged += (_, idx) => UpdateTurnUI(idx);

        for (int i = 0; i < letterButtons.Count; i++) {
            int idx = i;
            letterButtons[i].onClick.RemoveAllListeners();
            letterButtons[i].onClick.AddListener(() => OnLetterClicked(idx));
        }

        if (IsServer) InitializePuzzle();

        RefreshUI();
        UpdateTurnUI(currentPlayer.Value);
    }

    private void InitializePuzzle() {
        originalWord = wordList[Random.Range(0, wordList.Count)].ToUpper();
        char[] chars = originalWord.ToCharArray();

        for (int i = 0; i < chars.Length; i++) {
            int j = Random.Range(i, chars.Length);
            char tmp = chars[i];
            chars[i] = chars[j];
            chars[j] = tmp;
        }
        scrambledWord.Value = new NetworkString(new string(chars));
        currentPlayer.Value = 0;
        firstSelection.Value = -1;
    }

    private void RefreshUI() {
        string s = scrambledWord.Value.Value;
        for (int i = 0; i < letterButtons.Count; i++) {
            letterButtons[i].GetComponentInChildren<TMP_Text>().text = s[i].ToString();
            letterButtons[i].interactable = false;
            letterButtons[i].gameObject.SetActive(true);
        }
    }

    private void UpdateTurnUI(int playerIndex) {
        ulong clientId = playerIds[playerIndex];
        bool isMyTurn = NetworkManager.Singleton.LocalClientId == clientId;
        currentPlayerNameText.text = GetPlayerName(clientId) + "'s turn";
        for (int i = 0; i < letterButtons.Count; i++)
            letterButtons[i].interactable = isMyTurn;
    }

    private void OnLetterClicked(int index) {
        SubmitSelectionServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitSelectionServerRpc(int index, ServerRpcParams rpcParams = default) {
        if (playerIds[currentPlayer.Value] != rpcParams.Receive.SenderClientId) return;

        if (firstSelection.Value < 0) {
            firstSelection.Value = index;
        } else if (firstSelection.Value != index) {
            char[] chars = scrambledWord.Value.Value.ToCharArray();
            int i1 = firstSelection.Value;
            int i2 = index;
            char tmp = chars[i1];
            chars[i1] = chars[i2];
            chars[i2] = tmp;

            scrambledWord.Value = new NetworkString(new string(chars));
            firstSelection.Value = -1;

            if (new string(chars).Equals(originalWord)) {
                FinishServerRpc();
                return;
            }
            currentPlayer.Value = (currentPlayer.Value + 1) % playerIds.Count;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void FinishServerRpc(ServerRpcParams _ = default) {
        base.PuzzleFinish();
    }

    private string GetPlayerName(ulong clientId) {
        var client = NetworkManager.Singleton.ConnectedClients[clientId];
        return client.PlayerObject.GetComponent<InGameProfile>().nickname.text;
    }
}