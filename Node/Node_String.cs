public sealed class Node_String : Node {
    public string Value {
        get;
    }

    public Node_String(string value, int position) : base(position) {
        Value = value;
    }
}
