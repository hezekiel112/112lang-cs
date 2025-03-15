public sealed class Node_OSD : Node {
    public List<string> Outputs {
        get;
    }

    public Node_OSD(List<string> outputs, int position) : base(position) {
        Outputs = outputs;
    }
}