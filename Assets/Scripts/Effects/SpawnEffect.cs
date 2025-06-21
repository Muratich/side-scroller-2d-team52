using UnityEngine;

public class SpawnEffect : MonoBehaviour {
    [SerializeField] private Transform origin;
    public GameObject effect;
    public float lifetime = 1;

    public void Spawn() {
        if (effect == null) Debug.LogError("Effect prefab does not set!");
        if (origin == null) origin = transform;
        GameObject effectObj = Instantiate(effect, origin.transform.position, Quaternion.identity);
        Destroy(effectObj, lifetime);
    }
}
