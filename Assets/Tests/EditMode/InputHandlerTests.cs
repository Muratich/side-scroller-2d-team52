using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerTests {
    private class TestWeaponManager : WeaponManager {
        public bool attacked;
        public new void ExecuteAttack() {
            attacked = true;
        }
    }

    [Test]
    public void AttackPerformed_CallsExecuteAttack() {
        var ihGO = new GameObject("InputHandler");
        var ih = ihGO.AddComponent<InputHandler>();
        var wmGO = new GameObject("WeaponManager");
        var wm = wmGO.AddComponent<TestWeaponManager>();
        ih.weaponManager = wm;

        ih.Awake();

        var method = typeof(InputHandler)
            .GetMethod("AttackPerformed", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(ih, new object[] { default(InputAction.CallbackContext) });
    }
}
