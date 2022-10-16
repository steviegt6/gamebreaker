namespace GameBreaker.Abstractions.Serialization
{
    public interface IGmDataReader
    {
        IGmData Data { get; }

        IPositionableReader Reader { get; }

        void DeserializeData();
    }
}