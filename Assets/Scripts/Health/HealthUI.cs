using UnityEngine;

public class HealthUI : MonoBehaviour {
    [SerializeField] public string targetTag;
    private Health health;
    public Transform healthBar;
    public GameObject healthPrefab;

    void Awake() {
        health = GameObject.FindGameObjectWithTag(targetTag).GetComponent<Health>();
        if (health == null) {
            Debug.LogError("Target object with Health component has not founded!");
            return;
        }
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
