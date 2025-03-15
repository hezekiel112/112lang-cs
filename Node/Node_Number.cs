public sealed class Node_Number : Node {
    public string Value {
        get;
    }

    public Node_Number(string value, int position) : base(position) {
        Value = value;
    }
}