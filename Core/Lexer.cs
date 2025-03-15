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

            // 🔹 Identifier les mots-clés et les littéraux
            if (char.IsLetter(Current)) {
                string keyword = string.Empty;

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
            }

            // 🔹 Identifier les opérateurs
            if ("+-*/()".Contains(Current)) {
                _tokens.Add(new Token(Current.ToString(), ETokenKind.OPERATOR, Position));
                Next();
                continue;
            }

            // 🔹 Identifier les nombres
            if (char.IsDigit(Current)) {
                string number = string.Empty;
                while (char.IsDigit(Current) || Current == '.') {
                    number += Current;
                    Next();
                }
                _tokens.Add(new Token(number, ETokenKind.DIGIT, Position));
                continue;
            }

            // 🔹 Identifier les expressions entre `% %`
            if (Current == '%') {
                Next(); // Passer le premier `%`
                string expression = string.Empty;
                List<Token> expressionTokens = new List<Token>();

                while (Current != '%' && Current != '\0') {
                    if (char.IsDigit(Current)) {
                        string number = "";
                        while (char.IsDigit(Current)) {
                            number += Current;
                            Next();
                        }
                        expressionTokens.Add(new Token(number, ETokenKind.DIGIT, Position));
                    }
                    else if ("+-*/".Contains(Current)) {
                        expressionTokens.Add(new Token(Current.ToString(), ETokenKind.OPERATOR, Position));
                        Next();
                    }
                    else {
                        // Si ce n'est pas un chiffre ni un opérateur, on considère que c'est du texte
                        string text = "";
                        while (Current != '%' && !"0123456789+-*/".Contains(Current) && Current != '\0') {
                            text += Current;
                            Next();
                        }
                        expressionTokens.Add(new Token(text, ETokenKind.LITERAL, Position));
                    }
                }

                if (Current == '%') {
                    Next(); // Passer le second `%`
                    foreach (var t in expressionTokens) {
                        _tokens.Add(t);
                    }
                }
                else {
                    throw new Exception("Erreur : expression entre % manquante de fermeture.");
                }
                continue;
            }

            // 🔹 Identifier les références entre `[ ]`
            if (Current == '[') {
                Next();
                string reference = string.Empty;

                while (Current != ']' && Current != '\0') {
                    reference += Current;
                    Next();
                }

                if (Current == ']') {
                    Next();
                    _tokens.Add(new Token(reference, ETokenKind.REFERENCE, Position));
                }
                else {
                    throw new Exception("Erreur : référence fermante `]` manquante.");
                }
                continue;
            }
        }

        _tokens.Add(new Token(string.Empty, ETokenKind.EOF, Position));
        return _tokens;
    }

    public void Next() => Position++;

    public char LookAhead() {
        return (Position + 1 < Source.Length) ? Source[Position + 1] : '\0';
    }
}
