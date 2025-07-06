using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class Health : NetworkBehaviour {
    public int startHealth = 1;

    public UnityEvent onDamage;
    public UnityEvent onHeal;
    public UnityEvent onDie;

    public float invisibilityTime = 0.5f;
    private bool isInvisible = false;

    public NetworkVariable<int> CurrentHealth;

    void Awake() {
        CurrentHealth = new NetworkVariable<int>(startHealth, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        CurrentHealth.OnValueChanged += OnHealthChanged;
    }

    private void OnDestroy() {
        CurrentHealth.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int oldValue, int newValue) {
        if (newValue < oldValue)
            onDamage?.Invoke();
        else if (newValue > oldValue)
            onHeal?.Invoke();

        if (oldValue > 0 && newValue <= 0)
            onDie?.Invoke();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage) {
        if (!IsServer || isInvisible) return;
        CurrentHealth.Value = Math.Max(0, CurrentHealth.Value - damage);
        isInvisible = true;
        StartCoroutine(Invincibility());
    }

    [ServerRpc(RequireOwnership = false)]
    public void HealServerRpc(int amount) {
        if (!IsServer) return;
        CurrentHealth.Value = Math.Min(startHealth, CurrentHealth.Value + amount);
    }

    private void Die() {
        onDie?.Invoke();
    }
    
    IEnumerator Invincibility() {
        yield return new WaitForSeconds(invisibilityTime);
        isInvisible = false;
    }
}
