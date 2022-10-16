namespace GameBreaker.Abstractions.Serialization
{
    public interface IGmDataWriter
    {
        IGmData Data { get; }

        IPositionableWriter Writer { get; }

        void SerializeData();
    }
}