using Unity.Netcode;
using UnityEngine;

public class WeaponManager : NetworkBehaviour {
    public Transform holdPoint;
    public Transform dropPoint;
    public Transform scaleRef;
    public PickUpWeapon defaultWeapon;
    [HideInInspector] public Weapon currentWeapon;

    public override void OnNetworkSpawn() {
        if (holdPoint == null || dropPoint == null || scaleRef == null) Debug.LogError("Not all references set!");

        if (IsServer) GiveDefaultWeapon(OwnerClientId);
    }

    private void GiveDefaultWeapon(ulong clientId) {
        var go = Instantiate(defaultWeapon.weaponPrefab.gameObject, holdPoint.position, Quaternion.identity);
        var net = go.GetComponent<NetworkObject>();
        net.SpawnWithOwnership(clientId, destroyWithScene: true);

        currentWeapon = go.GetComponent<Weapon>();
        currentWeapon.target   = holdPoint;
        currentWeapon.scaleRef = scaleRef;

        SetCurrentWeaponClientRpc(net.NetworkObjectId,new ClientRpcParams {Send = new ClientRpcSendParams {TargetClientIds = new[] { clientId }}});
    }

    public void TryPickup(PickUpWeapon item) {
        if (!IsOwner) return;
        var itemNet = item.GetComponent<NetworkObject>();
        HandlePickupServerRpc(itemNet.NetworkObjectId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandlePickupServerRpc(ulong itemNetworkId, ServerRpcParams rpcParams = default) {
        var clientId = rpcParams.Receive.SenderClientId;
        if (currentWeapon != null && currentWeapon.dropPrefab != null) {
            var oldNetObj = currentWeapon.GetComponent<NetworkObject>();

            var dropGo = Instantiate(currentWeapon.dropPrefab, dropPoint.position, Quaternion.identity);
            var dropNet = dropGo.GetComponent<NetworkObject>();
            dropNet.Spawn(destroyWithScene: true);

            oldNetObj.Despawn(destroy: true);
            currentWeapon = null;

            ClearCurrentWeaponClientRpc(new ClientRpcParams {
                Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
            });
        }

        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemNetworkId, out var itemNetObj)) return;
        var pickable = itemNetObj.GetComponent<PickUpWeapon>();
        if (pickable == null) return;

        var weapGo = Instantiate(pickable.weaponPrefab.gameObject, holdPoint.position, Quaternion.identity);
        var weapNet = weapGo.GetComponent<NetworkObject>();
        weapNet.SpawnWithOwnership(clientId, destroyWithScene: true);

        currentWeapon = weapGo.GetComponent<Weapon>();
        currentWeapon.target = holdPoint;
        currentWeapon.scaleRef = scaleRef;

        SetCurrentWeaponClientRpc(weapNet.NetworkObjectId, new ClientRpcParams {
            Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
        });

        itemNetObj.Despawn(destroy: true);
    }

    [ClientRpc]
    private void SetCurrentWeaponClientRpc(ulong weaponNetworkId, ClientRpcParams rpcParams = default) {
        var netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[weaponNetworkId];
        currentWeapon = netObj.GetComponent<Weapon>();
        currentWeapon.target = holdPoint;
        currentWeapon.scaleRef = scaleRef;
    }

    [ClientRpc]
    private void ClearCurrentWeaponClientRpc(ClientRpcParams rpcParams = default) {currentWeapon = null;}

    public void ExecuteAttack() {
        if (!IsOwner || currentWeapon == null) return;
        ExecuteAttackServerRpc();
    }

    [ServerRpc(RequireOwnership = true)]
    public void ExecuteAttackServerRpc(ServerRpcParams rpcParams = default) { currentWeapon.Attack(); }
}
