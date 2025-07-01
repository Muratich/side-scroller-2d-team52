using UnityEngine;

public class HealthUI : MonoBehaviour {
    private Health health;
    public Transform healthBar;
    public GameObject healthPrefab;

    public void Init(Health target) {
        health = target;
        ClearBar();
        SetHealthUI(health.health);
        health.onDamage?.AddListener(onHealthChangeHandler);
        health.onHeal?.AddListener(onHealthChangeHandler);
    }

    private void onHealthChangeHandler() {
        SetHealthUI(health.health);
    }

    private void ClearBar() {
        foreach (Transform child in healthBar) {
            Destroy(child.gameObject);
        }
    }

    private void SetHealthUI(int value) {
        ClearBar();
        for (int i = 0; i < value; i++) {
            Transform pref = Instantiate(healthPrefab).transform;
            pref.SetParent(healthBar);
        }
    }
}
