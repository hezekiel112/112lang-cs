public sealed class Interpreter(List<Node> nodes) {
    public List<Node> Nodes {

        get;
    } = nodes;

    private int _position = 0;

    private Dictionary<string, object> _variablesMap = new Dictionary<string, object>();

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
                    TryAddValue(variable.VariableName, variable.VariableValue);
                    break;
            }

            _position++;
        }

        return 0;
    }

    public object GetVariable(string variableName) {
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