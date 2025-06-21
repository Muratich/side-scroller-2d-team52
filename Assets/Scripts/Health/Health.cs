using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
    public int health = 1;
    private int maxHealth;

    public UnityEvent onDamage;
    public UnityEvent onHeal;
    public UnityEvent onDie;

    public float invisibilityTime = 0.5f;
    private bool isInvisible = false;

    void Awake() {
        maxHealth = health;
    }

    public void TakeDamage(int damage) {
        if (isInvisible) return;

        onDamage?.Invoke();  
        health -= damage;
        if (health <= 0) {
            Die();
        }
        else StartCoroutine(Invicibility());
    }

    IEnumerator Invicibility() {
        isInvisible = true;
        yield return new WaitForSeconds(invisibilityTime);
        isInvisible = false;
    }

    public void Heal(int value) {
        health = Math.Min(maxHealth, health + value);
        onHeal?.Invoke();
    } 

    private void Die() {
        onDie?.Invoke();
    }
}
