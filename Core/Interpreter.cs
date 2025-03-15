using System.Xml.Linq;

public sealed class Interpreter(List<Node> nodes) {
    public List<Node> Nodes {

        get;
    } = nodes;

    private int _position = 0;

    private Dictionary<string, string> _variablesMap = new Dictionary<string, string>();

    public int Run() {
        while (_position < Nodes.Count) {
            Node node = Nodes[_position];

            switch (node) {
                case Node_OSD osd:
                    for (int i = 0; i < osd.Outputs.Count; i++) {
                        string? output = osd.Outputs[i];

                        if (output.StartsWith("___")) {
                            string variableName = output.Substring(3);
                            Console.Write(GetVariable(variableName)+" "); // Ajoute un espace entre chaque élément
                            Console.Write("\n");
                        }
                        else {
                            Console.Write(output+" ");
                            Console.Write("\n"); // Affiche directement l'élément
                        }
                    }

                    break;

                case Node_Variable variable:
                    Console.WriteLine(variable.VariableExpression);

                    object evaluatedValue = Evaluate(variable.VariableExpression);

                    if (evaluatedValue is string strValue) {
                        TryAddValue(variable.VariableName, strValue);
                    }
                    else if (evaluatedValue is int intValue) {
                        TryAddValue(variable.VariableName, intValue.ToString());
                    }
                    else {
                        throw new Exception($"Type non géré pour la variable {variable.VariableName}");
                    }
                    break;

                case Node_BinaryOperation binaryOperation:
                    Console.WriteLine(Evaluate(binaryOperation));
                    break;
            }

            _position++;
        }

        return 0;
    }

    private object Evaluate(Node node) {
        return node switch {
            Node_Digit digit => digit.Value, // Retourne un entier
            Node_String str => str.Value,   // Retourne une chaîne
            Node_BinaryOperation binOp => EvaluateBinary(binOp), // Gère l'addition
            Node_Concat concat => EvaluateConcat(concat), // Gère la concaténation
            _ => throw new Exception("Expression invalide")
        };
    }
    private string EvaluateConcat(Node_Concat concat) {
        return Evaluate(concat.Left).ToString() + Evaluate(concat.Right).ToString();
    }

    private int EvaluateBinary(Node_BinaryOperation binOp) {
        int left = Convert.ToInt32(Evaluate(binOp.Left));
        int right = Convert.ToInt32(Evaluate(binOp.Right));

        return binOp.Operator switch {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => right != 0 ? left / right : throw new DivideByZeroException(),
            _ => throw new Exception($"Opérateur non reconnu : {binOp.Operator}")
        };
    }

    public string GetVariable(string variableName) {
        if (_variablesMap.ContainsKey(variableName))
            return _variablesMap[variableName];

        throw new Exception("variable " + variableName + " does not exist in the current context");
    }

    private bool TryAddValue(string variableName, string variableValue) {
        if (_variablesMap.TryAdd(variableName, variableValue)) {
            return true;
        }

        Console.WriteLine("error while adding " + variableName);
        return false;
    }
}