using Unity.Netcode;
using UnityEngine;

public abstract class Weapon : NetworkBehaviour {
    public float reloadTime = 3;
    public Transform firePos;
    public GameObject proj;

    [HideInInspector] public Transform target;
    [HideInInspector] public Transform scaleRef;

    public abstract void Attack();

    protected virtual void Update() {
        if (scaleRef == null) return;
        transform.position = target.position;
        transform.localScale = scaleRef.localScale;
    }
}
