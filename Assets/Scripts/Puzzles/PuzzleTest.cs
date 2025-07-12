using Unity.Netcode;
using UnityEngine.UI;

public class PuzzleTest : Puzzle {
    public Button endPuzzleButton;

    public override void PuzzleStart() {
        endPuzzleButton.onClick.AddListener(OnFinishButtonClicked);
    }

    private void OnFinishButtonClicked() {
        SubmitFinishServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitFinishServerRpc(ServerRpcParams rpcParams = default) {
        base.PuzzleFinish();
    }
}
