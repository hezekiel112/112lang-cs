public sealed class Node_Concat : Node {
    public Node Left {
        get;
    }
    public Node Right {
        get;
    }

    public Node_Concat(Node left, Node right, int position) : base(position) {
        Left = left;
        Right = right;
    }
}
