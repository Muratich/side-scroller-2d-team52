using UnityEngine;
using UnityEngine.UI;

public class PuzzleItem : MonoBehaviour {
    public Button button;
    public GameObject cover;
    public Image artworkImage;

    public Sprite[] variationSprites;
    private Memo memo;

    void Awake() {
        memo = FindObjectOfType<Memo>();
        button.onClick.AddListener(() => {int currentIndex = transform.GetSiblingIndex(); memo.RequestFlipServerRpc(currentIndex);});
    }

    public void SetValue(int v) {
        if (variationSprites != null && v >= 0 && v < variationSprites.Length)
            artworkImage.sprite = variationSprites[v];
    }

    public void ApplyState(byte state) {
        switch (state) {
            case 0:
                cover.SetActive(true);
                button.interactable = true;
                break;
            case 1:
                cover.SetActive(false);
                button.interactable = true;
                break;
            case 2:
                cover.SetActive(false);
                button.interactable = false;
                break;
            default:
                Debug.LogError("Undefined state!");
                break;
        }
    }
}
