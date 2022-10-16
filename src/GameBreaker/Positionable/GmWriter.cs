namespace GameBreaker.Positionable
{
    public class GmWriter : BufferedWriter
    {
        public override void Write(bool value) {
            // Int32
            Write(value ? 1 : 0);
        }
    }
}