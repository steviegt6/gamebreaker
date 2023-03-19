using System;

namespace GameBreaker.Exceptions;

public abstract class GmDeserializerExceptionException : GmIoException {
    protected GmDeserializerExceptionException() { }

    protected GmDeserializerExceptionException(string message) : base(message) { }

    protected GmDeserializerExceptionException(string message, Exception inner) :
        base(message, inner) { }
}

public class InvalidFormHeaderException : GmDeserializerExceptionException {
    public InvalidFormHeaderException() { }

    public InvalidFormHeaderException(string message) : base(message) { }

    public InvalidFormHeaderException(string message, Exception inner) :
        base(message, inner) { }
}
