using System;

namespace Router.Commands.Exceptions;

public class UnknownCommandLineException : CommandLineException
{
    public UnknownCommandLineException(string[]? commands = null, string? message = null) 
        : base(commands ?? Array.Empty<string>(), message) 
    { }
}