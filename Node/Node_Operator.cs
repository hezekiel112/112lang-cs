public sealed class Node_Operator : Node {
    public string Operator {
        get;
    }

    public Node_Operator(string op, int position) : base(position) {
        Operator = op;
    }
}
