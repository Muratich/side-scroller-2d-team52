using Unity.Netcode;
using UnityEngine.Events;

public abstract class Puzzle : NetworkBehaviour {
    public UnityEvent onFinish;
    public override void OnNetworkSpawn() {
        PuzzleStart();
        var manager = FindAnyObjectByType<PuzzleManager>();
        if (manager != null)
            onFinish.AddListener(manager.PuzzleFinish);
    }

    public abstract void PuzzleStart();
    public virtual void PuzzleFinish() {
        onFinish.Invoke();
        if (IsServer) {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
