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
                    Node expression = new Node_Number("", Position); // Valeur par défaut

                    if (Current.Kind == ETokenKind.LITERAL) {
                        variableName = Current.Value;
                        Next();
                    }

                    expression = ParseExpression();

                    nodes.Add(new Node_Variable(variableName, expression, Position));
                    break;

            }

            switch (Current.Kind) {
                case ETokenKind.DIGIT:
                    nodes.Add(ParseExpression());
                    break;
            }
        }

        return nodes;
    }

    public Token LookAhead() {
        return (Position + 1 < Tokens.Count) ? Tokens[Position + 1] : new Token(string.Empty, ETokenKind.EOF, Position);
    }

    public Node ParseExpression() {
        Node expression = null;

        while (Current.Kind != ETokenKind.EOF && Current.Kind != ETokenKind.KEYWORD) {
            if (Current.Kind == ETokenKind.DIGIT) {
                Node right = new Node_Digit(int.Parse(Current.Value), Position);
                Next();

                if (expression == null) {
                    expression = right;
                }
                else {
                    expression = new Node_BinaryOperation(expression, "+", right, Position);
                }
            }
            else if (Current.Kind == ETokenKind.OPERATOR) {
                string op = Current.Value;
                Next();

                if (Current.Kind == ETokenKind.DIGIT) {
                    Node right = new Node_Digit(int.Parse(Current.Value), Position);
                    Next();
                    expression = new Node_BinaryOperation(expression, op, right, Position);
                }
                else {
                    throw new Exception("Opérateur sans chiffre valide après.");
                }
            }
            else if (Current.Kind == ETokenKind.LITERAL) {
                Node right = new Node_String(Current.Value, Position);
                Next();

                if (expression == null) {
                    expression = right;
                }
                else {
                    expression = new Node_Concat(expression, right, Position);
                }
            }
            else {
                break; // Fin de l'expression
            }
        }

        return expression;

    }
}