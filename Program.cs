sealed class Program {
    static void Main() {
        string code =
            @"VAR FOO %Hello%
            OSD TEST
            OSD [FOO] [FOO] [FOO] [FOO] [FOO]";

        Lexer lexer = new Lexer(code);
        var tokens = lexer.Lex();

        Parser parser = new Parser(tokens);
        var nodes = parser.Parse();

        Interpreter interpreter = new Interpreter(nodes);
        interpreter.Run();
    }
}