using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Router.Commands.Utils.Formatters;
using Xunit;

namespace Router.Commands.Utils.Tests;

public class FormattingTests
{
    private class OnlyPrimitiveObjects
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    private class OnlyComplex
    {
        public OnlyPrimitiveObjects Primitives { get; set; }
    }
    
    private static KeyValuePair<string, string>[] Extract(object obj) 
        => Formatting.extractState(obj)
                     .ToArray();

    public static IEnumerable<object[]> ObjectsWithOnlyPrimitiveProperties =>
        new[]
        {
            new object[]
            {
                new OnlyPrimitiveObjects {Name = "Some name", Value = 1}
            }
        };

    [Theory(DisplayName = "With primitive types; Should return single pair")]
    [MemberData(nameof(ObjectsWithOnlyPrimitiveProperties))]
    public void WithPrimitiveType_ReturnSingleValues(object primitive)
    {
        var actual = Extract(primitive);
        
    }
}