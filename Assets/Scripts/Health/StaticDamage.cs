using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class StaticDamage : MonoBehaviour {
    public int damage = 1;
    public List<string> enemyTags;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!NetworkManager.Singleton.IsServer || !enemyTags.Contains(collision.tag)) return;

        if (collision.TryGetComponent<Health>(out Health health))
            health.TakeDamageServerRpc(damage);
    }
}
