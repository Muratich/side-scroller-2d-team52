using UnityEngine;

public class ReverseLock : MonoBehaviour {
    public Transform parent;
    public Vector3 desiredScale = new Vector3(1,1,1);

    void LateUpdate() {
        if (parent != null) {
            Vector3 pScale = parent.lossyScale;
            transform.localScale = new Vector3(desiredScale.x / pScale.x, desiredScale.y / pScale.y, desiredScale.z / pScale.z);
        }
    }

}
