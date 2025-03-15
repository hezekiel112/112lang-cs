sealed class Program {
    static void Main() {
        string code =
            @"VAR FOO %123+123%
            OSD [FOO]
VAR bar %hw%
            OSD [bar]";

        Lexer lexer = new Lexer(code);
        var tokens = lexer.Lex();

        Parser parser = new Parser(tokens);
        var nodes = parser.Parse();

        Interpreter interpreter = new Interpreter(nodes);
        interpreter.Run();
    }
}