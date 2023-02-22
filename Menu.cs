using StereoKit;


interface MenuUI<T> where T : Node<T> {
    void Open(T n);
}

class MenuSK : MenuUI<NodeSK> {

    public Pose pos;
    public NodeSK? nodeSelected;
    public void Open(NodeSK n) {
        pos.position = n.pos.position + n.pos.orientation.Rotate(new Vec3(-0.2f, 0.0f, 0.0f));
        nodeSelected = n;
    }

    public void Step() {
        pos.orientation = Quat.LookAt(pos.position, Input.Head.position);
        UI.WindowBegin(nodeSelected == null ? "Open some node" : "Node info", ref pos);
        if (nodeSelected != null) {
            if (nodeSelected.text != "") {
                UI.Text("Text");
                UI.HSeparator();
                UI.Text(nodeSelected.text); // TODO
                UI.HSeparator();
            }
            // UI.Button("");
            // UI.SameLine();
            if (nodeSelected.parent != null) {
                bool deleted = UI.Button("Delete");
                if (deleted) {
                    nodeSelected.parent.children.Remove(nodeSelected);
                    nodeSelected = null;
                    return;
                }
            }
            // UI.SameLine();
            // UI.Button("");
        }
        UI.WindowEnd();
    }
}