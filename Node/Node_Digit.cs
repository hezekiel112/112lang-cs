public sealed class Node_Digit : Node {
    public int Value {
        get;
    }

    public Node_Digit(int value, int position) : base(position) {
        Value = value;
    }
}
