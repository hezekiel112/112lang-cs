
/// <summary>
/// Reprensent an data object inside the code
/// </summary>
/// <param name="value">Example : VAR, FOO, %5%</param>
/// <param name="kind">Example : VAR is <seealso cref="ETokenKind.KEYWORD"/></param>
/// <param name="position">Example : Line position in the code where the token has been created</param>
public class Token(string value, ETokenKind kind, int position) {
    public string Value {

        get;
    } = value;

    public ETokenKind Kind {

        get;
    } = kind;

    public int Position {

        get;
    } = position;
}
