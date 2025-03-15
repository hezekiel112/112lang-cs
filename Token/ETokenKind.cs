public enum ETokenKind {
    /// <summary>
    /// Kind : OSD, VAR, JMP ...
    /// </summary>
    KEYWORD,

    /// <summary>
    /// Kind : %foo%, %bar%
    /// </summary>
    VARIABLE,

    /// <summary>
    /// Kind : 123, 321, 000
    /// </summary>
    DIGIT,

    /// <summary>
    /// Kind : +, -, /, ++, --, ...
    /// </summary>
    OPERATOR,

    /// <summary>
    /// Kind : assume code is at end
    /// </summary>
    EOF,
    LITERAL,
    REFERENCE,
    PARENTHESIS,
    UNARY_OPERATION,
    ARITHMETIC_OPERATION,
}
