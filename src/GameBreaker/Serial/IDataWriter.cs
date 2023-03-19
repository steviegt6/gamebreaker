using System.Collections.Generic;
using GameBreaker.Models;

namespace GameBreaker.Serial;

// TODO: Reconsider mandatory properties, summaries, etc.
/// <summary>
///     An extension to <see cref="IBinaryWriter"/> which provides additional
///     methods for writing GameMaker data.
/// </summary>
public interface IDataWriter : IBinaryWriter {
    /// <summary>
    ///     The <see cref="GMData"/> instance this writer is working with.
    /// </summary>
    GMData Data { get; }

    /// <summary>
    ///     The <see cref="GMData.GMVersionInfo"/> instance this writer is
    ///     working with.
    /// </summary>
    GMData.GMVersionInfo VersionInfo { get; }

    // These properties probably need to get moved or dealt with elsewhere
    // somehow.
    List<GMWarning> Warnings { get; }

    Dictionary<IGMSerializable, int> PointerOffsets { get; }

    Dictionary<
        GMVariable,
        List<(int, GMCode.Bytecode.Instruction.VariableType)>
    > VariableReferences { get; }

    Dictionary<
        GMFunctionEntry,
        List<(int, GMCode.Bytecode.Instruction.VariableType)>
    > FunctionReferences { get; }

    /// <summary>
    ///     Writes the <see cref="Data"/> to the buffer.
    /// </summary>
    void Write();

    /// <summary>
    ///     Writes a GameMaker-style string to the buffer.
    /// </summary>
    /// <param name="value">The GameMaker-style string to write.</param>
    void WriteGMString(string value);

    /// <summary>
    ///     Writes a 32-bit pointer value in the current position for an object.
    /// </summary>
    /// <param name="obj"></param>
    void WritePointer(IGMSerializable obj);
    
    /// <summary>
    ///     Writes a 32-bit pointer value in the current position for a string.
    /// </summary>
    /// <param name="obj"></param>
    void WritePointerString(GMString obj);

    /// <summary>
    ///     Sets the current offset to be the pointer location for the specified
    ///     object.
    /// </summary>
    /// <param name="obj"></param>
    void WriteObjectPointer(IGMSerializable obj);
}
