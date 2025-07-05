using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : NetworkBehaviour {
    public Transform holdPoint;
    public Transform dropPoint;
    public Transform scaleRef;
    [HideInInspector] public Weapon currentWeapon;

    void Start() {
        if (holdPoint == null || dropPoint == null || scaleRef == null) Debug.LogError("Not all references set!");
    }

    public void TryPickup(PickUpWeapon item) {
        if (!IsOwner) return;
        var itemNet = item.GetComponent<NetworkObject>();
        PickupServerRpc(itemNet.NetworkObjectId);
    }

    [ClientRpc]
    private void SetCurrentWeaponClientRpc(ulong weaponNetworkId, ClientRpcParams rpcParams = default)
    {
        var netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[weaponNetworkId];
        currentWeapon = netObj.GetComponent<Weapon>();
        currentWeapon.target = holdPoint;
        currentWeapon.scaleRef = scaleRef;
    }

    [ServerRpc(RequireOwnership = false)]
    private void PickupServerRpc(ulong itemNetworkId, ServerRpcParams rpcParams = default) {
        if (currentWeapon != null || !NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemNetworkId, out var itemNetObj))
            return;

        var pickable = itemNetObj.GetComponent<PickUpWeapon>();
        if (pickable == null) return;

        var clientId = rpcParams.Receive.SenderClientId;

        var weapGo = Instantiate(pickable.weaponPrefab.gameObject, holdPoint.position, Quaternion.identity);
        var weapNet = weapGo.GetComponent<NetworkObject>();
        weapNet.SpawnWithOwnership(clientId, destroyWithScene: true);

        currentWeapon = weapGo.GetComponent<Weapon>();
        currentWeapon.target = holdPoint;
        currentWeapon.scaleRef = scaleRef;

        SetCurrentWeaponClientRpc(weapNet.NetworkObjectId, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } } } );
        itemNetObj.Despawn(destroy: true);
    }

    public void ExecuteAttack() {
        if (!IsOwner || currentWeapon == null) return;
        ExecuteAttackServerRpc();
    }
    
    [ServerRpc(RequireOwnership = true)]
    public void ExecuteAttackServerRpc(ServerRpcParams rpcParams = default) { currentWeapon.Attack();}
}
