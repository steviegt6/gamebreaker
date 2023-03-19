using System;
using System.IO;

namespace GameBreaker.Exceptions;

/// <summary>
///     Represents an exception that is thrown by GameBreaker.
///     <br />
///     This is used to differentiate between exceptions thrown by GameBreaker
///     and exceptions thrown by other libraries. As of now, it exists purely as
///     a convenience.
///     <br />
///     See implementors for actual exceptions.
/// </summary>
public interface IGmException { }

public abstract class GmIoException : IOException,
                                      IGmException {
    protected GmIoException() { }

    protected GmIoException(string message) : base(message) { }

    protected GmIoException(string message, Exception inner) : base(
        message,
        inner
    ) { }
}
