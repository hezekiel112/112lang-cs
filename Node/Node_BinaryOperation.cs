public sealed class Node_BinaryOperation : Node {
    public Node Left {
        get;
    }

    public string Operator {
        get;
    }

    public Node Right {
        get;
    }

    public Node_BinaryOperation(Node left, string op, Node right, int position) : base(position) {
        Left = left;
        Operator = op;
        Right = right;
    }
}
