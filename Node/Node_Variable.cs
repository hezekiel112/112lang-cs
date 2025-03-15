public sealed class Node_Variable : Node {
    public string? VariableValue {
        get;
    }

    public string VariableName {
        get;
    }

    public Node? VariableExpression {
        get;
    }

    public Node_Variable(string variableName, string variableValue, int position) : base(position) {
        VariableName = variableName;
        VariableValue = variableValue;
    }

    public Node_Variable(string variableName, Node expression, int position) : base(position) {
        VariableName = variableName;
        VariableExpression = expression;
    }
}