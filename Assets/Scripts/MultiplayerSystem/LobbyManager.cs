using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {
    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject lobbyPanel;

    [Header("UI Elements")]
    public TMP_InputField inputField;
    public TMP_Text statusText;
    public Button startGameButton;
    public Button hostButton;
    public Button clientButton;
    public Button singleplayerButton;

    [Header("Player")]
    public GameObject playerPrefab;
    public ushort port = 777;
    private UnityTransport utp;
    public bool IsSingleplayer { get; private set; } = false;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (inputField != null) inputField.text = "127.0.0.1";
        if (utp == null) {
            Debug.LogError("Did not founded UnityTransport on NetworkManager");
            return;
        }
        if (NetworkManager.Singleton == null) {
            Debug.LogError("NetworkManager не найден в сцене!");
            return;
        }

        lobbyPanel.SetActive(false);
        startGameButton.interactable = false;
        hostButton.onClick.AddListener(OnHostButtonClicked);
        clientButton.onClick.AddListener(OnClientButtonClicked);
        startGameButton.onClick.AddListener(OnStartGameClicked);

        if (singleplayerButton != null)
            singleplayerButton.onClick.AddListener(Singleplayer);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy() {
        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    void ConfigureTransport(string connectAddress, ushort port, string listenAddress = null) {
        utp.SetConnectionData(connectAddress, port, listenAddress);
    }

    public void Singleplayer() {
        IsSingleplayer = true;

        if (NetworkManager.Singleton.IsListening)
            NetworkManager.Singleton.Shutdown();

        ConfigureTransport("127.0.0.1", port, "0.0.0.0");
        if (!NetworkManager.Singleton.StartHost()) {
            Debug.LogError("Failed to start host for singleplayer");
            return;
        }
        NetworkManager.Singleton.SceneManager.LoadScene("Level_1", LoadSceneMode.Single);
    }

    public void OnHostButtonClicked() {
        IsSingleplayer = false;
        string localIP = inputField.text;
        if (string.IsNullOrEmpty(localIP)) {Debug.LogError("Enter LAN!"); return; }

        if (NetworkManager.Singleton.IsListening)
            NetworkManager.Singleton.Shutdown();

        ConfigureTransport(localIP, port, "0.0.0.0");
        NetworkManager.Singleton.StartHost();
    }
    
    public void OnClientButtonClicked() {
        IsSingleplayer = false;
        string hostIP = inputField.text;
        if (string.IsNullOrEmpty(hostIP)) {Debug.LogError("Enter host IP!"); return; }

        if (NetworkManager.Singleton.IsListening)
            NetworkManager.Singleton.Shutdown();

        ConfigureTransport(hostIP, port);
        NetworkManager.Singleton.StartClient();
    }

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
        if (IsSingleplayer) return;
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
