using UnityEngine;

public class MusicSwitcher : MonoBehaviour {
    public AudioSource calm;
    public AudioSource boss;

    public void SwitchToBoss(bool isOn) {
        if (isOn) { calm.Stop(); boss.Play(); }
        else { boss.Stop();  calm.Play();  }
    }
}
