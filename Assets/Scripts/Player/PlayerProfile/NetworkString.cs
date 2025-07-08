using Unity.Netcode;
public struct NetworkString : INetworkSerializable {
    public string Value;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref Value);
    }

    public NetworkString(string value) {
        Value = value;
    }
}
