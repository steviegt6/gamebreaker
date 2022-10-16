using System.Text;

namespace GameBreaker.Abstractions.Serialization
{
    public interface IPositionable
    {
        Encoding Encoding { get; }

        ulong Length { get; set; }

        ulong Position { get; set; }
    }
}