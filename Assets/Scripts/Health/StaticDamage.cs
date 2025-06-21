using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticDamage : MonoBehaviour {
    public int damage = 1;
    public List<string> enemyTags;

    void OnTriggerEnter2D(Collider2D collision) {
        if (enemyTags.Contains(collision.tag)) {
            if (collision.TryGetComponent<Health>(out Health health)) {
                health.TakeDamage(damage);
            } else {
                Debug.Log("Object does not have Health script");
            }
        }
    }
}
