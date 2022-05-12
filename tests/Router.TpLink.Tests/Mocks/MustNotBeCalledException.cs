using System;
using System.Runtime.CompilerServices;

namespace Router.TpLink.Tests;

public class MustNotBeCalledException : Exception
{
    public MustNotBeCalledException([CallerMemberName]string name = "")
     : base($"{name} must not be called")
    { }
}