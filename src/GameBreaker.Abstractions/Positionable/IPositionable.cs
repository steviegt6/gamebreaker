using System.Text;

namespace GameBreaker.Abstractions.Positionable
{
    public interface IPositionable
    {
        Encoding Encoding { get; }

        ulong Length { get; set; }

        ulong Position { get; set; }
    }
}