using GameBreaker.Serial.IO;

namespace GameBreaker.Serial.Extensions; 

public static class PositionableExtensions {
    public static void Align(this IPositionable p, int alignment) {
        if (p.Position % alignment != 0)
            p.Position += alignment - (p.Position % alignment);
    }
}
