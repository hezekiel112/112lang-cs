public sealed class Parser(List<Token> tokens) : IIterator<Token> {
    /// <summary>
    /// Store the <seealso cref="t"/> character position
    /// </summary>
    public int Position {

        get; set;
    }

    /// <summary>
    /// Store the current character of <seealso cref="Tokens"/> depending of <seealso cref="Position"/>
    /// </summary>
    public Token Current => Position < Tokens.Count ? Tokens[Position] : new Token(string.Empty, ETokenKind.EOF, Position);

    public List<Token> Tokens {

        get;
    } = tokens;

    public void Next() => Position++;

    public List<Node> Parse() {
        List<Node> nodes = new List<Node>();

        while (Current.Kind != ETokenKind.EOF) {
            switch (Current.Value) {
                case Code.KEYWORD_OSD:
                    Next();
                    List<string> outputs = new List<string>();

                    while (Current.Kind == ETokenKind.REFERENCE) {
                        outputs.Add($"___{Current.Value}");
                        Next();
                    }

                    while (Current.Kind == ETokenKind.LITERAL) {
                        outputs.Add($"{Current.Value}");
                        Next();
                    }

                    nodes.Add(new Node_OSD(outputs, Position));
                    break;
                case Code.KEYWORD_VAR:
                    Next();
                    string variableName = string.Empty;
                    string variableValue = string.Empty;

                    if (Current.Kind == ETokenKind.LITERAL) {
                        variableName = Current.Value;
                        Next();
                    }

                    if (Current.Kind == ETokenKind.VARIABLE) {
                        variableValue = Current.Value;
                        Next();
                    }

                    nodes.Add(new Node_Variable(variableValue, variableName, Position));
                    break;
                case Code.OPERATOR_PLUS:

                    break;
            }
        }

        return nodes;
    }

    public Token LookAhead() {
        return (Position + 1 < Tokens.Count) ? Tokens[Position + 1] : new Token(string.Empty, ETokenKind.EOF, Position);
    }
}