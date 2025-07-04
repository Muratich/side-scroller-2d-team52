using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSettings : NetworkBehaviour {
    public static InGameSettings Instance;

    void Awake() => Instance = this;

    public void OnDisconnectButtonPressed() {
        if (!IsOwner) return;
        RequestDisconnectServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDisconnectServerRpc(ServerRpcParams rpcParams = default) {
        DisconnectAllClientRpc();
    }

    [ClientRpc]
    private void DisconnectAllClientRpc(ClientRpcParams rpcParams = default) {
        DisconnectToMenu();
    }

    public void DisconnectToMenu() {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
