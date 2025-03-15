public sealed class Node_Expression : Node {
    public List<Node> Elements {
        get;
    }

    public Node_Expression(List<Node> elements, int position) : base(position) {
        Elements = elements;
    }
}
