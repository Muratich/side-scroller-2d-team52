using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimonSays : Puzzle {
    public List<Button> colorButtons;
    [SerializeField] private float highlightDuration = 0.5f;
    [SerializeField] private float pauseBetween = 0.2f;
    [SerializeField] private int maxSequenceLength = 8;

    public NetworkList<int> sequence = new NetworkList<int>();
    private NetworkVariable<int> currentPlayer = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private List<ulong> playerIds = new List<ulong>();
    private int inputIndex = 0;
    [SerializeField] private Sprite origSprite;
    [SerializeField] private Sprite highlitedSprite;
    [SerializeField] private Sprite wrongChooseSprite;
    [SerializeField] private TMP_Text currentPlayerNameText;

    public override void PuzzleStart() {
        playerIds = new List<ulong>(NetworkManager.Singleton.ConnectedClients.Keys);

        if (IsServer) {
            sequence.Clear();
            AddRandomToSequence();

            currentPlayer.Value = 0;
            inputIndex = 0;
            SendSequenceToClients();
        }

        currentPlayer.OnValueChanged += (_, newIdx) => UpdateTurnUI(newIdx);
        UpdateTurnUI(currentPlayer.Value);
    }

    private void AddRandomToSequence() {
        int idx = Random.Range(0, colorButtons.Count);
        sequence.Add(idx);
    }

    private void UpdateTurnUI(int playerIndex) {
        ulong clientId = playerIds[playerIndex];
        bool isMyTurn = NetworkManager.Singleton.LocalClientId == clientId;

        currentPlayerNameText.text = GetPlayerName(clientId);
        foreach (var btn in colorButtons)
            btn.interactable = false;
    }

    private void SendSequenceToClients() {
        foreach (var btn in colorButtons) btn.image.sprite = origSprite;

        int count = sequence.Count;
        int[] arr = new int[count];
        for (int i = 0; i < count; i++)
            arr[i] = sequence[i];
        ShowSequenceClientRpc(arr, count);
    }

    [ClientRpc]
    private void ShowSequenceClientRpc(int[] seq, int length) {
        StopAllCoroutines();
        StartCoroutine(PlaySequence(seq, length));
        UpdateTurnUI(currentPlayer.Value);
    }

    private IEnumerator PlaySequence(int[] seq, int length) {
        foreach (var btn in colorButtons) btn.image.sprite = origSprite;
        foreach (var btn in colorButtons)
            btn.interactable = false;

        yield return new WaitForSeconds(pauseBetween);
        for (int i = 0; i < length; i++) {
            int idx = seq[i];
            var btn = colorButtons[idx];

            btn.image.sprite = highlitedSprite;
            yield return new WaitForSeconds(highlightDuration);
            btn.image.sprite = origSprite;
            yield return new WaitForSeconds(pauseBetween);
        }
        if (NetworkManager.Singleton.LocalClientId == playerIds[currentPlayer.Value]) {
            foreach (var btn in colorButtons)
                btn.interactable = true;
        }
        inputIndex = 0;
    }

    public void OnColorButtonPressed(int index) {
        SubmitInputServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitInputServerRpc(int index, ServerRpcParams rpcParams = default) {
        if (playerIds[currentPlayer.Value] != rpcParams.Receive.SenderClientId)
            return;

        if (index == sequence[inputIndex]) {
            HighlightButtonClientRpc(index, true);
            inputIndex++;
            if (inputIndex >= sequence.Count) {
                if (sequence.Count >= maxSequenceLength) {
                    FinishServerRpc();
                    return;
                }
                AddRandomToSequence();
                inputIndex = 0;
                currentPlayer.Value = (currentPlayer.Value + 1) % playerIds.Count;
                SendSequenceToClients();
            }
        } else {
            HighlightButtonClientRpc(index, false);
            StartCoroutine(ResetSequenceAfterDelay());
        }
    }

    private IEnumerator ResetSequenceAfterDelay() {
        yield return new WaitForSeconds(highlightDuration/2+0.1f);
        sequence.Clear();
        AddRandomToSequence();
        inputIndex = 0;
        currentPlayer.Value = 0;
        SendSequenceToClients();
    }

    [ClientRpc]
    private void HighlightButtonClientRpc(int index, bool correctChose) {
        StartCoroutine(HighlightButton(index, correctChose));
    }

    private IEnumerator HighlightButton(int idx, bool correctChose) {
        var btn = colorButtons[idx];
        btn.image.sprite = (correctChose) ? highlitedSprite : wrongChooseSprite;
        yield return new WaitForSeconds(highlightDuration/2);
        btn.image.sprite = origSprite;
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