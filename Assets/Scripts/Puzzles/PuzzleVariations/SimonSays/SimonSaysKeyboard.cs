using UnityEngine;

public class SimonSaysKeyboard : MonoBehaviour {
    [SerializeField] private SimonSays puzzle;

    private void Start() {
        for (int i = 0; i < puzzle.colorButtons.Count; i++) {
            int idx = i;
            puzzle.colorButtons[i].onClick.AddListener(() => puzzle.OnColorButtonPressed(idx));
        }
    }
}
