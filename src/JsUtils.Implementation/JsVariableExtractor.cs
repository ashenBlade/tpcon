using System.Collections;
using System.Runtime.InteropServices.ComTypes;
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
                    yield return ReadVariableDeclaration();
                }
                else
                {
                    MoveNext();
                }
            }
        }

        private bool MoveNext() => JsTokenEnumerator.MoveNext();
        private JsToken Current => JsTokenEnumerator.Current;

        private JsVariable ReadVariableDeclaration()
        {
            ReadVar();
            var id = ReadIdentifier();
            ReadEquals();
            
            var type = ReadType();
            ReadSemicolon();
            return new JsVariable(id.Name, type);
        }

        private void ReadSemicolon()
        {
            if (Current is not JsSemicolon)
                throw new Exception($"Expected ';' got {Current.Name}");
            MoveNext();
        }

        private JsTypes.JsType ReadType()
        {
            JsTypes.JsType type;
            if (Current is JsType jsType)
            {
                type = jsType.ToJsType();
            }
            else if (Current is JsNew)
            {
                type = ReadObjectDeclaration();
            }
            else
            {
                throw new Exception("Expected basic type or object");
            }
            
            MoveNext();
            return type;
        }

        private JsTypes.JsType ReadObjectDeclaration()
        {
            ReadNew();
            var objectClassName = ReadIdentifier();
            ReadLeftParenthesis();
            if (objectClassName.Name == "Array")
            {
                var array = new JsArray();
                while (Current is not JsRightParenthesis)
                {
                    var type = ReadType();
                    array.Add(type);
                    if (Current is JsComma)
                    {
                        break;
                    }
                }

                return array;
            }

            var obj = new JsObject();
            while (Current is not JsRightParenthesis)
            {
                ReadType();
                if (Current is JsComma)
                {
                    break;
                }
            }
            return obj;
        }

        private void ReadLeftParenthesis()
        {
            if (Current is not JsLeftParenthesis)
            {
                throw new Exception($"Expected '(' got {Current.Name}");
            }

            MoveNext();
        }

        private void ReadNew()
        {
            if (Current is not JsNew)
            {
                throw new Exception($"Expected 'new' got {Current.Name}");
            }

            MoveNext();
        }

        private void ReadEquals()
        {
            if (Current is not JsEquals equals)
            {
                throw new Exception($"Expected '=' got {Current.Name}");
            }

            MoveNext();
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