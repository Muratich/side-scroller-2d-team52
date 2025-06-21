using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private float yOffset = 1f;
    [SerializeField] private float zOffset = 10f;
    private Transform target;
    public bool isMoving;

    void Start() {
        isMoving = true;
        target = FindFirstObjectByType<Movement>().transform;
        if (target == null) Debug.LogError("Target object for Camera not founded!");
    }
    void LateUpdate() {
        if (isMoving && target != null) {
            Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -zOffset);
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }
}