> **Warning** — GameBreaker is still under heavy, active development. Expect things to shift around often.

# GameBreaker

> GameMaker reverse-engineering tooling, done right.

---

GameBreaker is a GameMaker reverse-engineering and modding toolchain written in C# for .NET 6.

Taking into consideration previous projects' (subjective) successes and failures, GameBreaker intends to create a powerful, efficient, fast toolchain that reverse-engineering and modding GameMaker games significantly easier.

Currently planned is a reliable deserialization and serialization system. If all goes well, decompilation- and compilation-related projects will undergo experimentation. A free (as in free speech and free beer) desktop runner exists in an idealistic future as well.

## Progress

- [ ] IFF/chunk library.
  - [ ] IFF/chunk abstractions.
    - [x] Funamental layout.
    - [ ] Plan out extensibility structure to abstract away GameMaker-specific behavior.
  - [ ] Extensibility structure to abstract away GameMaker-specific behavior.
- [ ] Core (de)serialization library.
  - [ ] Core (de)serialization abstractions.
    - [x] `IGMSerializable` + basic layout for handling deserializable data.
    - [ ] Plan out extensibility structure to abstract away version-dependent behavior.
  - [ ] Extensibility structure to abstract away version-dependent behavior.
- [ ] GMS modification library.
  - [ ] Chunk (de)serialization.
    - [ ] Proper version detection.
    - [ ] Account for conditional chunk padding.
    - [x] `GEN8` chunk (de)serialization.

## Examples

> For a more detailed list of examples, see various usages in `./src/GameBreaker.Tests/` and `./GameBreaker.Examples/`.

### Simple deserialization of a GameMaker IFF file.

```cs
// The IChunkFileMetadata object helps to control differences in versions, so a specific class is not used as an example here.
var file = new GameMakerFile(new ChunkFileMetadata());
var stream = ...; // Any stream containing the bytes of the IFF file to deserialize will do.
var deserializer = new GmDataDeserializer(new GmReader(stream)); // Create a new deserializer encapsulating a reader that reads the IFF stream.

// Deserialize the IFF file. This encapsulates the deserialized data from the deserializer in the GameMakerFile (IChunkedFile instance).
file.Deserialize(deserializer);
```

## Building

```sh
git clone https://github.com/steviegt6/gamebreaker
cd ./gamebreaker/src/
dotnet build Tomat.GameBreaker.sln -c Release
```

## Licensing

GameBreaker utilizes research and code by myself and others, see [COPYING.md](COPYING.md) for peoples' hard work and relevant software licenses.
