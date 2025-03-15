public sealed class Node_Variable : Node {
    public string VariableValue {
        get;
    }

    public string VariableName {
        get;
    }

    public Node_Variable(string variableValue, string variableName, int position) : base(position) {
        VariableValue = variableValue;
        VariableName = variableName;
    }
}