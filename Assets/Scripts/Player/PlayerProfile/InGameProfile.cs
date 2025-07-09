using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InGameProfile : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>(
        new NetworkString(""), 
            readPerm: NetworkVariableReadPermission.Everyone, 
                writePerm: NetworkVariableWritePermission.Owner);

    public TMP_Text nickname;

    public override void OnNetworkSpawn() {
        playerName.OnValueChanged += (oldValue, newValue) => { SetNameForPlayer(newValue.Value); };

        if (IsOwner) {
            var profile = FindObjectOfType<Profile>();
            if (profile != null) {
                playerName.Value = new NetworkString(profile.GetPlayerName());
            }
            else {
                Debug.LogWarning("Profile не найден в сцене!");
            }
        }
        SetNameForPlayer(playerName.Value.Value);
    }

    public void SetNameForPlayer(string name) {
        nickname.text = name;
    }
}
