using UnityEngine;

public class MusicSwitcher : MonoBehaviour {
    public AudioSource calm;
    public AudioSource boss;

    public void Start() {
        boss.gameObject.SetActive(false);
        calm.gameObject.SetActive(true);
    }
    
    public void SwitchToBoss(bool isOn) {
        calm.gameObject.SetActive(!isOn);
        boss.gameObject.SetActive(isOn);
    }
}
