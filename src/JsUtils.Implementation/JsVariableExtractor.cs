using System.Collections;

using JsTypes;
using JsUtils.Implementation.JsTokens;
using JsType = JsUtils.Implementation.JsTokens.JsType;

namespace JsUtils.Implementation;

public class JsVariableExtractor : IJsVariableExtractor
{
    private ITokenizer Tokenizer { get; }

    public JsVariableExtractor(ITokenizer tokenizer)
    {
        Tokenizer = tokenizer;
    }
    
    public IEnumerable<JsVariable> ExtractVariables(string script)
    {
        using var extractor = new VariableStatementExtractor(Tokenizer.Tokenize(script));
        return extractor.ToList();
    }

    private class VariableStatementExtractor : IDisposable, IEnumerable<JsVariable>
    {
        public IEnumerator<JsToken> JsTokenEnumerator { get; }
        public VariableStatementExtractor(IEnumerable<JsToken> jsTokens)
        {
            JsTokenEnumerator = jsTokens.GetEnumerator();
        }

        public void Dispose()
        {
            JsTokenEnumerator?.Dispose();
        }
        
        public IEnumerator<JsVariable> GetEnumerator()
        {
            while (JsTokenEnumerator.MoveNext())
            {
                var token = JsTokenEnumerator.Current;
                if (token is JsVar)
                {
                    yield return ReadVariable();
                }
                else
                {
                    MoveNext();
                }
            }
        }

        private bool MoveNext() => JsTokenEnumerator.MoveNext();
        private JsToken Current => JsTokenEnumerator.Current;

        private JsVariable ReadVariable()
        {
            ReadVar();
            var id = ReadIdentifier();
            ReadEquals();
            var type = ReadType();
            ReadSemicolon();
            return new JsVariable(id.Name, type.ToJsType());
        }

        private void ReadSemicolon()
        {
            if (Current is not JsSemicolon)
                throw new Exception($"Expected ';' got {Current.Name}");
            MoveNext();
        }

        private JsType ReadType()
        {
            if (Current is not JsType type)
            {
                throw new Exception($"Expected JsType(number, string, regex) got {Current.Name}");
            }

            MoveNext();
            return type;
        }

        private JsEquals ReadEquals()
        {
            if (Current is not JsEquals equals)
            {
                throw new Exception($"Expected '=' got {Current.Name}");
            }

            MoveNext();
            return equals;
        }

        private JsIdentifier ReadIdentifier()
        {
            if (Current is not JsIdentifier identifier)
            {
                throw new Exception($"Expected identifier got {Current.Name}");
            }
            
            MoveNext();
            return new JsIdentifier(identifier.Name);
        }

        private void ReadVar()
        {
            if (Current is not JsVar)
            {
                throw new Exception($"Expected 'var' got '{Current.Name}'");
            }
            MoveNext();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}