namespace GameBreaker.Serial.IO;

// TODO: int32 only covers so much - files greater than two gigs are tough...
public interface IPositionable {
    int Position { get; set; }

    int Length { get; }
}
