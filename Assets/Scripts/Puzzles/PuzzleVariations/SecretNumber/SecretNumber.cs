using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class SecretNumber : Puzzle {
    public int SecretLength = 5;
    [SerializeField] private List<TMP_Text> guessFields;
    [SerializeField] private GameObject keyboard;
    [SerializeField] private TMP_Text currentPlayerNameText;

    public NetworkList<int> secretNumber = new NetworkList<int>();
    private NetworkVariable<int> currentPlayer = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private List<ulong> playerIds = new List<ulong>();


    private enum Mark { None, Yellow, Green }
    private readonly Color green = Color.green;
    private readonly Color yellow = Color.yellow;
    private readonly Color white = Color.white;

    public override void PuzzleStart() {
        playerIds = NetworkManager.Singleton.ConnectedClients.Keys.OrderBy(id => id).ToList();

        if (IsServer) {
            var rng = new System.Random();
            secretNumber.Clear();
            for (int i = 0; i < SecretLength; i++)
                secretNumber.Add(rng.Next(0, 10));
            currentPlayer.Value = 0;
        }

        currentPlayer.OnValueChanged += (_, newIdx) => UpdateTurnUI(newIdx);
        UpdateTurnUI(currentPlayer.Value);
    }

    private void UpdateTurnUI(int playerIndex) {
        var clientId = playerIds[playerIndex];
        bool isMyTurn = NetworkManager.Singleton.LocalClientId == clientId;
        keyboard.SetActive(isMyTurn);
        currentPlayerNameText.text = GetPlayerName(clientId);
    }

    private void ClearFields() {
        foreach (var f in guessFields) {
            f.text = "_";
            f.color = white;
        }
    }

    private void PreviewGuessDigit(int index, int digit) {
        if (index == 0) ClearFields();
        if (index < 0 || index >= guessFields.Count) return;
        guessFields[index].text = digit.ToString();
        guessFields[index].color = white;
    }

    public void OnKeyboardInput(int[] guess) => SubmitGuessServerRpc(guess);
    [ServerRpc(RequireOwnership = false)] public void PreviewGuessServerRpc(int index, int digit, ServerRpcParams rpcParams = default) => PreviewGuessClientRpc(index, digit);
    [ClientRpc] private void PreviewGuessClientRpc(int index, int digit) => PreviewGuessDigit(index, digit);

    [ServerRpc(RequireOwnership = false)]
    private void SubmitGuessServerRpc(int[] guess, ServerRpcParams rpcParams = default) {
        if (playerIds[currentPlayer.Value] != rpcParams.Receive.SenderClientId) return;

        var marks = EvaluateGuess(guess);
        GiveColorsClientRpc(guess, marks);

        if (marks.All(m => m == Mark.Green))
            FinishServerRpc();
        else if (playerIds.Count > 1)
            currentPlayer.Value = (currentPlayer.Value + 1) % playerIds.Count;
    }

    private Mark[] EvaluateGuess(int[] guess) {
        var marks = new Mark[SecretLength];
        var used = new bool[SecretLength];

        for (int i = 0; i < SecretLength; i++) {
            if (guess[i] == secretNumber[i]) {
                marks[i] = Mark.Green;
                used[i] = true;
            }
        }
        for (int i = 0; i < SecretLength; i++) {
            if (marks[i] != Mark.None) continue;
            for (int j = 0; j < SecretLength; j++) {
                if (!used[j] && guess[i] == secretNumber[j]) {
                    marks[i] = Mark.Yellow;
                    used[j] = true;
                    break;
                }
            }
        }
        return marks;
    }

    [ClientRpc]
    private void GiveColorsClientRpc(int[] guess, Mark[] marks) {
        for (int i = 0; i < SecretLength; i++) {
            guessFields[i].text = guess[i].ToString();
            guessFields[i].color = marks[i] == Mark.Green ? green : marks[i] == Mark.Yellow ? yellow : white;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void FinishServerRpc(ServerRpcParams _ = default) => base.PuzzleFinish();

    private string GetPlayerName(ulong clientId) {
        var client = NetworkManager.Singleton.ConnectedClients[clientId];
        return client.PlayerObject.GetComponent<InGameProfile>().nickname.text;
    }
}