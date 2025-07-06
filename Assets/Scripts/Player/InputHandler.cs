using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : NetworkBehaviour  {
    private Control controls = null;
    [HideInInspector] public Vector2 movement = Vector2.zero;
    [HideInInspector] public float jumpPressTime = float.NegativeInfinity;
    [HideInInspector] public float jumpStartTime = float.NegativeInfinity;
    public WeaponManager weaponManager;

    public void Awake() {
        controls = new Control();
        if (weaponManager == null) Debug.LogError("Weapon Manager is null!");
    }
    
    public override void OnNetworkSpawn() {
        if (IsOwner) {
            controls.Enable();
            controls.Player.Move.performed += OnMovePerformed;
            controls.Player.Move.canceled += OnMoveCanceled;
            controls.Player.Jump.performed += JumpPress;
            controls.Player.Jump.canceled += JumpRelease;
            controls.Player.Attack.performed += AttackPerformed;
        }
        else {
            controls.Disable();
        }
    }

    private void AttackPerformed(InputAction.CallbackContext ctx) {
        if (weaponManager != null)
            weaponManager.ExecuteAttack();
    }

    public override void OnNetworkDespawn() {
        if (IsOwner) {
            controls.Player.Move.performed -= OnMovePerformed;
            controls.Player.Move.canceled -= OnMoveCanceled;
            controls.Player.Jump.performed -= JumpPress;
            controls.Player.Jump.canceled -= JumpRelease;
            controls.Player.Attack.performed -= AttackPerformed;
            controls.Disable();
        }
    }

    void OnMovePerformed(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    void OnMoveCanceled(InputAction.CallbackContext value) => movement = Vector2.zero;

    void JumpPress(InputAction.CallbackContext value) => jumpPressTime = Time.time;
    void JumpRelease(InputAction.CallbackContext value) => jumpPressTime = jumpStartTime = float.NegativeInfinity;
}
