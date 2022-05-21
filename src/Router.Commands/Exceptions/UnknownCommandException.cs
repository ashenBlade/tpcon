using System;

namespace Router.Commands.Exceptions;

public class UnknownCommandException : CommandLineException
{
    public UnknownCommandException(string[]? commands = null, string? message = null) 
        : base(commands ?? Array.Empty<string>(), message) 
    { }
}