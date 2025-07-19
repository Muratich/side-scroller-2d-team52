using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretNumberKeyboard : MonoBehaviour {
    [SerializeField] private List<Button> numberButtons;
    [SerializeField] private SecretNumber puzzle;

    private List<int> currentGuess = new List<int>();

    private void Start() {
        for (int i = 0; i < numberButtons.Count; i++) {
            int digit = i;
            numberButtons[i].onClick.AddListener(() => OnNumberPressed(digit));
        }
    }

    private void OnNumberPressed(int digit)
    {
        if (!puzzle) return;
        if (currentGuess.Count > puzzle.SecretLength) return;
        int idx = currentGuess.Count;
        puzzle.PreviewGuessServerRpc(idx, digit);
        currentGuess.Add(digit);
        if (currentGuess.Count == puzzle.SecretLength)
        {
            puzzle.OnKeyboardInput(currentGuess.ToArray());
            currentGuess.Clear();
        }
    }
}
