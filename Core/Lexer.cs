public sealed class Lexer(string source) : IIterator<char> {
    /// <summary>
    /// Plain code script
    /// </summary>
    public string Source {
        get;
    } = source;

    /// <summary>
    /// Store the <seealso cref="Source"/> character position
    /// </summary>
    public int Position {

        get; set;
    }

    /// <summary>
    /// Store the current character of <seealso cref="Source"/> depending of <seealso cref="Position"/>
    /// </summary>
    public char Current => Position < Source.Length ? Source[Position] : '\0';

    private List<Token> _tokens = new List<Token>();

    public List<Token> Lex() {
        while (Position < Source.Length) {
            if (char.IsWhiteSpace(Current)) {
                Next();

                continue;
            }

            if (char.IsLetter(Current)) {
                string keyword = string.Empty;

                #region LEXING KEYWORD
                while (char.IsLetterOrDigit(Current)) {
                    keyword += Current;
                    Next();
                }

                switch (keyword) {
                    case Code.KEYWORD_OSD:
                    case Code.KEYWORD_VAR:
                        _tokens.Add(new Token(keyword, ETokenKind.KEYWORD, Position));
                        break;

                    default:
                        _tokens.Add(new Token(keyword, ETokenKind.LITERAL, Position));
                        break;
                }

                continue;
                #endregion
            }

            #region LEXING OPERATOR 
            if ("+-*/!&|".Contains(Current)) {
                char op = Current;
                char aheadOf = LookAhead();
                Next();

                string operatorValue = op.ToString();

                if (op == '+' && aheadOf == '+') {
                    operatorValue += aheadOf;
                    Next();
                }

                Console.WriteLine(operatorValue);
                _tokens.Add(new Token(operatorValue, ETokenKind.OPERATOR, Position));

                continue;
            }
            #endregion

            #region LEXING VARIABLE
            if ("%".Contains(Current)) {
                Next();
                string variable = string.Empty;

                while (Current != '%' && Current != '\0') {
                    variable += Current;
                    Next();
                }

                if (Current.Equals('%')) {
                    Next();
                    _tokens.Add(new Token(variable, ETokenKind.VARIABLE, Position));
                }
                else {
                    throw new Exception("missing % ending segment for variable declaration statement");
                }

                continue;
            }
            #endregion

            #region LEXING REFERENCE
            if ("[".Contains(Current)) {
                Next();
                string reference = string.Empty;

                while (Current != ']' && Current != '\0') {
                    reference += Current;
                    Next();
                }

                if (Current.Equals(']')) {
                    Next();
                    _tokens.Add(new Token(reference, ETokenKind.REFERENCE, Position));
                }
                else {
                    throw new Exception("missing ] ending segment for reference declaration statement");
                }

                continue;
            }
            #endregion
        }

        _tokens.Add(new Token(string.Empty, ETokenKind.EOF, Position));
        return _tokens;
    }

    public void Next() => Position++;

    public char LookAhead() {
        return (Position + 1 < Source.Length) ? Source[Position + 1] : '\0';
    }
}
