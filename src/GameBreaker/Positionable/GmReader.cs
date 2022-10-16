using System.Diagnostics;

namespace GameBreaker.Positionable
{
    public class GmReader : BufferedReader
    {
        public override bool ReadBoolean() {
            Debug.Assert(Position + 1 <= Length, "ReadBoolean: Read out of bounds.");
            int val = ReadInt32();
            Debug.Assert(val is 0 or 1, $"ReadBoolean: Value was not 0 or 1 ({val}).");
            return val != 0;
        }
    }
}