using System.Collections;
using UnityEngine;

public class WhiteShade : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    [SerializeField] private Color shadeColor = Color.red;
    private float stopTime;
    private Color defaultColor = Color.white;

    public void Awake() {
        if (spriteRenderer == null) { Debug.LogError("Sprtie renderer not set to WhiteShade");  return; }
        defaultColor = spriteRenderer.color;
    }

    public void Shade() {
        spriteRenderer.color = shadeColor;
        stopTime = Time.time + 0.5f;
        StartCoroutine(ShadeCor());
    }

    IEnumerator ShadeCor() {
        while (Time.time < stopTime) {
            yield return null;
        }
        spriteRenderer.color = defaultColor;
    }
}
