using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {
    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject lobbyPanel;

    [Header("UI Elements")]
    public TMP_Text statusText;
    public Button startGameButton;
    public Button hostButton;
    public Button clientButton;

    [Header("Player")]
    public GameObject playerPrefab;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        if (NetworkManager.Singleton == null) {
            Debug.LogError("NetworkManager не найден в сцене!");
            return;
        }

        lobbyPanel.SetActive(false);
        startGameButton.interactable = false;
        hostButton.onClick.AddListener(OnHostButtonClicked);
        clientButton.onClick.AddListener(OnClientButtonClicked);
        startGameButton.onClick.AddListener(OnStartGameClicked);


        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy() {
        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    public void OnHostButtonClicked() => NetworkManager.Singleton.StartHost();
    public void OnClientButtonClicked() => NetworkManager.Singleton.StartClient();

    private void OnClientConnected(ulong clientId) {
        if (clientId == NetworkManager.Singleton.LocalClientId) {
            EnterLobby();
        }
        UpdateLobbyStatus();
    }

    private void OnClientDisconnected(ulong clientId) {
        UpdateLobbyStatus();
        if (clientId == NetworkManager.Singleton.LocalClientId) {
            lobbyPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
    }

    private void EnterLobby() {
        menuPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        UpdateLobbyStatus();
    }

    private void UpdateLobbyStatus() {
        int count = NetworkManager.Singleton.ConnectedClients.Count;
        statusText.text = $"{count} / 2 players connected";

        startGameButton.interactable = NetworkManager.Singleton.IsHost && count == 2;
    }

    private void OnStartGameClicked() {
        NetworkManager.Singleton.SceneManager.LoadScene("Level_1", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
