using System.Text;

namespace GameBreaker.Serial.IO;

public interface IEncodable {
    Encoding Encoding { get; }
}
